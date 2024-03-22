using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;
using DefineHelper;

public class ResultPopup : MonoBehaviour
{
    readonly float NORMAL_DEVIDE_NUM = 0.1f;
    readonly float FAST_DEVIDE_NUM = 0.12f;
    readonly float HARD_DEVIDE_NUM = 0.14f;

    [SerializeField] GameController _result;

    [Header("ResultPanel")]
    [SerializeField] GameObject _infinitePanel;
    [SerializeField] GameObject _tutorialPanel;

    [Header("InfiniteResult")]
    [SerializeField] Image _backgroundImg;
    [SerializeField] GameObject _resultSection;
    [SerializeField] TextMeshProUGUI _nowScore;
    [SerializeField] TextMeshProUGUI _maxScore;
    [SerializeField] TextMeshProUGUI _crystaltxt;
    [SerializeField] GameObject _congraturation;
    [SerializeField] GameObject _adBtn;

    [Header("TutorialResult")]
    [SerializeField] Image _hideOnBlack;
    [SerializeField] GameObject _success;
    [SerializeField] GameObject _failure;
    [SerializeField] GameObject _score;
    [SerializeField] TextMeshProUGUI _scoretxt;
    [SerializeField] Button _retryBtn;

    //    private RewardedAd rewardedAd;

    //    public void Start()
    //    {
    //        InitAds();
    //    }
    //    public void InitAds()
    //    {
    //        string adUnitId;
    //        //현재는 테스트앱ID > 앱 등록 후 구글애드몹에서 ID 가져와야함 
    //#if UNITY_ANDROID
    //        adUnitId = "ca-app-pub-8865941325495641/5690199935";
    //#else
    //        adUnitId = "unexpected_platform";
    //#endif

    //        RewardedAd.Load(adUnitId, new AdRequest.Builder().Build(), LoadCallback);
    //    }
    //    public void LoadCallback(RewardedAd rewardedAd, LoadAdError loadAdError)
    //    {
    //        if (rewardedAd != null)
    //        {
    //            this.rewardedAd = rewardedAd;
    //            Debug.Log("로드성공");
    //        }
    //        else
    //        {
    //            Debug.Log(loadAdError.GetMessage());
    //        }

    //    }
    //광고 보여주는 함수
    public void ShowAds()
    {
        _adBtn.gameObject.SetActive(false);
        ADManager.Instance._rewardAD.ShowRewardAd();
    }

    ////보상 함수
    public void GetReward()
    {
        int gold = Score.Instance.GetReward();
        BackendGameData.Instance.UserGameData.gold += gold;

        gold *= 2;     // 2배 리워드
        _crystaltxt.text = gold.ToString();

        BackendGameData.Instance.GameDataUpdate();
        if (BackendStoreData.Instance.StoreGameData.eraseAdNotYet)
        {
            ADManager.Instance._rewardAD.LoadRewardAd();
        }
    }

    public IEnumerator BGFill(bool isFill)
    {
        _tutorialPanel.SetActive(false);
        _infinitePanel.SetActive(true);
        gameObject.SetActive(true);
        _resultSection.gameObject.SetActive(false);

        if (isFill)
        {
            _nowScore.text = Score.Instance.GetScore().ToString();

            switch (BackendGameData.Instance.UserGameData.selectInfiniteLevel)
            {
                case (int)eInfiniteLevel.NORMAL:
                    if (Score.Instance.GetScore() > BackendGameData.Instance.UserGameData.normalBestScore)
                    {
                        BackendGameData.Instance.UserGameData.normalBestScore = Score.Instance.GetScore();
                        _congraturation.SetActive(true);
                    }
                    _maxScore.text = BackendGameData.Instance.UserGameData.normalBestScore.ToString();
                    _crystaltxt.text = ((int)(Score.Instance.GetScore() * NORMAL_DEVIDE_NUM)).ToString();
                    break;

                case (int)eInfiniteLevel.FAST:
                    if (Score.Instance.GetScore() > BackendGameData.Instance.UserGameData.fastBestScore)
                    {
                        BackendGameData.Instance.UserGameData.fastBestScore = Score.Instance.GetScore();
                        _congraturation.SetActive(true);
                    }
                    _maxScore.text = BackendGameData.Instance.UserGameData.fastBestScore.ToString();
                    _crystaltxt.text = ((int)(Score.Instance.GetScore() * FAST_DEVIDE_NUM)).ToString();
                    break;

                case (int)eInfiniteLevel.HARD:
                    if (Score.Instance.GetScore() > BackendGameData.Instance.UserGameData.hardBestScore)
                    {
                        BackendGameData.Instance.UserGameData.hardBestScore = Score.Instance.GetScore();
                        _congraturation.SetActive(true);
                    }
                    _maxScore.text = BackendGameData.Instance.UserGameData.hardBestScore.ToString();
                    _crystaltxt.text = ((int)(Score.Instance.GetScore() * HARD_DEVIDE_NUM)).ToString();
                    break;
            }
            BackendGameData.Instance.GameDataUpdate();
        }

        if (isFill)
        {
            while (_backgroundImg.fillAmount < 1)
            {
                _backgroundImg.fillAmount += Time.deltaTime * 2;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (_backgroundImg.fillAmount > 0)
            {
                _backgroundImg.fillAmount -= Time.deltaTime * 2;
                yield return new WaitForEndOfFrame();
            }
        }
        _resultSection.SetActive(isFill);
        gameObject.SetActive(isFill);
        yield break;
    }

    public IEnumerator TutorialResult(bool complete)
    {
        gameObject.SetActive(true);
        _infinitePanel.SetActive(false);
        _tutorialPanel.SetActive(true);

        while (_hideOnBlack.fillAmount < 1)
        {
            _backgroundImg.fillAmount += Time.deltaTime * 2;
            yield return null;
        }

        _resultSection.SetActive(true);
        _success.SetActive(complete);
        _failure.SetActive(!complete);
        _score.SetActive(!complete);
        _scoretxt.text = Score.Instance.GetScore().ToString();
        _retryBtn.interactable = !complete;
    }

    public void HomeBtn()
    {
        if (InformManager._instance._tutorialComplete)
            SceneManager.LoadScene("Story");
        else
            SceneManager.LoadScene("Lobby");

    }
    public void ReplayBtn()
    {
        SceneManager.LoadScene("InGame");
    }

}
