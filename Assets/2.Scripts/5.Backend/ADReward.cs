using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using GoogleMobileAds.Api;
using TMPro;

public class ADReward : MonoBehaviour
{
    //보상형 광고 
    //private RewardedAd rewardedAd;
    public TMP_Text gold_txt;   // 늘어난 골드량을 표시할 txt 넣어주기

    public void Start()
    {
        InitAds();
    }

    //광고 초기화 함수
    public void InitAds()
    {
//        string adUnitId;
//        //현재는 테스트앱ID > 앱 등록 후 구글애드몹에서 ID 가져와야함 
//#if UNITY_ANDROID
//        adUnitId = "ca-app-pub-8865941325495641/3854960819";
//#else
//        adUnitId = "unexpected_platform";
//#endif

//        RewardedAd.Load(adUnitId, new AdRequest.Builder().Build(), LoadCallback);
    }

    ////로드 콜백 함수
    //public void LoadCallback(RewardedAd rewardedAd, LoadAdError loadAdError)
    //{
        //if (rewardedAd != null)
        //{
        //    this.rewardedAd = rewardedAd;
        //    Debug.Log("로드성공");
        //}
        //else
        //{
        //    Debug.Log(loadAdError.GetMessage());
        //}

    //}

    //광고 보여주는 함수
    public void ShowAds()
    {
        //if (rewardedAd.CanShowAd())
        //{
        //    rewardedAd.Show(GetReward);
        //}
        //else
        //{
        //    Debug.Log("광고 재생 실패");
        //}
    }

    //보상 함수
    //public void GetReward(Reward reward)
    //{
    //    BackendGameData.Instance.UserGameData.gold += (int)reward.Amount;     // 2배 리워드
    //    BackendGameData.Instance.DoingMission(DefineHelper.eMissionType.WatchAD);
    //    BackendGameData.Instance.GameDataUpdate();
    //    InitAds();
    //}
}
