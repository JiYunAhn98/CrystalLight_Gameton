using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyirikennSolo : MonoBehaviour
{
    [SerializeField] Transform[] _syirikenn;

    void Update()
    {
        transform.Rotate(Vector3.back * -90 * Time.deltaTime);
        for (int i = 0; i < _syirikenn.Length; i++)
        {
            _syirikenn[i].Rotate(Vector3.forward * 180 * Time.deltaTime);
        }
    }
}
