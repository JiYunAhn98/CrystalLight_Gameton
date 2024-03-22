using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class EffectManager : DestroySingleton<EffectManager>
{
    Dictionary<eParticle, List<GameObject>> _activeEffects;
    FrostEffect _frost;

    int _frostLevel;
    bool _frostNow;

    Dictionary<eParticle, float> _startEffectTime;

    public bool _ableFrost { get { return !_frostNow; } }

    public void Initialize()
    {
        _activeEffects = new Dictionary<eParticle, List<GameObject>>();
        _startEffectTime = new Dictionary<eParticle, float>();
        _frost = Camera.main.GetComponentInChildren<FrostEffect>();
        _frostNow = false;
        _frostLevel = 0;
        _frost.FrostAmount = 0;

        _startEffectTime.Add(eParticle.Shield, 0);
        _startEffectTime.Add(eParticle.Super, 0);
        _startEffectTime.Add(eParticle.Magnet, 0);
    }
    public IEnumerator PlayEffectOneShotPos(eParticle particle, Vector3 pos)
    {
        int i;
        EffectGen(particle, out i);
        _activeEffects[particle][i].transform.position = pos;

        while (_activeEffects[particle][i].GetComponent<ParticleSystem>().isPlaying)
        {
            yield return new WaitForSeconds(1);
        }
        _activeEffects[particle][i].SetActive(false);
    }
    public IEnumerator PlayEffectOneShot(eParticle particle, Transform tf, int offset = 0)
    {
        int i;
        EffectGen(particle, out i);
        _activeEffects[particle][i].transform.SetParent(tf);
        _activeEffects[particle][i].transform.localPosition = Vector3.zero + Vector3.forward * offset;

        while (_activeEffects[particle][i].GetComponent<ParticleSystem>().isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }
        _activeEffects[particle][i].SetActive(false);
    }
    public IEnumerator PlayEffectTimer(eParticle particle, Transform tf, float time)
    {
        int i;

        if (_startEffectTime[particle] <= 0)
        {
            EffectGen(particle, out i);
            _activeEffects[particle][i].transform.SetParent(tf);
            _activeEffects[particle][i].transform.localPosition = Vector3.zero;
            _startEffectTime[particle] = Time.time + time;
        }
        else
        {
            _startEffectTime[particle] = Time.time + time;
            yield break;
        }

        while (_startEffectTime[particle] >= Time.time)
        {
            yield return new WaitForSeconds(0.1f);
        }
        _startEffectTime[particle] = 0;
        _activeEffects[particle][i].SetActive(false);
    }
    public void EffectDisActive(eParticle particle)
    {
        for (int i = 0; i < _activeEffects[particle].Count; i++)
        {
            _activeEffects[particle][i].SetActive(false);
        }
    }

    public IEnumerator Ice()
    {
        if (_frostNow) yield break;
        SoundManager._instance.PlayEffect(eEffectSound.Ice);
        _frostNow = true;
        _frostLevel++;

        switch (_frostLevel)
        {
            case 1:
                _frost.FrostAmount =  0.4f;
                break;
            case 2:
                _frost.FrostAmount = 0.5f;
                break;
            case 3:
                _frost.FrostAmount = 0.65f;
                break;
        }


        yield return new WaitForSeconds(0.5f);

        _frostNow = false;

        yield return new WaitForSeconds(2);

        while (_frost.FrostAmount > 0)
        {
            _frost.FrostAmount -= Time.deltaTime / 6;
            yield return new WaitForEndOfFrame();
        }
        _frostLevel = 0;
    }

    public void Vibrate()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (BackendGameData.Instance.UserGameData.isVibrateOn)
        {
            Debug.Log("Haptic");
        }
#elif UNITY_ANDROID || UNITY_IOS
        if (BackendGameData.Instance.UserGameData.isVibrateOn)
        {
            Handheld.Vibrate();
        }
#endif
    }

    void EffectGen(eParticle particle, out int i)
    {
        i = 0;

        if (_activeEffects.ContainsKey(particle))
        {
            for (i = 0; i < _activeEffects[particle].Count; i++)
            {
                if (!_activeEffects[particle][i].activeSelf) break;
            }

            if (i == _activeEffects[particle].Count)
            {
                _activeEffects[particle].Add(Instantiate(PrefabManager._instance.GetParticle(particle)));
            }
        }
        else
        {
            _activeEffects.Add(particle, new List<GameObject>());
            _activeEffects[particle].Add(Instantiate(PrefabManager._instance.GetParticle(particle)));
        }
        _activeEffects[particle][i].SetActive(true);
    }
}
