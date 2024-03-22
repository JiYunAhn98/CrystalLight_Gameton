using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public class WithDrawCanvas : MonoBehaviour
{
    public void OKBtn()
    {
        Backend.BMember.WithdrawAccount(callback => {
            Debug.Log("È¸¿øÅ»Åð ¼º°ø~");
            SceneLoadManager._instance.LoadScene("Loading");
        });
    }
    public void NoBtn()
    {
        gameObject.SetActive(false);
    }
}
