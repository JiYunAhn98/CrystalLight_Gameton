using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DefineHelper;

public class DictionaryZone : MonoBehaviour
{
    eDictionaryCategory _category;  // 이 클래스가 해당하는 카테고리
    int _beforeIndex;
    int _nowPickindex;              // 현재 고른 index
    float _distance;                // 거리
    float[] _pos;                   // 거리에 따른 Position
    float _scrollPos;               // 현재 픽하고 있는 scroll값

    [SerializeField] GameObject scrollbar;
    [SerializeField] RectTransform _objects;
    [SerializeField] float _minSize;
    [SerializeField] float _maxSize;
    [SerializeField] HorizontalLayoutGroup _sideSizeController;
    [SerializeField] GameObject _slot;

    public eDictionaryCategory _myCategory { get { return _category; } }
    public int _myPick { get { return _nowPickindex; } }
    public int _myBeforePick { get { return _beforeIndex; } }
    public bool _isChange { get { return _nowPickindex != _beforeIndex; } }

    public void Initialize(int category)
    {
        RectTransform rt = GetComponent<RectTransform>();
        CanvasScaler cs = rt.parent.parent.parent.GetComponentInChildren<CanvasScaler>();

        float wRatio = cs.referenceResolution.x / Screen.width;
        float hRatio = cs.referenceResolution.y / Screen.height;
        float ratio = wRatio * (1f - cs.matchWidthOrHeight) + hRatio * (cs.matchWidthOrHeight);

        _sideSizeController.padding.left = (int)(Screen.width * ratio - 50);    // 현재 Content의 크기가 절반이라서
        _sideSizeController.padding.right = (int)(Screen.width * ratio - 50);   // 1의 크기로 간다

        _category = (eDictionaryCategory)category;
        GameObject go;
        switch (_category)
        {
            case eDictionaryCategory.Character:
                for (int i = 0; i < (int)eCharacter.Cnt; i++)
                {
                    go = Instantiate(_slot, _objects);
                    go.GetComponent<Image>().sprite = PrefabManager._instance.GetCharacterThumbnail((eCharacter)i);
                    go.transform.GetChild(0).gameObject.SetActive(BackendGameData.Instance.StateChoiceCharacter(i) > 0);
                }
                _nowPickindex = BackendGameData.Instance.UserGameData.selectBody;
                break;

            case eDictionaryCategory.Face:
                for (int i = 0; i < (int)eFace.Cnt; i++)
                {
                    go = Instantiate(_slot, _objects);
                    go.GetComponent<Image>().sprite = PrefabManager._instance.GetFaceThumbnail((eFace)i);
                    go.transform.GetChild(0).gameObject.SetActive(!BackendGameData.Instance.StateChoiceFace(i));
                }
                _nowPickindex = BackendGameData.Instance.UserGameData.selectFace;
                break;

            case eDictionaryCategory.Accessory:
                for (int i = 0; i < (int)eAccessory.Cnt; i++)
                {
                    go = Instantiate(_slot, _objects);
                    go.GetComponent<Image>().sprite = PrefabManager._instance.GetAccessaryThumbnail((eAccessory)i);
                    go.transform.GetChild(0).gameObject.SetActive(!BackendGameData.Instance.StateChoiceAccessory(i));
                }
                _nowPickindex = BackendGameData.Instance.UserGameData.selectAccessory;
                break;
        }
        _beforeIndex = _nowPickindex;

        _pos = new float[_objects.childCount];
        _distance = 1.0f / (_pos.Length - 1);
        for (int i = 0; i < _pos.Length; i++)
        {
            _pos[i] = _distance * i;
        }
        scrollbar.GetComponent<Scrollbar>().value = _pos[_nowPickindex];
        _scrollPos = scrollbar.GetComponent<Scrollbar>().value;
        gameObject.SetActive(false);
    }

    public void SettingNowPick()
    {
        switch (_category)
        {
            case eDictionaryCategory.Character:
                _nowPickindex = BackendGameData.Instance.UserGameData.selectBody;
                break;

            case eDictionaryCategory.Face:
                _nowPickindex = BackendGameData.Instance.UserGameData.selectFace;
                break;

            case eDictionaryCategory.Accessory:
                _nowPickindex = BackendGameData.Instance.UserGameData.selectAccessory;
                break;
        }
        _beforeIndex = _nowPickindex;

        scrollbar.GetComponent<Scrollbar>().value = _pos[_nowPickindex];
        _scrollPos = scrollbar.GetComponent<Scrollbar>().value;
        gameObject.SetActive(false);
    }

    public void UpdateState()
    {
        _beforeIndex = _nowPickindex;


        if (Input.GetMouseButton(0))
            _scrollPos = scrollbar.GetComponent<Scrollbar>().value;
        else
        {
            for (int i = 0; i < _pos.Length; i++)
            {
                if (_scrollPos < _pos[i] + (_distance / 2) && _scrollPos > _pos[i] - (_distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, _pos[i], 0.2f);
                }
            }
        }

        for (int i = 0; i < _pos.Length; i++)
        {
            if (_scrollPos < _pos[i] + (_distance / 2) && _scrollPos > _pos[i] - (_distance / 2))
            {
                _objects.GetChild(i).localScale = Vector2.Lerp(_objects.GetChild(i).localScale, Vector2.one * _maxSize, 0.2f);
                _nowPickindex = i;
                CheckEquip();
                for (int j = 0; j < _pos.Length; j++)
                {
                    if (j != i) _objects.GetChild(j).localScale = Vector2.Lerp(_objects.GetChild(j).localScale, Vector2.one * _minSize, 0.2f);
                }
            }
        }
    }

    public void CheckEquip()
    {
        switch (_category)
        {
            case eDictionaryCategory.Character:
                if (BackendGameData.Instance.StateChoiceCharacter(_nowPickindex) == 0)
                {
                    BackendGameData.Instance.UserGameData.selectBody = _nowPickindex;
                    transform.GetChild(0).GetChild(0).GetChild(0).GetChild(_nowPickindex).GetChild(0).gameObject.SetActive(BackendGameData.Instance.StateChoiceCharacter(_nowPickindex) > 0);
                }
                break;

            case eDictionaryCategory.Face:
                if (BackendGameData.Instance.StateChoiceFace(_nowPickindex))
                {
                    BackendGameData.Instance.UserGameData.selectFace = _nowPickindex;
                }
                break;

            case eDictionaryCategory.Accessory:
                if (BackendGameData.Instance.StateChoiceAccessory(_nowPickindex))
                {
                    BackendGameData.Instance.UserGameData.selectAccessory = _nowPickindex;
                }
                break;
        }
    }
}
