using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingWnd : MonoBehaviour
{
    [SerializeField] Image _BGMOnImg;
    [SerializeField] Image _BGMOffImg;
    [SerializeField] Image _effectOnImg;
    [SerializeField] Image _effectOffImg;
    [SerializeField] Image _hapticOnImg;
    [SerializeField] Image _hapticOffImg;
    [SerializeField] GameObject _accountZone;
    [SerializeField] GameObject _witdrawCanvas;

    public void Initialize()
    {
        _hapticOnImg.gameObject.SetActive(BackendGameData.Instance.UserGameData.isVibrateOn);
        _hapticOffImg.gameObject.SetActive(!BackendGameData.Instance.UserGameData.isVibrateOn);

        _BGMOnImg.gameObject.SetActive(BackendGameData.Instance.UserGameData.isBGMOn);
        _BGMOffImg.gameObject.SetActive(!BackendGameData.Instance.UserGameData.isBGMOn);
        SoundManager._instance.BGMSoundSetting();

        _effectOnImg.gameObject.SetActive(BackendGameData.Instance.UserGameData.isSFXOn);
        _effectOffImg.gameObject.SetActive(!BackendGameData.Instance.UserGameData.isSFXOn);
    }
    public void HapticBtn()
    {
        BackendGameData.Instance.UserGameData.isVibrateOn = !BackendGameData.Instance.UserGameData.isVibrateOn;
        _hapticOnImg.gameObject.SetActive(BackendGameData.Instance.UserGameData.isVibrateOn);
        _hapticOffImg.gameObject.SetActive(!BackendGameData.Instance.UserGameData.isVibrateOn);
    }
    public void BGMSoundBtn()
    {
        BackendGameData.Instance.UserGameData.isBGMOn = !BackendGameData.Instance.UserGameData.isBGMOn;
        _BGMOnImg.gameObject.SetActive(BackendGameData.Instance.UserGameData.isBGMOn);
        _BGMOffImg.gameObject.SetActive(!BackendGameData.Instance.UserGameData.isBGMOn);
        SoundManager._instance.BGMSoundSetting();
    }
    public void EffectSoundBtn()
    {
        BackendGameData.Instance.UserGameData.isSFXOn = !BackendGameData.Instance.UserGameData.isSFXOn;
        _effectOnImg.gameObject.SetActive(BackendGameData.Instance.UserGameData.isSFXOn);
        _effectOffImg.gameObject.SetActive(!BackendGameData.Instance.UserGameData.isSFXOn);
    }
    public void OnAccountBtn()
    {
        _accountZone.SetActive(!_accountZone.activeSelf);
    }
    public void WithDraw()
    {
        _witdrawCanvas.SetActive(true);
    }
    public void GoInformLink()
    {
        Application.OpenURL("https://storage.thebackend.io/f9f4b5b361b0a8ceb369941331bcae1cdf43e3211bf173d75293f70ae665cb34/privacy.html");
    }
}
