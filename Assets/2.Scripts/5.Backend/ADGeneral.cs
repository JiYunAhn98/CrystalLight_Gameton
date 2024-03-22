using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using GoogleMobileAds;
//using GoogleMobileAds.Api;

public class ADGeneral : MonoBehaviour
{
    //string adUnitId;

    //private InterstitialAd interstitialAd;

    public void Start()
    {
//#if UNITY_ANDROID
//        adUnitId = "ca-app-pub-8865941325495641/3854960819";
//#else
//        adUnitId = "unexpected_platform";
//#endif

//        LoadInterstitialAd();
    }

    //public void OnEnable()
    //{
    //    if (BackendGameData.Instance.UserGameData.gameCount % 5 == 4 && BackendStoreData.Instance.StoreGameData.eraseAdNotYet)
    //    {
    //        ShowAd();
    //    }
    //}

    public void LoadInterstitialAd() //±¤°í ·Îµå
    {
        //if (interstitialAd != null)
        //{
        //    interstitialAd.Destroy();
        //    interstitialAd = null;
        //}

        //Debug.Log("Loading the interstitial ad.");

        //var adRequest = new AdRequest.Builder()
        //        .AddKeyword("unity-admob-sample")
        //        .Build();

        //InterstitialAd.Load(adUnitId, adRequest,
        //    (InterstitialAd ad, LoadAdError error) =>
        //    {
        //        if (error != null || ad == null)
        //        {
        //            Debug.LogError("interstitial ad failed to load an ad " +
        //                           "with error : " + error);
        //            return;
        //        }

        //        Debug.Log("Interstitial ad loaded with response : "
        //                  + ad.GetResponseInfo());

        //        interstitialAd = ad;
        //    });

    }

    public void ShowAd() //±¤°í º¸±â
    {
        //if (interstitialAd != null && interstitialAd.CanShowAd())
        //{
        //    Debug.Log("Showing interstitial ad.");
        //    interstitialAd.Show();
        //}
        //else
        //{
        //    LoadInterstitialAd(); //±¤°í Àç·Îµå

        //    Debug.LogError("Interstitial ad is not ready yet.");
        //}
    }
}
