using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DefineHelper;
using TMPro;

public class GamePassCanvas : MonoBehaviour
{
    [SerializeField] Transform _scrollViewTF;
    [SerializeField] GameObject _slot;
    [SerializeField] TextMeshProUGUI _passLevel;
    [SerializeField] Button _shopBtn;
    [SerializeField] TopPanelViewer _topview;
    [SerializeField] GameObject _completeLock;
    [SerializeField] Slider _experienceState;
    PassSlot[] _passslots;
    int _level;
    bool _isReward;

    public void Initialize()
    {
        _passslots = new PassSlot[39];

        for (int i = 0; i < _passslots.Length; i++)
        {
            _passslots[i] = Instantiate(_slot, _scrollViewTF).GetComponent<PassSlot>();
        }
        SettingStates();
    }

    private void OnEnable()
    {
        _isReward = false;
        SettingStates();

    }

    public void SettingStates()
    {
        _level = (int)(BackendGameData.Instance.UserGameData.experience / 850);
        if (_level >= 40)
        {
            _experienceState.value = 850;
            _level = 40;
            _experienceState.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "MAX";
        }
        else
        {
            _experienceState.value = BackendGameData.Instance.UserGameData.experience % 850;
            _experienceState.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = string.Format("{0} / 850", _experienceState.value);
        }

        _passLevel.text = _level.ToString();
        _shopBtn.interactable = !BackendStoreData.Instance.StoreGameData.battlepass;
        _completeLock.SetActive(!BackendStoreData.Instance.StoreGameData.battlepass);

        for (int i = 0; i < _passslots.Length; i++)
        {
            _passslots[i].Initialize(i + 1, _level);
        }
    }

    public void GetRewards()
    {
        if (_isReward) return;

        if (BackendStoreData.Instance.StoreGameData.battlepass)
        {
            for (int i = BackendStoreData.Instance.StoreGameData.OrePassCnt; i < _level; i++)
            {
                Debug.Log(i);
                if (i < _passslots.Length)
                    _passslots[i].GetReward(true);
                else
                    InformManager._instance.EnqueueCharacter(eCharacter.Diamond);
            }
            BackendStoreData.Instance.StoreGameData.OrePassCnt = _level;
        }

        for (int i = BackendStoreData.Instance.StoreGameData.freePassCnt; i < _level; i++)
        {
            if (i < _passslots.Length)
                _passslots[i].GetReward(false);
        }
        BackendStoreData.Instance.StoreGameData.freePassCnt = _level;

        if (BackendStoreData.Instance.StoreGameData.battlepass && _level >= 40)
        {
            BackendGameData.Instance.UserGameData.diamond = 1;
        }

        _topview.UpdateGameData();

        BackendGameData.Instance.GameDataUpdate();
        BackendStoreData.Instance.StoreDataUpdate();

        _isReward = true;
    }

    public void GoToShop()
    {
        SceneLoadManager._instance.LoadScene(eSceneName.Store);
    }
}
