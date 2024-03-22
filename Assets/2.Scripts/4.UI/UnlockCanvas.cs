using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DefineHelper;

public class UnlockCanvas : MonoBehaviour
{
    [SerializeField] CharacterUnlockUI _unlockChar;
    [SerializeField] UnLockOthersUI _unlockOthers;
    [SerializeField] Confetti[] _confetti;

    bool _isNext;

    public IEnumerator Initialize()
    {
        _isNext = false;

        _unlockOthers.gameObject.SetActive(false);
        _unlockChar.gameObject.SetActive(false);

        if (InformManager._instance._unlockChars.Count <= 0 && InformManager._instance._unlockThings.Count <= 0)
        {
            gameObject.SetActive(false);
            yield break;
        }
        else
        {
            SoundManager._instance.PlayEffect(eEffectSound.Unlock);
            gameObject.SetActive(true);

            _unlockChar.gameObject.SetActive(true);
            while (InformManager._instance._unlockChars.Count > 0)
            {
                _unlockChar.GetCharacter(InformManager._instance._unlockChars.Peek());
                InformManager._instance._unlockChars.Dequeue();
                while (!_isNext)
                {
                    ConfettiEffect();
                    yield return null;
                }
                _isNext = false;
            }
            _unlockChar.gameObject.SetActive(false);

            _unlockOthers.gameObject.SetActive(true);
            while (InformManager._instance._unlockThings.Count > 0)
            {
                _unlockOthers.UnlockOther(InformManager._instance._unlockThings.Peek());
                InformManager._instance._unlockThings.Dequeue();
                while (!_isNext)
                {
                    ConfettiEffect();
                    yield return null;
                }
                _isNext = false;
            }
            gameObject.SetActive(false);
            yield break;
        }
    }

    public void ConfettiEffect()
    {
        for(int i=0; i< _confetti.Length; i++)
        {
            _confetti[i].ConfettiEffect();
        }
    }

    public void OKBtn()
    {
        _isNext = true;
    }
}
