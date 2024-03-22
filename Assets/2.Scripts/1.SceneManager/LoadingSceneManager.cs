using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using GoogleMobileAds;
//using GoogleMobileAds.Api;
using BackEnd;
using System;

public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] LoadingBar _bars;
    [SerializeField] BackendManager _backendManager;
    [SerializeField] GameObject _login;
    [SerializeField] UserInfo user;
    [SerializeField] GameObject _outCanvas;
    [SerializeField] GameObject _updateCanvas;

    eState _nowState;

    enum eState
    {
        BackendManagerPhase,
        GoogleSheetManagerPhase,
        BackendDataPhase,
        Finish
    }
    void Awake()
    {
        SoundManager._instance.Initialize(DefineHelper.eBGMSound.Loading);
        //MobileAds.Initialize((InitializationStatus initStatus) =>
        //{
        //    //초기화 완료
        //});
    }

    void Start()
    {
        if (!Complete())
        {
            _bars.gameObject.SetActive(true);
            _login.SetActive(false);

            _nowState = eState.BackendManagerPhase;
            BackendSetup();
            _backendManager.Initialize();
            StartCoroutine(_bars.BarMove(0.2f));

#if UNITY_IOS || UNITY_ANDROID
            Application.targetFrameRate = 60;
            Application.runInBackground = true;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            UpdateCheck();
#endif
        }
        else
        {
            _nowState = eState.Finish;
            _login.SetActive(false);    // 여기서 기기에 저장된 토큰을 지워버려야 한다. 자동로그인 방지
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _outCanvas.SetActive(true);
        }
        if (Backend.IsInitialized && _nowState == eState.BackendManagerPhase)
        {
            StartCoroutine(GoogleSheetManager._instance.SheetDataLoad());
            _nowState = eState.GoogleSheetManagerPhase;
        }

        if (GoogleSheetManager._instance._isComplete && _nowState == eState.GoogleSheetManagerPhase)
        {
            StopAllCoroutines();
            _bars.gameObject.SetActive(false);
            _login.SetActive(true);
            _nowState = eState.BackendDataPhase;
        }
        if (_nowState == eState.BackendDataPhase && Complete())
        {
            _nowState = eState.Finish;
            PrefabManager._instance.Initialize();
            InformManager._instance.Initialize();
        }
        if (_nowState == eState.Finish)
        {
            SceneLoadManager._instance.LoadScene("Lobby");
        }
    }

    public void BackendSetup()
    {
        var bro = Backend.Initialize(true);
        if (bro.IsSuccess())
        {
            Debug.Log($"초기화 성공 : {bro}");
        }
        else
        {
            Debug.LogError($"초기화 실패 : {bro}");
        }
    }

    public bool Complete()
    {
        return BackendChartData._isLoadFinish && user._isFinish &&
            BackendStoreData._isLoadFinish && BackendGameData._isLoadFinish;
    }
    public bool ADLoadComplete()
    {
        return ADManager.Instance._isFinish && ADManager.Instance._isBannerLoadFinish
            && ADManager.Instance._isInterstitialLoadFinish && ADManager.Instance._isRewardLoadFinish;
    }

    private void UpdateCheck()
    {
        // 유니티 플레이어 세팅에 설정한 버전 정보
        Version client = new Version(Application.version);
        Debug.Log("clientVersion: " + client);

#if UNITY_EDITOR
        // 뒤끝 버전 정보 조회는 iOS / Android 환경에서만 호출 할 수 있습니다.
        Debug.Log("에디터 모드에서는 버전 정보를 조회할 수 없습니다.");
        return;
#endif

        // 뒤끝 콘솔에서 설정한 버전 정보를 조회
        Backend.Utils.GetLatestVersion(callback => {
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("버전 정보를 조회하는데 실패하였습니다.\n" + callback);
                return;
            }

            var version = callback.GetReturnValuetoJSON()["version"].ToString();
            Version server = new Version(version);

            var result = server.CompareTo(client);

            if (result == 0)
            {
                // 0 이면 두 버전이 일치하는 것 입니다.
                // 아무 작업 안하고 리턴
                return;
            }
            else if (result < 0)
            {
                // 0 미만인 경우 server 버전이 client 보다 작은 경우 입니다.
                // 애플/구글 스토어에 검수를 넣었을 경우 여기에 해당 할 수 있습니다.
                // ex)
                // 검수를 신청한 클라이언트 버전은 3.0.0, 
                // 라이브에 운용중인 클라이언트 버전은 2.0.0,
                // 뒤끝 콘솔에 등록한 버전은 2.0.0 

                // 아무 작업을 안하고 리턴
                return;
            }
            // 0보다 크면 server 버전이 클라이언트 이후 버전일 수 있습니다.
            else if (client == null)
            {
                // 단 클라이언트 버전 정보가 null인 경우에도 0보다 큰 값이 리턴될 수 있습니다.
                // 이 때는 아무 작업을 안하고 리턴하도록 하겠습니다.
                Debug.LogError("클라이언트 버전 정보가 null 입니다.");
                return;
            }

            // 여기까지 리턴 없이 왔으면 server 버전(뒤끝 콘솔에 등록한 버전)이
            // 클라이언트보다 높은 경우 입니다.
            // 유저가 스토어에서 업데이트를 하도록 업데이트 UI를 띄워줍니다.
            _updateCanvas.SetActive(true);
        });
    }
}
