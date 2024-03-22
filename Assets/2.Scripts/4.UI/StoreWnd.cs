using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreWnd : ACPanel
{
    [Header("부활권 버튼")]
    [SerializeField] Button _revive1;
    [SerializeField] Button _revive1000;
    [SerializeField] Button _revive5000;

    [Header("슈퍼 패키지")]
    [SerializeField] Button _package4;
    [SerializeField] Button _package5;

    [Header("더블 크리스탈 혜택")]
    [SerializeField] GameObject _cry1000;
    [SerializeField] GameObject _cry1500;
    [SerializeField] GameObject _cry3000;
    [SerializeField] GameObject _cry4000;
    [SerializeField] GameObject _cry5000;

    [Header("더보기 버튼")]
    [SerializeField] Button _eraseAD;
    [SerializeField] Button _orePass;

    [SerializeField] RectTransform _myBtn;
    public override void Initialize()
    {
        gameObject.SetActive(false);
    }
    public override IEnumerator OpenMove()
    {
        _myBtn.anchoredPosition += Vector2.down * 50;
        gameObject.SetActive(true);
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
        _revive1.interactable = BackendGameData.Instance.UserGameData.gold >= 100;
        _revive1000.interactable = BackendGameData.Instance.UserGameData.gold >= 1000;
        _revive5000.interactable = BackendGameData.Instance.UserGameData.gold >= 5000;

        _package4.interactable = BackendStoreData.Instance.StoreGameData.package4BuyNum > 0;
        _package5.interactable = BackendStoreData.Instance.StoreGameData.package5BuyNum > 0;

        _cry1000.SetActive(BackendStoreData.Instance.StoreGameData.crystal_1000BuyNotYet);
        _cry1500.SetActive(BackendStoreData.Instance.StoreGameData.crystal_1500BuyNotYet);
        _cry3000.SetActive(BackendStoreData.Instance.StoreGameData.crystal_3000BuyNotYet);
        _cry4000.SetActive(BackendStoreData.Instance.StoreGameData.crystal_4000BuyNotYet);
        _cry5000.SetActive(BackendStoreData.Instance.StoreGameData.crystal_5000BuyNotYet);

        _eraseAD.interactable = BackendStoreData.Instance.StoreGameData.eraseAdNotYet;
        _orePass.interactable = !BackendStoreData.Instance.StoreGameData.battlepass;
    }

    public void BuyRevive(int num)
    {
        BackendGameData.Instance.UserGameData.heart += num;
        BackendGameData.Instance.UserGameData.gold -= num * 100;
        BackendGameData.Instance.UserGameData.useCrystal += num * 100;
        BackendGameData.Instance.GameDataUpdate();
    }
}

