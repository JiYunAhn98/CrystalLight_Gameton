using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeSlot : MonoBehaviour
{
    [SerializeField] Transform level;
    [SerializeField] Outline _selectOutline;
    [SerializeField] TextMeshProUGUI _myText;
    int _myIndex;

    public void Initialize(int index)
    {
        _myIndex = index;
        _selectOutline.enabled = false;
        UpdateUpgradeState();
    }

    public int ClickMe(int baseCrystal)
    {
        _selectOutline.enabled = true;

        return baseCrystal + BackendGameData.Instance.UserGameData.itemLevel[_myIndex] * 500;
    }

    public void OutOfMe()
    {
        _selectOutline.enabled = false;
    }

    public void UpdateUpgradeState()
    {
        for (int i = 0; i < level.childCount - 1; i++)
        {
            if (i < BackendGameData.Instance.UserGameData.itemLevel[_myIndex])
            {
                level.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                level.GetChild(i).gameObject.SetActive(false);
            }
        }
        level.GetChild(level.childCount - 1).GetComponent<TextMeshProUGUI>().text = string.Format("{0}´Ü°è", BackendGameData.Instance.UserGameData.itemLevel[_myIndex]);

        switch (_myIndex)
        {
            case 0:
                _myText.text = (Constants.BASE_CRYSTAL_SCORE + (float)BackendGameData.Instance.UserGameData.itemLevel[_myIndex] * Constants.UPGRADE_CRYSTAL_SCORE).ToString().Split('0')[0];
                break;
            case 1:
                _myText.text = (Constants.BASE_SHIELD_TIME + BackendGameData.Instance.UserGameData.itemLevel[_myIndex] * Constants.UPGRADE_SHIELD_TIME).ToString();
                break;
            case 2:
                _myText.text = (Constants.BASE_MAGNET_TIME + BackendGameData.Instance.UserGameData.itemLevel[_myIndex] * Constants.UPGRADE_MAGNET_TIME).ToString();
                break;
            case 3:
                _myText.text = (Constants.BASE_TIMECONTROL_TIME + (float)BackendGameData.Instance.UserGameData.itemLevel[_myIndex] * Constants.UPGRADE_TIMECONTROL_TIME).ToString().Split('0')[0];
                break;
        }
    }
}
