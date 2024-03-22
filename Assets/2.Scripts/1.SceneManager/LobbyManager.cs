using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
public class LobbyManager : MonoBehaviour
{
    [SerializeField] ACPanel[] _panels;

    [SerializeField] Transform _backGroundLight;
    [SerializeField] TopPanelViewer _toppanel;
    [SerializeField] UserInfo user;
    [SerializeField] CharacterSystem characterSystem;
    [SerializeField] QuestSystem questSystem;
    [SerializeField] UnlockCanvas unlockSystem;
    [SerializeField] GameObject _block;
    [SerializeField] GameObject _outCanvas;
    [SerializeField] GameObject _nickNamePopup;
    [SerializeField] GameObject _gamspassCanvas;

    Stack<int> _activePanel;

    private void Awake()
    {
        _block.SetActive(false);
        if (BackendStoreData.Instance.StoreGameData.eraseAdNotYet)
        {
            ADManager.Instance._bannerAD.ShowBannerAd();
        }
    }
    private void Start()
    {
        SoundManager._instance.PlayBGM(eBGMSound.Lobby);
        _activePanel = new Stack<int>();
        if (BackendGameData.Instance.UserGameData.ruby > 1 && BackendGameData.Instance.UnlockCharacterNumber() >= 4)
        {
            InformManager._instance.EnqueueCharacter(eCharacter.Ruby);
        }
        if (BackendGameData.Instance.UserGameData.realgold > 1 && BackendGameData.Instance.UserGameData.useCrystal >= 10000)
        {
            InformManager._instance.EnqueueCharacter(eCharacter.RealGold);
        }

        _activePanel.Push(0);
        for (int i = 0; i < _panels.Length; i++)
        {
            _panels[i].Initialize();
        }
        _toppanel.UpdateGameData();
        _outCanvas.SetActive(false);

        _nickNamePopup.SetActive(UserInfo.Data.nickname == null);
        _gamspassCanvas.GetComponent<GamePassCanvas>().Initialize();
        if (UserInfo.Data.nickname != null)
        {
            _gamspassCanvas.SetActive(InformManager._instance._firstEnter);
            InformManager._instance._firstEnter = false;
        }
    }
    private void Update()
    {
        _backGroundLight.Rotate(Vector3.forward * 50 * Time.deltaTime);
        if(_activePanel.Count > 0) _panels[_activePanel.Peek()].PanelUpdate();

        if (InformManager._instance._unlockChars.Count > 0 || InformManager._instance._unlockThings.Count > 0)
        {
            StartCoroutine(unlockSystem.Initialize());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _outCanvas.SetActive(true);
        }
    }

    public void OpenOtherPanel(int index)
    {
        BackendGameData.Instance.GameDataUpdate();
        if (_panels[index]._beforePanelClose)
        {
            while (_activePanel.Count > 0)
            {
                StartCoroutine(_panels[_activePanel.Peek()].GetComponent<ACPanel>().CloseMove());
                _activePanel.Pop();
            }
        }
        StartCoroutine(_panels[index].GetComponent<ACPanel>().OpenMove());
        _activePanel.Push(index);
    }
    public void CloseMyPanel()
    {
        StopAllCoroutines();
        StartCoroutine(_panels[_activePanel.Peek()].GetComponent<ACPanel>().CloseMove());
        _activePanel.Pop();
        BackendGameData.Instance.GameDataUpdate();
    }
    public void MoveOtherScene()
    {
        SceneLoadManager._instance.LoadScene(eSceneName.Store.ToString());
    }
}
