using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOutCanvas : MonoBehaviour
{
    public void OKBtn()
    {
        Application.Quit();
    }
    public void NoBtn()
    {
        gameObject.SetActive(false);
    }
}
