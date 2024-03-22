using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class BounceCollider : MonoBehaviour
{

    [SerializeField] PlayerMove _player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            BackendGameData.Instance.UserGameData.itemCount++;

            if (other.CompareTag("Magnet"))
            {
                int time = Constants.BASE_MAGNET_TIME + BackendGameData.Instance.UserGameData.itemLevel[(int)eItem.Magnet] * Constants.UPGRADE_MAGNET_TIME;
                other.gameObject.SetActive(false);
                StartCoroutine(_player.Magnet(time));
                StartCoroutine(EffectManager._instance.PlayEffectTimer(eParticle.Magnet, transform, time));
                SoundManager._instance.PlayEffect(eEffectSound.GetItem);

                _player._itemGetCount++;
            }
            else if (other.CompareTag("CrystalUp"))
            {
                other.gameObject.SetActive(false);
                Score.Instance.BonusScore(InGameManager._instance._itemScore);
                StartCoroutine(EffectManager._instance.PlayEffectOneShot(eParticle.Crystal, transform));
                SoundManager._instance.PlayEffect(eEffectSound.GetCrystal);
            }
            else if (other.CompareTag("SlowTime"))
            {
                float time = Constants.BASE_TIMECONTROL_TIME + BackendGameData.Instance.UserGameData.itemLevel[(int)eItem.SlowTime] * Constants.UPGRADE_TIMECONTROL_TIME;
                other.gameObject.SetActive(false);
                StartCoroutine(EffectManager._instance.PlayEffectOneShot(eParticle.SlowTime, transform));
                StartCoroutine(_player.SlowTime(time));
                SoundManager._instance.PlayEffect(eEffectSound.GetItem);

                _player._itemGetCount++;
            }
            else if (other.CompareTag("Shield"))
            {
                int time = Constants.BASE_SHIELD_TIME + BackendGameData.Instance.UserGameData.itemLevel[(int)eItem.Shield] * Constants.UPGRADE_SHIELD_TIME;
                other.gameObject.SetActive(false);
                StartCoroutine(_player.Shield(time));
                StartCoroutine(EffectManager._instance.PlayEffectTimer(eParticle.Shield, transform, time));
                SoundManager._instance.PlayEffect(eEffectSound.GetItem);

                _player._itemGetCount++;
            }
        }
        else
        {
            if (_player._ObstacleState == eCharacterState.Dead) return;
            else if (other.CompareTag("Obstacle"))
            {
                _player._isObstacleTrigger = true;
                _player._bounceVector = (Vector2)(other.gameObject.transform.position - transform.position).normalized;
                _player._ObstacleState = eCharacterState.Bounce;
            }
        }
    }
}
