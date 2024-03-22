using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class ItemCollider : MonoBehaviour
{
    [SerializeField] PlayerMove _player;
    float _distance;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            _distance = 0;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            other.gameObject.transform.position = Vector3.Lerp(other.gameObject.transform.position, _player.transform.position, 0.1f + _distance);
            _distance += 3 * Time.deltaTime;
        }
    }
}
