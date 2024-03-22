using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class StoreManager : MonoBehaviour
{
    [SerializeField] TopPanelViewer _topviewer;
    [SerializeField] ACPanel[] _windows;
    [SerializeField] GameObject _outCanvas;

    int _nowIndex;

    private void Start()
    {
        SoundManager._instance.PlayBGM(eBGMSound.Store);
        _nowIndex = 0;
        for (int i = 0; i < _windows.Length; i++)
        {
            _windows[i].Initialize();
        }
        StartCoroutine(_windows[_nowIndex].OpenMove());
    }
    private void Update()
    {
        _windows[_nowIndex].PanelUpdate();
        _topviewer.UpdateGameData();
        if (Input.GetKey(KeyCode.Escape))
        {
            _outCanvas.SetActive(true);
        }
    }

    public void MoveOtherScene()
    {
        SceneLoadManager._instance.LoadScene(eSceneName.Lobby.ToString());
    }
    public void OpenOtherPanel(int index)
    {
        StartCoroutine(_windows[_nowIndex].CloseMove());
        StartCoroutine(_windows[index].OpenMove());
        _nowIndex = index;
    }
}
