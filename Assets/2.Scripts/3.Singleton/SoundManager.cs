using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class SoundManager : MonoSingleton<SoundManager>
{
    // resource 참조
    AudioClip[] _effectAudioSources;
    AudioClip[] _bgmAudioSources;

    // 참조 변수
    AudioSource _bgmSoundPlayer;
    AudioSource _effectSoundPlayer;
    public float _soundVolume { get { return _bgmSoundPlayer.volume; } }

    // 초기 설정
    public void Initialize(eBGMSound scene)
    {
        if (_effectSoundPlayer != null || _bgmSoundPlayer != null) return;

        _effectSoundPlayer = new GameObject("EffectSoundPlayer").AddComponent<AudioSource>();
        _effectSoundPlayer.transform.parent = gameObject.transform;
        _bgmSoundPlayer = new GameObject("BGMSoundPlayer").AddComponent<AudioSource>();
        _bgmSoundPlayer.transform.parent = gameObject.transform;

        _bgmSoundPlayer.volume = 0.1f;
        _effectSoundPlayer.volume = 0.4f;

        _effectAudioSources = new AudioClip[(int)eEffectSound.Cnt];
        for (int i = 0; i < (int)eEffectSound.Cnt; i++)
        {
            _effectAudioSources[i] = Resources.Load("Sound/Effect/" + ((eEffectSound)i).ToString()) as AudioClip;
        }
        _bgmAudioSources = new AudioClip[(int)eBGMSound.Cnt];
        for (int i = (int)eSceneName.Loading; i < (int)eBGMSound.Cnt; i++)
        {
            _bgmAudioSources[i - (int)eSceneName.Loading] = Resources.Load("Sound/BGM/" + ((eBGMSound)i).ToString()) as AudioClip;
        }

        _bgmSoundPlayer.clip = _bgmAudioSources[(int)scene];
        _bgmSoundPlayer.loop = true;

        _bgmSoundPlayer.Play();
    }

    // 소리를 Play
    public void PlayBGM(eBGMSound type, bool _isLoop = true)
    {
        _bgmSoundPlayer.clip = _bgmAudioSources[(int)type];
        _bgmSoundPlayer.loop = _isLoop;

        _bgmSoundPlayer.Play();
    }
    public void PlayEffect(eEffectSound type)
    {
        if (BackendGameData._isLoadFinish && !BackendGameData.Instance.UserGameData.isSFXOn) return;

        _effectSoundPlayer.PlayOneShot(_effectAudioSources[(int)type]);
    }
    public void BGMSoundSetting()
    {
        if (BackendGameData._isLoadFinish && BackendGameData.Instance.UserGameData.isBGMOn)
            _bgmSoundPlayer.volume = 0.1f;
        else
            _bgmSoundPlayer.volume = 0;
    }
}
