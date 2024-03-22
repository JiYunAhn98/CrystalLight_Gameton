using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionaryPanel : ACPanel
{
    readonly int SIZE = 5;
    readonly int SIZE_SPEED = 10;

    [SerializeField] CharacterSystem _purchasePopup;
    [SerializeField] GameObject _informUI;
    RectTransform _rt;

    public override void Initialize()
    {
        _purchasePopup.Initilaize();
        _purchasePopup.gameObject.SetActive(false);

        _rt = GetComponent<RectTransform>();
        _rt.localScale = Vector3.one * SIZE;
        gameObject.SetActive(true);
        _beforePanelClose = true;
        _informUI.SetActive(false);
    }
    public override IEnumerator OpenMove()
    {
        float num;
        float totalLerp = 0;
        _informUI.SetActive(false);
        while (_rt.localScale.x > 1)
        {
            yield return null;
            num = SIZE_SPEED * Time.deltaTime;
            totalLerp += num;
            _rt.localScale -= Vector3.one * num;
        }
        _rt.localScale = Vector3.one;
        _purchasePopup.SectionActive(true);
    }
    public override IEnumerator CloseMove()
    {
        _purchasePopup.SectionActive(false);
        _informUI.SetActive(false);
        float num;
        while (_rt.localScale.x < 5)
        {
            yield return null;
            num = SIZE_SPEED * Time.deltaTime;
            _rt.localScale += Vector3.one * num;
        }
        _rt.localScale = Vector3.one * SIZE;
    }
    public override void PanelUpdate()
    {
        _purchasePopup.UpdateNowPick();
    }
}
