using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using GoogleMobileAds.Api;
using TMPro;

public class ADReward : MonoBehaviour
{
    //������ ���� 
    //private RewardedAd rewardedAd;
    public TMP_Text gold_txt;   // �þ ��差�� ǥ���� txt �־��ֱ�

    public void Start()
    {
        InitAds();
    }

    //���� �ʱ�ȭ �Լ�
    public void InitAds()
    {
//        string adUnitId;
//        //����� �׽�Ʈ��ID > �� ��� �� ���۾ֵ������ ID �����;��� 
//#if UNITY_ANDROID
//        adUnitId = "ca-app-pub-8865941325495641/3854960819";
//#else
//        adUnitId = "unexpected_platform";
//#endif

//        RewardedAd.Load(adUnitId, new AdRequest.Builder().Build(), LoadCallback);
    }

    ////�ε� �ݹ� �Լ�
    //public void LoadCallback(RewardedAd rewardedAd, LoadAdError loadAdError)
    //{
        //if (rewardedAd != null)
        //{
        //    this.rewardedAd = rewardedAd;
        //    Debug.Log("�ε强��");
        //}
        //else
        //{
        //    Debug.Log(loadAdError.GetMessage());
        //}

    //}

    //���� �����ִ� �Լ�
    public void ShowAds()
    {
        //if (rewardedAd.CanShowAd())
        //{
        //    rewardedAd.Show(GetReward);
        //}
        //else
        //{
        //    Debug.Log("���� ��� ����");
        //}
    }

    //���� �Լ�
    //public void GetReward(Reward reward)
    //{
    //    BackendGameData.Instance.UserGameData.gold += (int)reward.Amount;     // 2�� ������
    //    BackendGameData.Instance.DoingMission(DefineHelper.eMissionType.WatchAD);
    //    BackendGameData.Instance.GameDataUpdate();
    //    InitAds();
    //}
}
