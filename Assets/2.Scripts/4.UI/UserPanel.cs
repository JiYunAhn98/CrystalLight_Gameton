using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class UserPanel : MonoBehaviour
{
    public void WithDraw()
    {
        Backend.BMember.WithdrawAccount(callback => {
            Debug.Log("È¸¿øÅ»Åð ¼º°ø~");
            SceneLoadManager._instance.LoadScene("Loading");
        });
    }
    public void GoInformLink()
    {
        Application.OpenURL("https://storage.thebackend.io/f9f4b5b361b0a8ceb369941331bcae1cdf43e3211bf173d75293f70ae665cb34/privacy.html");
    }
}
