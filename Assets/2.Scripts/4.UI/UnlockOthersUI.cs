using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnLockOthersUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _txt;

    public void UnlockOther(DefineHelper.eLockThings other)
    {
        _txt.text = other.ToString();
    }
}
