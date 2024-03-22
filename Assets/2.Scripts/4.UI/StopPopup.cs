using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPopup : SettingWnd
{
    public void StopBtn()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    public void OutBtn()
    {
        Time.timeScale = 1;
        SceneLoadManager._instance.LoadScene(DefineHelper.eSceneName.Lobby);
    }
}
