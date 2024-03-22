using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using GoogleMobileAds;
//using GoogleMobileAds.Api;
using System;

using TMPro;

public class ADBanner : MonoSingleton<ADBanner>
{
    //string adUnitId;
    //float _time;
    //bool _cheatPoint;
    //BannerView _bannerView;

    //private void OnApplicationFocus(bool focus)
    //{
    //    if (focus)
    //    {
    //        if (Time.time - _time <= 5)
    //        {
    //            if (_cheatPoint)
    //            {
    //                Instantiate(Resources.Load("ErrorQuitCanvas"));
    //            }
    //            else
    //            {
    //                _cheatPoint = true;
    //            }
    //        }
    //        else
    //        {
    //            _cheatPoint = false;
    //        }
    //        if (!BackendStoreData._isLoadFinish || (BackendStoreData._isLoadFinish && BackendStoreData.Instance.StoreGameData.eraseAdNotYet))
    //        {
    //            GameObject go = Instantiate(Resources.Load("ErrorQuitCanvas")) as GameObject;
    //            go.GetComponentInChildren<TextMeshProUGUI>().text = _bannerView.IsDestroyed.ToString() + " / " + (_bannerView != null).ToString();
    //            LoadAd();
    //            go = Instantiate(Resources.Load("ErrorQuitCanvas")) as GameObject;
    //            go.GetComponentInChildren<TextMeshProUGUI>().text = _bannerView.IsDestroyed.ToString() + "/" + (_bannerView != null).ToString() + "/" + _bannerView.GetResponseInfo().GetResponseId();
    //            _bannerView.Show();
    //        }
    //        _time = Time.time;
    //    }
    //}

    public void Initialize()
    {
//#if UNITY_ANDROID
//        // ¿¸√º ±§∞Ì APP ID => ±§∞Ì∫∞∑Œ ID∞° ¥Ÿ∏£¥Ÿ.
//        adUnitId = "ca-app-pub-8865941325495641/8152711746";
//#else
//        adUnitId = "unexpected_platform";
//#endif
    }

    public void LoadAd() //±§∞Ì ∑ŒµÂ
    {
        //if (_bannerView == null)
        //{
        //    CreateBannerView();
        //}
        //var adRequest = new AdRequest.Builder()
        //    .AddKeyword("unity-admob-sample")
        //    .Build();

        //Debug.Log("Loading banner ad.");
        //_bannerView.LoadAd(adRequest);
    }

    public void CreateBannerView() //±§∞Ì ∫∏ø©¡÷±‚
    {
        //Debug.Log("Creating banner view");

        //if (_bannerView != null)
        //{
        //    DestroyAd();
        //}

        //_bannerView = new BannerView(adUnitId, new AdSize(320, 50), AdPosition.Top);
    }

    public void DestroyAd() //±§∞Ì ¡¶∞≈
    {
        //if (_bannerView != null)
        //{
        //    Debug.Log("Destroying banner ad.");
        //    _bannerView.Destroy();
        //    _bannerView = null;
        //}
    }
    public void HideAd() //±§∞Ì ¡¶∞≈
    {
        //if (_bannerView != null)
        //{
        //    Debug.Log("Hiding banner ad.");
        //    _bannerView.Hide();
        //}
    }
}
