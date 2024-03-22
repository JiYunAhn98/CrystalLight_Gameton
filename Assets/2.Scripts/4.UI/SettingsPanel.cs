using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPanel : ACPanel
{
    [SerializeField] GameObject[] _menuWnds;
    [SerializeField] GameObject _Block;
    RectTransform _rt;

    int _nowIndex;
    float SIZE;
    float SIZE_SPEED;

    public override void Initialize()
    {
        _rt = GetComponent<RectTransform>();

        CanvasScaler cs = _rt.parent.GetComponentInChildren<CanvasScaler>();

        float wRatio = cs.referenceResolution.x / Screen.width;
        float hRatio = cs.referenceResolution.y / Screen.height;
        float ratio = wRatio * (1f - cs.matchWidthOrHeight) + hRatio * (cs.matchWidthOrHeight);

        SIZE = Screen.width * ratio;
        SIZE_SPEED = SIZE * 2;
        _rt.anchoredPosition = Vector2.left * SIZE;

        for (int i = 0; i < _menuWnds.Length; i++)
        {
            _menuWnds[i].SetActive(false);
        }
        _menuWnds[0].GetComponent<SettingWnd>().Initialize();
        _menuWnds[1].GetComponent<RankLoader>().Initialize();

        _beforePanelClose = false;
        _nowIndex = 0;
    }
    public override IEnumerator OpenMove()
    {
        _Block.SetActive(true);
        _menuWnds[2].GetComponent<QuestSystem>().ServerTime();
        while (_rt.anchoredPosition.x <= 0)
        {
            yield return null;
            _rt.anchoredPosition += Vector2.right * SIZE_SPEED * Time.deltaTime;
        }
        _rt.anchoredPosition = Vector2.zero;
        _Block.SetActive(false);
    }
    public override IEnumerator CloseMove()
    {
        _Block.SetActive(true);
        while (_rt.anchoredPosition.x >= -SIZE)
        {
            yield return null;
            _rt.anchoredPosition += Vector2.left * SIZE_SPEED * Time.deltaTime;
        }
        _rt.anchoredPosition = Vector2.left * SIZE;
        _Block.SetActive(false);
    }
    public override void PanelUpdate()
    {
    }

    public void ActiveMenu(int index)
    {
        _menuWnds[_nowIndex].SetActive(false);
        _menuWnds[index].SetActive(true);
        _nowIndex = index;
    }
}
