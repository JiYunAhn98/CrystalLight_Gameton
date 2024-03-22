using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Advertisements;
using TMPro;

public class ADManager : MonoBehaviour
{
    public AD _initializeAD;
    public InterstitialAD _interstitialAD;
    public BannerAD _bannerAD;
    public RewardAD _rewardAD;

    public bool _isFinish { get { return _initializeAD._isInitializeFinish; } }
    public bool _isBannerLoadFinish { get { return _bannerAD._isLoadFinish; } }
    public bool _isInterstitialLoadFinish { get { return _interstitialAD._isLoadFinish; } }
    public bool _isRewardLoadFinish { get { return _rewardAD._isLoadFinish; } }

    public static ADManager Instance { get; private set; }

    private void Awake()
    {
        _initializeAD.InitializeAds();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        StartCoroutine(LoadAll());
    }

    public IEnumerator LoadAll()
    {
        while (!_isFinish)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1);
        _bannerAD.LoadBanner();
        yield return new WaitForSeconds(1);
        _rewardAD.LoadRewardAd();
        yield return new WaitForSeconds(1);
        _interstitialAD.LoadInterstitialAd();
    }

    //private void OnGUI()
    //{
    //    GUIStyle style = new GUIStyle();
    //    style.fontSize = 100;
    //    style.normal.textColor = Color.white;

    //    GUI.Label(new Rect(0, 0, 1000, 50), _isFinish.ToString(), style);
    //    GUI.Label(new Rect(0, 60, 1000, 50), _bannerAD._mysetting, style);
    //    GUI.Label(new Rect(0, 120, 1000, 50), _rewardAD._mysetting, style);
    //    GUI.Label(new Rect(0, 180, 1000, 50), _interstitialAD._mysetting, style);
    //}
}