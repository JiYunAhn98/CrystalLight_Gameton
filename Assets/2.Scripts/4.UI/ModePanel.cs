using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class ModePanel : ACPanel
{
    readonly int DOWN_SIZE = -800;
    readonly int UP_SIZE = 0;
    readonly int SIZE_SPEED = 2000;

    [SerializeField] GameObject[] _modeTypes;
    [SerializeField] GameObject[] _modeBtnLocks;
    [SerializeField] GameObject _touchSection;
    RectTransform _rt;
    int _nowIndex;

    public override void Initialize()
    {
        _touchSection.SetActive(false);
        _rt = GetComponent<RectTransform>();
        _rt.anchoredPosition = Vector2.up * DOWN_SIZE;
        _beforePanelClose = false;
        _modeTypes[0].SetActive(false);
        _modeTypes[1].SetActive(false);
        _modeTypes[BackendGameData.Instance.UserGameData.selectMode].SetActive(true);
        _nowIndex = BackendGameData.Instance.UserGameData.selectMode;
        PanelUpdate();
    }

    public override IEnumerator OpenMove()
    {
        _touchSection.SetActive(true);
        while (_rt.anchoredPosition.y < UP_SIZE)
        {
            yield return null;
            _rt.anchoredPosition += Vector2.up * SIZE_SPEED * Time.deltaTime;
        }
    }
    public override IEnumerator CloseMove()
    {
        _touchSection.SetActive(false);
        while (_rt.anchoredPosition.y > DOWN_SIZE)
        {
            yield return null;
            _rt.anchoredPosition += Vector2.down * SIZE_SPEED * Time.deltaTime;
        }
    }
    public void ClickOtherMode(int mode)
    {
        _modeTypes[_nowIndex].SetActive(false);
        _modeTypes[mode].SetActive(true);
        _nowIndex = mode;
        BackendGameData.Instance.UserGameData.selectMode = mode;
    }

    public void ClickMode(int mode)
    {
        Debug.Log(BackendGameData.Instance.UserGameData.selectMode + " = " + BackendGameData.Instance.UserGameData.selectInfiniteLevel);
        if (BackendGameData.Instance.UserGameData.selectMode == (int)eLevelMode.Infinite)
        {
            BackendGameData.Instance.UserGameData.selectInfiniteLevel = mode;
        }
        else
        {
            BackendGameData.Instance.UserGameData.selectTutorialLevel = mode;
        }

    }
    public override void PanelUpdate()
    {
        // _modeBtnLocks Àá±è ÇØ±Ý
        if (BackendGameData.Instance.UserGameData.infiniteLevel > 0)
        {
            _modeBtnLocks[0].SetActive(false);
        }
        if (BackendGameData.Instance.UserGameData.infiniteLevel > 1)
        {
            _modeBtnLocks[1].SetActive(false);
        }
        if (BackendGameData.Instance.UserGameData.tutorialLevel > 0)
        {
            _modeBtnLocks[2].SetActive(false);
        }
        if (BackendGameData.Instance.UserGameData.tutorialLevel > 1)
        {
            _modeBtnLocks[3].SetActive(false);
        }
    }
}
