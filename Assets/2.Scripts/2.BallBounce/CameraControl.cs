using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //[Header("Mode (1 = �ٷ� ��, 2 = �߾�ȸ��, 3 = �ݴ������� ȸ��, 4 = ���� ������ ������)")]
    //[SerializeField] [Range(1,4)] int mode = 1;
    //[Header("ī�޶� ��ü�� ������� �ӵ�")]
    //[SerializeField] float _time = 5;
    //[Header("ī�޶�� ��ü ������ �Ÿ�")]
    //[SerializeField] float _offsetY = 1;
    //[SerializeField] float _offsetZ = 1;
    //[Header("���� ���� ����")]
    //[SerializeField] [Range(1, 10)] float _mulVal = 1;

    //Transform _player;

    //void Start()
    //{
    //    _player = GameObject.FindGameObjectWithTag("Player").transform;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    if (_player != null)
    //    {
    //        switch (mode)
    //        {
    //            case 1:
    //                transform.position = Vector3.Lerp(transform.position, _player.position + Vector3.up * _offsetY + Vector3.back * _offsetZ, 5 * Time.deltaTime);
    //                break;
    //            case 2:
    //                transform.position = Vector3.up * _offsetY + Vector3.back * _offsetZ;
    //                break;
    //            case 3:
    //                transform.position = Vector3.up * _offsetY + _player.position * -1 + Vector3.back * _offsetZ;
    //                break;
    //            case 4:
    //                transform.position = _player.position / _mulVal + Vector3.up * _offsetY + Vector3.back * _offsetZ;
    //                break;
    //        }
    //        transform.LookAt(_player.position);
    //    }
    //}
}
