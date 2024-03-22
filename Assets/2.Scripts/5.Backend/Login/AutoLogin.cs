using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using TMPro;
public class AutoLogin : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _informText;
    [SerializeField] GameObject _loginPopup;
    [SerializeField] UserInfo user;

    public void TryAutoLogin()
    {
        Backend.BMember.LoginWithTheBackendToken((callback) =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("자동 로그인에 성공했습니다");

                BackendGameData.Instance.GameDataLoad();
                BackendStoreData.Instance.StoreDataLoad();
                BackendChartData.LoadAllChart();
                user.GetUserInfoFromBackend();
            }
            else
            {
                _informText.text = "자동 로그인을 할 수 없습니다.";
                LoginOn();
            }
        });
    }
    public void LoginOn()
    {
        _loginPopup.SetActive(true);
    }
}
