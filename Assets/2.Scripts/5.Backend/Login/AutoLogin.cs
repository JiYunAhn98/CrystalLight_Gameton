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
                Debug.Log("�ڵ� �α��ο� �����߽��ϴ�");

                BackendGameData.Instance.GameDataLoad();
                BackendStoreData.Instance.StoreDataLoad();
                BackendChartData.LoadAllChart();
                user.GetUserInfoFromBackend();
            }
            else
            {
                _informText.text = "�ڵ� �α����� �� �� �����ϴ�.";
                LoginOn();
            }
        });
    }
    public void LoginOn()
    {
        _loginPopup.SetActive(true);
    }
}
