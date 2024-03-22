using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class BannerAD : MonoBehaviour
{
    // For the purpose of this example, these buttons are for functionality testing:
    [SerializeField] BannerPosition _bannerPosition = BannerPosition.TOP_CENTER;

    [SerializeField] string _androidAdUnitId = "Banner_Android";
    [SerializeField] string _iOSAdUnitId = "Banner_iOS";

    string _adUnitId = null; // This will remain null for unsupported platforms.
    public bool _isLoadFinish { get; set; }
    public string _mysetting { get; set; }
    void Start()
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
        // Set the banner position:
        Advertisement.Banner.SetPosition(_bannerPosition);
    }

    // 로드 배너 버튼 클릭시 호출
    public void LoadBanner()
    {
        _mysetting = "loading";
        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,    // 버튼에 리스너를 넣는다. 필요 없음
            errorCallback = OnBannerError
        };

        // Load the Ad Unit with banner content:
        Advertisement.Banner.Load(_adUnitId, options);
    }

    // Implement a method to call when the Show Banner button is clicked:
    public void ShowBannerAd()
    {
        // Set up options to notify the SDK of show events:
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        // Show the loaded Banner Ad Unit:
        Advertisement.Banner.Show(_adUnitId, options);
    }

    // 광고 숨김 => 광고 제거 사면
    public void HideBannerAd()
    {
        // Hide the banner:
        Advertisement.Banner.Hide();
    }

    #region[Callback]
    // Implement code to execute when the loadCallback event triggers:
    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");
        _mysetting = "Load Finish";

        _isLoadFinish = true;
    }
    // Implement code to execute when the load errorCallback event triggers:
    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        _mysetting = "Error : + " + message;
        // Optionally execute additional code, such as attempting to load another ad.
        LoadBanner();

    }
    void OnBannerClicked()
    {
        _mysetting = "Clicked";
    }
    void OnBannerShown()
    {
        _mysetting = "Show Finish";
    }
    void OnBannerHidden()
    {
        _mysetting = "Hidden";
    }
    #endregion[Callback]
}