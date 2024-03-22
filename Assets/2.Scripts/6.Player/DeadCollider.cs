using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class DeadCollider : MonoBehaviour
{
    [SerializeField] PlayerMove _player;

    private void OnTriggerEnter(Collider other)
    {
        // 면역관련 넣기
        if (other.CompareTag("Obstacle"))
        {
            _player._isObstacleTrigger = true;
            _player._bounceVector = (Vector2)(other.gameObject.transform.position - transform.position).normalized;
            _player._ObstacleState = eCharacterState.Dead;
        }
	}
}
