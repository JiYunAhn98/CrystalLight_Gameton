using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DefineHelper;

public class GachaWnd : ACPanel
{
    readonly int GOCHA_CRYSTAL = 1000;

    [SerializeField] RectTransform _myBtn;
    [SerializeField] Button _gochaBtn;
    [SerializeField] RectTransform _gachaEffect;
    [SerializeField] GameObject[] _dictionaryPanel;
    [SerializeField] TextMeshProUGUI _gachaTxt;
    [SerializeField] Image _gachaImg;
    [SerializeField] Toggle _isMotionSkip;

    //°¡Ã­ ºí¶ô
    [SerializeField] GameObject _gachaBlock;
    [SerializeField] Button _nextBtn;

    int _nowIndex;

    List<int> _faceNotHave;
    List<int> _accessaryNotHave;

    public override void Initialize()
    {
        gameObject.SetActive(true);
        _dictionaryPanel[0].SetActive(true);
        _dictionaryPanel[1].SetActive(false);
        _gachaBlock.SetActive(false);

        _faceNotHave = new List<int>();
        _accessaryNotHave = new List<int>();

        for (int i = 0; i < (int)eFace.Cnt; i++)
        {
            if (BackendGameData.Instance.UserGameData.faces[i] == false)
            {
                _faceNotHave.Add(i);
            }
        }
        for (int i = 0; i < (int)eAccessory.Cnt; i++)
        {
            if (BackendGameData.Instance.UserGameData.accessories[i] == false)
            {
                _accessaryNotHave.Add(i);
            }
        }

        _gochaBtn.interactable = CanFaceGacha();
    }
    public override IEnumerator OpenMove()
    {
        _myBtn.anchoredPosition += Vector2.down * 50;
        gameObject.SetActive(true);
        if (_nowIndex == 0)
            _gochaBtn.interactable = CanFaceGacha();
        else
            _gochaBtn.interactable = CanAccessoryGacha();

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
        _gachaEffect.Rotate(Vector3.forward * 100 * Time.deltaTime);
    }
    public void OtherGachaBtn(int index)
    {
        _dictionaryPanel[_nowIndex].SetActive(false);
        _dictionaryPanel[index].SetActive(true);
        _nowIndex = index;

        if (_nowIndex == 0)
            _gochaBtn.interactable = CanFaceGacha();
        else
            _gochaBtn.interactable = CanAccessoryGacha();
    }

    public void GachaGoBtn()
    {
        int got;
        if (_nowIndex == 1 && BackendGameData.Instance.UserGameData.accessoryTicket > 0)
        {
            BackendGameData.Instance.UserGameData.accessoryTicket--;
        }
        else if (_nowIndex == 0 && BackendGameData.Instance.UserGameData.faceTicket > 0)
        {
            BackendGameData.Instance.UserGameData.faceTicket--;
        }
        else
        {
            BackendGameData.Instance.UserGameData.gold -= GOCHA_CRYSTAL;
            BackendGameData.Instance.UserGameData.useCrystal += GOCHA_CRYSTAL;
        }

        switch (_nowIndex)
        {
            case 0:
                got = Random.Range(0, _faceNotHave.Count);
                BackendGameData.Instance.UserGameData.faces[_faceNotHave[got]] = true;
                Debug.Log(_faceNotHave[got]);
                if (_isMotionSkip.isOn)
                {
                    GochaSkip(_faceNotHave[got]);
                }
                else
                {
                    StartCoroutine(GochaMoment(_faceNotHave[got]));
                }
                _faceNotHave.RemoveAt(got);
                _gochaBtn.interactable = CanFaceGacha();
                break;
            case 1:
                got = Random.Range(0, _accessaryNotHave.Count);
                BackendGameData.Instance.UserGameData.accessories[_accessaryNotHave[got]] = true;
                if (_isMotionSkip.isOn)
                {
                    GochaSkip(_accessaryNotHave[got]);
                }
                else
                {
                    StartCoroutine(GochaMoment(_accessaryNotHave[got]));
                }
                _accessaryNotHave.RemoveAt(got);
                _gochaBtn.interactable = CanAccessoryGacha();
                break;
        }
        BackendGameData.Instance.DoingMission(eMissionType.GoGacha);
        BackendGameData.Instance.GameDataUpdate();
    }

    public IEnumerator GochaMoment(int index)
    {
        _gachaBlock.gameObject.SetActive(true);
        _nextBtn.gameObject.SetActive(false);
        float time = 0.02f;

        switch (_nowIndex)
        {
            case 0:
                while (time <= 0.5f)
                {
                    int rand = Random.Range(1, (int)eFace.Cnt);
                    _gachaImg.sprite = PrefabManager._instance.GetFaceThumbnail((eFace)rand);
                    _gachaTxt.text = GoogleSheetManager._instance.TakeString(GoogleSheetManager.eSheetIndex.FaceTable, rand + 1, GoogleSheetManager.eNameTableIndex.Name.ToString());
                    yield return new WaitForSeconds(time);
                    time += 0.03f;
                }
                _gachaImg.sprite = PrefabManager._instance.GetFaceThumbnail((eFace)index);
                _gachaTxt.text = GoogleSheetManager._instance.TakeString(GoogleSheetManager.eSheetIndex.FaceTable, index + 1, GoogleSheetManager.eNameTableIndex.Name.ToString());
                break;
            case 1:
                while (time <= 0.5f)
                {
                    int rand = Random.Range(1, (int)eAccessory.Cnt);
                    _gachaImg.sprite = PrefabManager._instance.GetAccessaryThumbnail((eAccessory)rand);
                    _gachaTxt.text = GoogleSheetManager._instance.TakeString(GoogleSheetManager.eSheetIndex.AccessoryTable, rand + 1, GoogleSheetManager.eNameTableIndex.Name.ToString());
                    yield return new WaitForSeconds(time);
                    time += 0.03f;
                }
                _gachaImg.sprite = PrefabManager._instance.GetAccessaryThumbnail((eAccessory)index);
                _gachaTxt.text = GoogleSheetManager._instance.TakeString(GoogleSheetManager.eSheetIndex.AccessoryTable, index + 1, GoogleSheetManager.eNameTableIndex.Name.ToString());
                break;
        }

        SoundManager._instance.PlayEffect(eEffectSound.Unlock);
        _nextBtn.gameObject.SetActive(true);
    }
    public void GochaSkip(int index)
    {
        _gachaBlock.gameObject.SetActive(true);
        _nextBtn.gameObject.SetActive(false);

        switch (_nowIndex)
        {
            case 0:
                _gachaImg.sprite = PrefabManager._instance.GetFaceThumbnail((eFace)index);
                _gachaTxt.text = GoogleSheetManager._instance.TakeString(GoogleSheetManager.eSheetIndex.FaceTable, index + 1, GoogleSheetManager.eNameTableIndex.Name.ToString());
                break;
            case 1:
                _gachaImg.sprite = PrefabManager._instance.GetAccessaryThumbnail((eAccessory)index);
                _gachaTxt.text = GoogleSheetManager._instance.TakeString(GoogleSheetManager.eSheetIndex.AccessoryTable, index + 1, GoogleSheetManager.eNameTableIndex.Name.ToString());
                break;
        }
        SoundManager._instance.PlayEffect(eEffectSound.Unlock);
        _nextBtn.gameObject.SetActive(true);
    }

    public void GochaEndBtn()
    {
        _nextBtn.gameObject.SetActive(false);
        _gachaBlock.gameObject.SetActive(false);
    }

    bool CanAccessoryGacha()
    {
        return _accessaryNotHave.Count > 0 && (BackendGameData.Instance.UserGameData.gold >= 1000 || BackendGameData.Instance.UserGameData.accessoryTicket > 0);
    }
    bool CanFaceGacha()
    {
        return _faceNotHave.Count > 0 && (BackendGameData.Instance.UserGameData.gold >= 1000 || BackendGameData.Instance.UserGameData.faceTicket > 0);
    }
}
