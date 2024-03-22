using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopButton : MonoBehaviour
{
    [SerializeField] GameObject _doubleGo;
    public void DoubleInitilaize(bool isOn)
    {
        _doubleGo.SetActive(isOn);
    }

    public void InteractableInitialize(bool isOn)
    {
        GetComponent<Button>().interactable = isOn;
        if(_doubleGo != null) _doubleGo.SetActive(isOn);
    }
}
