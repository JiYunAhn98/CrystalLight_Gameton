using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoGametonCanvas : MonoBehaviour
{
    public void OKBtn()
    {
        Application.OpenURL("https://lnk.bio/Ore_O_games");
    }
    public void NoBtn()
    {
        gameObject.SetActive(false);
    }

}
