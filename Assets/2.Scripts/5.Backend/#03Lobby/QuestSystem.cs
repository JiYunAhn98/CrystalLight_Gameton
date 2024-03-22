using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using BackEnd;

public class QuestSystem : MonoBehaviour
{
    [SerializeField] QuestSlot[] _myslots;

    // 시간이 저장됨
    public void ServerTime()
    {
        Backend.Utils.GetServerTime((callback) =>
        {
            string time = callback.GetReturnValuetoJSON()["utcTime"].ToString();
            System.DateTime parsedDate = System.DateTime.Parse(time);
            int dayOfMonth = parsedDate.Day;
            int lastOf = BackendGameData.Instance.UserGameData.lastday;
            if (lastOf != dayOfMonth)
            {
                //퀘스트 생성함수 호출
                BackendGameData.Instance.MissionReset(dayOfMonth);
                Debug.Log("마지막 접속일 업데이트 : " + dayOfMonth);
            }
            else if (lastOf == dayOfMonth)
            {
                Debug.Log("마지막 접속일과 현재 접속일이 일치합니다");
            }
        });
    }


    // 게임을 켤 때마다 부를 내용
    private void OnEnable()
    {
        for (int i = 0; i < _myslots.Length; i++)
        {
            _myslots[i].Initialize(i);
        }
    }

}
