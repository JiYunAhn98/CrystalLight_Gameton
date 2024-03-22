using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdradeWnd : ACPanel
{
    readonly int BASE_UPGRADE_CRYSTAL = 1000;
    readonly int UPGRADE_MAX_LEVEL = 5;
    enum item { crystal, shield, magnet, timeControl }

    [SerializeField] RectTransform _myBtn;
    [SerializeField] UpgradeSlot[] _selectAbility;
    [SerializeField] TextMeshProUGUI _crystalTxt;
    [SerializeField] Button _upgradeBtn;
    int _nowIndex;
    int _nowCrystalValue;

    public override void Initialize()
    {
        _nowIndex = _selectAbility.Length;
        for (int i = 0; i < _nowIndex; i++)
        {
            _selectAbility[i].Initialize(i);
        }
        gameObject.SetActive(false);
    }
    public override IEnumerator OpenMove()
    {
        _myBtn.anchoredPosition += Vector2.down * 50;
        gameObject.SetActive(true);
        _upgradeBtn.interactable = false;
        yield break;
    }
    public override IEnumerator CloseMove()
    {
        _myBtn.anchoredPosition += Vector2.up * 50;
        gameObject.SetActive(false);
        yield break;
    }
    public override void PanelUpdate()
    {
    }
    public void PickAbility(int index)
    {
        if(_nowIndex < _selectAbility.Length && index != _nowIndex) _selectAbility[_nowIndex].OutOfMe();
        _nowCrystalValue = _selectAbility[index].ClickMe(BASE_UPGRADE_CRYSTAL);
        _nowIndex = index;
        _upgradeBtn.interactable = (_nowCrystalValue <= BackendGameData.Instance.UserGameData.gold && BackendGameData.Instance.UserGameData.itemLevel[_nowIndex] < UPGRADE_MAX_LEVEL);
        //_crystalTxt 정하기
        if (BackendGameData.Instance.UserGameData.itemLevel[_nowIndex] < UPGRADE_MAX_LEVEL)
            _crystalTxt.text = _nowCrystalValue.ToString();
        else
            _crystalTxt.text = "MAX";
    }
    public void UpgradeBtn()
    {
        //백엔드에 저장되어있는 아이템의 유효시간 늘려주기
        //돈 계산되도록 하기
        BackendGameData.Instance.UserGameData.gold -= _nowCrystalValue;
        BackendGameData.Instance.UserGameData.useCrystal += _nowCrystalValue;
        BackendGameData.Instance.UserGameData.itemLevel[_nowIndex]++;
        BackendGameData.Instance.GameDataUpdate();
        _selectAbility[_nowIndex].UpdateUpgradeState();
        PickAbility(_nowIndex);
    }
}

