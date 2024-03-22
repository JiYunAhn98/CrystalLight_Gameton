using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckTryInform : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _nowObstacle;
    [SerializeField] TextMeshProUGUI _nowWall;
    [SerializeField] TextMeshProUGUI _tunnelCnt;
    [SerializeField] TextMeshProUGUI _time;
    [SerializeField] TextMeshProUGUI _speed;

    public void SetTryInform(string obs, string wall, string tunnelCnt, string time, string speed)
    {
        _nowObstacle.text = obs;
        _nowWall.text = wall;
        _tunnelCnt.text = tunnelCnt;
        _time.text = time;
        _speed.text = speed;
    }
}
