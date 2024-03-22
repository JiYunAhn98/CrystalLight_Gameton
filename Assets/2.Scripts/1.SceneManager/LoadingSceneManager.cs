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
        //    //�ʱ�ȭ �Ϸ�
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
            _login.SetActive(false);    // ���⼭ ��⿡ ����� ��ū�� ���������� �Ѵ�. �ڵ��α��� ����
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
            Debug.Log($"�ʱ�ȭ ���� : {bro}");
        }
        else
        {
            Debug.LogError($"�ʱ�ȭ ���� : {bro}");
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
        // ����Ƽ �÷��̾� ���ÿ� ������ ���� ����
        Version client = new Version(Application.version);
        Debug.Log("clientVersion: " + client);

#if UNITY_EDITOR
        // �ڳ� ���� ���� ��ȸ�� iOS / Android ȯ�濡���� ȣ�� �� �� �ֽ��ϴ�.
        Debug.Log("������ ��忡���� ���� ������ ��ȸ�� �� �����ϴ�.");
        return;
#endif

        // �ڳ� �ֿܼ��� ������ ���� ������ ��ȸ
        Backend.Utils.GetLatestVersion(callback => {
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("���� ������ ��ȸ�ϴµ� �����Ͽ����ϴ�.\n" + callback);
                return;
            }

            var version = callback.GetReturnValuetoJSON()["version"].ToString();
            Version server = new Version(version);

            var result = server.CompareTo(client);

            if (result == 0)
            {
                // 0 �̸� �� ������ ��ġ�ϴ� �� �Դϴ�.
                // �ƹ� �۾� ���ϰ� ����
                return;
            }
            else if (result < 0)
            {
                // 0 �̸��� ��� server ������ client ���� ���� ��� �Դϴ�.
                // ����/���� ���� �˼��� �־��� ��� ���⿡ �ش� �� �� �ֽ��ϴ�.
                // ex)
                // �˼��� ��û�� Ŭ���̾�Ʈ ������ 3.0.0, 
                // ���̺꿡 ������� Ŭ���̾�Ʈ ������ 2.0.0,
                // �ڳ� �ֿܼ� ����� ������ 2.0.0 

                // �ƹ� �۾��� ���ϰ� ����
                return;
            }
            // 0���� ũ�� server ������ Ŭ���̾�Ʈ ���� ������ �� �ֽ��ϴ�.
            else if (client == null)
            {
                // �� Ŭ���̾�Ʈ ���� ������ null�� ��쿡�� 0���� ū ���� ���ϵ� �� �ֽ��ϴ�.
                // �� ���� �ƹ� �۾��� ���ϰ� �����ϵ��� �ϰڽ��ϴ�.
                Debug.LogError("Ŭ���̾�Ʈ ���� ������ null �Դϴ�.");
                return;
            }

            // ������� ���� ���� ������ server ����(�ڳ� �ֿܼ� ����� ����)��
            // Ŭ���̾�Ʈ���� ���� ��� �Դϴ�.
            // ������ ������ ������Ʈ�� �ϵ��� ������Ʈ UI�� ����ݴϴ�.
            _updateCanvas.SetActive(true);
        });
    }
}
