using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System.Collections.Generic;
using System.Collections;

public class RewardAD : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId = null; // This will remain null for unsupported platforms

    public bool _isLoadFinish { get; set; }
    public string _mysetting { get; set; }

    void Awake()
    {
        _isLoadFinish = false;
        // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
#elif UNITY_EDITOR
        _adUnitId = _androidAdUnitId; //Only for testing the functionality in the Editor
#endif
    }
    // Load content to the Ad Unit:
    public void LoadRewardAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        _mysetting = "Loading ";
        Advertisement.Load(_adUnitId, this);
    }

    // Show the loaded content in the Ad Unit:
    public void ShowRewardAd()
    {
        // Note that if the ad content wasn't previously loaded, this method will fail
        Debug.Log("Showing Ad: " + _adUnitId);
        _mysetting = "Showing";
        Advertisement.Show(_adUnitId, this);
    }

    #region [Callback]
    // Implement Load Listener and Show Listener interface methods: 
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Optionally execute code if the Ad Unit successfully loads content.
        _isLoadFinish = true;
        _mysetting = "Load Finish";
    }
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {_adUnitId} - {error.ToString()} - {message}");
        _mysetting = "Load Failure";
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
    }
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {_adUnitId}: {error.ToString()} - {message}");
        _mysetting = "Show Failure";
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
    }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        _mysetting = "Show Finish";
        if (_adUnitId == adUnitId && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Ads Fully Watched.....");
            if (SceneLoadManager._instance.GetActiveScene() == DefineHelper.eSceneName.InGame.ToString())
            {
                GameObject.FindGameObjectWithTag("RewardPopup").GetComponent<ResultPopup>().GetReward();
            }
            else if (SceneLoadManager._instance.GetActiveScene() == DefineHelper.eSceneName.Lobby.ToString())
            {
                StartCoroutine(GameObject.FindGameObjectWithTag("RewardPopup").GetComponent<QuestSlot>().GetRewardAD());
            }
            BackendGameData.Instance.GameDataUpdate();
        }
        LoadRewardAd();
    }
    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
    #endregion [Callback]
}