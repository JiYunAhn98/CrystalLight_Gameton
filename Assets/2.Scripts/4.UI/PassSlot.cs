using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using DefineHelper;

public class PassSlot : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _level;
    [SerializeField] Transform _rewardTf;
    [SerializeField] Transform _stateTF; 

    int _index;
    string _type;
    ePassReward _reward;
    ePassState _state;

    public int _thisLevel { get { return _index; } }

    // 0은 Lock
    // 1은 CanGet
    // 2는 AlreadyGet
    public void Initialize(int index, int level)
    {
        _type = GoogleSheetManager._instance.TakeString(GoogleSheetManager.eSheetIndex.BattlePass, index, GoogleSheetManager.eBattlePassTableIndex.Type.ToString());
        _reward = (ePassReward)Enum.Parse(typeof(ePassReward), GoogleSheetManager._instance.TakeString(GoogleSheetManager.eSheetIndex.BattlePass, index, GoogleSheetManager.eBattlePassTableIndex.Reward.ToString()));
        _index = index;
        _level.text = index.ToString();

        for (int i = 0; i < _stateTF.childCount; i++)
        {
            _stateTF.GetChild(i).gameObject.SetActive(false);
        }

        _rewardTf.GetChild((int)_reward).gameObject.SetActive(true);

        if (_type == "OrePass")
        {
            if (!BackendStoreData.Instance.StoreGameData.battlepass) Lock(true);
            else if (index <= level)        // 받을 수 있는 상품이다.
            {
                if (BackendStoreData.Instance.StoreGameData.OrePassCnt < index) CanGet(true);
                else AlreadyGet(true);
            }
            else
            {
                CantGet(true);
            }
        }
        else if (index <= level)        // 받을 수 있는 상품이다.
        {
            if (BackendStoreData.Instance.StoreGameData.freePassCnt < index) CanGet(true);
            else AlreadyGet(true);
        }
        else
        {
            CantGet(true);
        }
    }

    public void GetReward(bool isBattlePass )
    {
        if (!isBattlePass && _type == "OrePass") return;

        switch (_reward)
        {
            case ePassReward.Crystal:
                BackendGameData.Instance.UserGameData.gold += GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.BattlePass, _index, GoogleSheetManager.eBattlePassTableIndex.Count.ToString());
                break;
            case ePassReward.Face:
                BackendGameData.Instance.UserGameData.faceTicket += GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.BattlePass, _index, GoogleSheetManager.eBattlePassTableIndex.Count.ToString());
                break;
            case ePassReward.Accessory:
                BackendGameData.Instance.UserGameData.accessoryTicket += GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.BattlePass, _index, GoogleSheetManager.eBattlePassTableIndex.Count.ToString());
                break;
        }

        for (int i = 0; i < _stateTF.childCount; i++)
        {
            _stateTF.GetChild(i).gameObject.SetActive(false);
        }
        AlreadyGet(true);
    }

    public void CantGet(bool isOn)
    {
        _stateTF.GetChild((int)ePassState.CantGet).gameObject.SetActive(isOn);
    }
    public void Lock(bool isOn)
    {
        _stateTF.GetChild((int)ePassState.Lock).gameObject.SetActive(isOn);
    }

    public void CanGet(bool isOn)
    {
        _stateTF.GetChild((int)ePassState.CanGet).gameObject.SetActive(isOn);
    }
    public void AlreadyGet(bool isOn)
    {
        _stateTF.GetChild((int)ePassState.AlreadyGet).gameObject.SetActive(isOn);
    }

}
