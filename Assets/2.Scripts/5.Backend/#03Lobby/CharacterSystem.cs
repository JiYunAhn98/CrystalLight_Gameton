using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DefineHelper;
using TMPro;
using BackEnd;

public class CharacterSystem : MonoBehaviour
{
    [Header("시스템 사용")] 
    [SerializeField] TopPanelViewer _topViewer;         // 재화 상태 표시창
    [SerializeField] GameObject _purchaseWindow;        // 구매 팝업
    [SerializeField] GameObject _cantPurchaseWindow;    // 구매 불가능 팝업
    [SerializeField] GameObject _informUI;

    [Header("화면 표시")]
    [SerializeField] DictionaryZone[] _category;        // 나열할 오브젝트
    [SerializeField] TextMeshProUGUI _nameText;         // 고른 이름
    [SerializeField] TextMeshProUGUI _informationText;  // 내용
    [SerializeField] TextMeshProUGUI _crystalValue;     // 가격

    [Header("구매 가능 표시")]
    [SerializeField] GameObject _notYet;                // 해금 버튼
    [SerializeField] Text _notYetTxt;                   // 해금 설명
    [SerializeField] Button _buyButton;                 // 구매 버튼

    [Header("표현 캐릭터")]
    [SerializeField] MeshRenderer _nowPickCharacter;    // 몸통
    [SerializeField] MeshRenderer _nowPickFace;       // 얼굴
    [SerializeField] Transform _nowPickAccessory;       //악세

    int _categoryIndex;

    public void Initilaize()
    {
        _categoryIndex = 0;
        _informationText.gameObject.SetActive(false);
        _nameText.gameObject.SetActive(false);
        _notYet.SetActive(false);
        _notYetTxt.gameObject.SetActive(false);
        _buyButton.gameObject.SetActive(false);
        _purchaseWindow.SetActive(false);
        _cantPurchaseWindow.SetActive(false);

        for (int i = 0; i < _category.Length; i++)
        {
            _category[i].Initialize(i);
        }
        gameObject.SetActive(false);
    }

    public void CategoryBtn(int category)
    {
        _informUI.SetActive(true);
        _category[_categoryIndex].gameObject.SetActive(false);
        _category[category].gameObject.SetActive(true);
        _categoryIndex = category;

        Process();
    }

    public void SectionActive(bool isOn)
    {
        gameObject.SetActive(isOn);

        if (!isOn)
        {
            for (int i = 0; i < _category.Length; i++)
            {
                _category[i].gameObject.SetActive(false);
            }
            _nowPickAccessory.GetChild(_category[2]._myPick).gameObject.SetActive(false);
        }
        else
        {
            for (int i = 0; i < _category.Length; i++)
            {
                _category[i].SettingNowPick();
            }
        }
    }

    public void UpdateNowPick()
    {
        if (_category[_categoryIndex].gameObject.activeSelf) _category[_categoryIndex].UpdateState();
        if (_category[_categoryIndex]._isChange) Process();
    }

    /// <summary>
    /// 현재 캐릭터 상태에 따른 구매 버튼 활성화 
    /// </summary>
    public void Process()
    {
        switch (_categoryIndex)
        {
            case (int)eDictionaryCategory.Character:
                _nameText.text = GoogleSheetManager._instance.TakeString(GoogleSheetManager.eSheetIndex.CharacterTable, _category[_categoryIndex]._myPick + 1, GoogleSheetManager.eCharacterTableIndex.NickName.ToString());
                _informationText.text = GoogleSheetManager._instance.TakeString(GoogleSheetManager.eSheetIndex.CharacterTable, _category[_categoryIndex]._myPick + 1, GoogleSheetManager.eCharacterTableIndex.Ability.ToString());
                

                _informationText.gameObject.SetActive(true);
                _nameText.gameObject.SetActive(true);
                _nowPickCharacter.material = PrefabManager._instance.GetCharacterMaterial((eCharacter)_category[_categoryIndex]._myPick);

                switch (BackendGameData.Instance.StateChoiceCharacter(_category[_categoryIndex]._myPick))
                {
                    case 0:
                        _buyButton.gameObject.SetActive(false);
                        _notYetTxt.gameObject.SetActive(false);
                        _notYet.gameObject.SetActive(false);
                        break;
                    case 1:
                        _crystalValue.text = GoogleSheetManager._instance.TakeString(GoogleSheetManager.eSheetIndex.CharacterTable, _category[_categoryIndex]._myPick + 1, GoogleSheetManager.eCharacterTableIndex.Cost.ToString());
                        _buyButton.gameObject.SetActive(true);
                        _notYetTxt.gameObject.SetActive(false);
                        _notYet.gameObject.SetActive(false);
                        break;
                    case 2:
                        _notYetTxt.text = GoogleSheetManager._instance.TakeString(GoogleSheetManager.eSheetIndex.CharacterTable, _category[_categoryIndex]._myPick + 1, GoogleSheetManager.eCharacterTableIndex.Unlock.ToString());
                        _buyButton.gameObject.SetActive(false);
                        _notYetTxt.gameObject.SetActive(true);
                        _notYet.gameObject.SetActive(true);
                        break;
                }
                break;

            case (int)eDictionaryCategory.Face:
                _nameText.gameObject.SetActive(true);
                _informationText.gameObject.SetActive(false);
                _buyButton.gameObject.SetActive(false);
                _notYetTxt.gameObject.SetActive(false);
                _nameText.text = GoogleSheetManager._instance.TakeString(GoogleSheetManager.eSheetIndex.FaceTable, _category[_categoryIndex]._myPick + 1, GoogleSheetManager.eNameTableIndex.Name.ToString());
                _notYet.gameObject.SetActive(!BackendGameData.Instance.StateChoiceFace(_category[_categoryIndex]._myPick));
                _nowPickFace.material = PrefabManager._instance.GetFaceMaterial((eFace)_category[_categoryIndex]._myPick);
                break;

            case (int)eDictionaryCategory.Accessory:
                _nameText.gameObject.SetActive(true);
                _informationText.gameObject.SetActive(false);
                _buyButton.gameObject.SetActive(false);
                _notYetTxt.gameObject.SetActive(false);
                _nameText.text = GoogleSheetManager._instance.TakeString(GoogleSheetManager.eSheetIndex.AccessoryTable, _category[_categoryIndex]._myPick + 1, GoogleSheetManager.eNameTableIndex.Name.ToString());
                _notYet.gameObject.SetActive(!BackendGameData.Instance.StateChoiceAccessory(_category[_categoryIndex]._myPick));
                _nowPickAccessory.GetChild(_category[_categoryIndex]._myBeforePick).gameObject.SetActive(false);
                _nowPickAccessory.GetChild(_category[_categoryIndex]._myPick).gameObject.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// 캐릭터 구매
    /// </summary>
    public void PurchaseLevel()
    {
        _purchaseWindow.SetActive(false);
        // 골드 정산
        int currentGold = BackendGameData.Instance.UserGameData.gold;
        int afterGold = currentGold - GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.CharacterTable, _category[_categoryIndex]._myPick + 1, GoogleSheetManager.eCharacterTableIndex.Cost.ToString());

        UpdateMyCharacterGoldData(afterGold);
        BackendGameData.Instance.UserGameData.gold = afterGold;
        BackendGameData.Instance.UserGameData.useCrystal += GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.CharacterTable, _category[_categoryIndex]._myPick + 1, GoogleSheetManager.eCharacterTableIndex.Cost.ToString());

        // 캐릭터 해금
        int characterdata = 0;
        if (_categoryIndex == (int)eDictionaryCategory.Character)
        {
            UpdateCharacterData(characterdata, ((eCharacter)_category[_categoryIndex]._myPick).ToString());
        }

        // 여기 카테고리 값
        if (_categoryIndex == (int)eDictionaryCategory.Character)
        {
            BackendGameData.Instance.SetBodyState(_category[_categoryIndex]._myPick, 0);
        }

        // 화면 반영
        _topViewer.UpdateGameGoldData();

        Process();
    }
    /// <summary>
    /// 취소 버튼
    /// </summary>
    public void PurchaseOn()
    {
        int currentGold = BackendGameData.Instance.UserGameData.gold;

        if (currentGold < BackendChartData.characterChart[_category[_categoryIndex]._myPick].price)
            _cantPurchaseWindow.SetActive(true);
        else
            _purchaseWindow.SetActive(true);
    }
    /// <summary>
    /// 취소 버튼
    /// </summary>
    public void PurchaseOff()
    {
        _purchaseWindow.SetActive(false);
    }
    public void CantPurchaseOff()
    {
        _cantPurchaseWindow.SetActive(false);
    }

    #region [ 백엔드 작업 ]
    /// <summary>
    /// 구매 후 재화 등록
    /// </summary>
    /// <param name="newGold"></param>
    public void CharacterInsert(int newGold)
    {
        string inDate = string.Empty;
        Backend.GameData.GetMyData(Constants.USER_DATA_TABLE, new Where(), callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"데이터 조회 중 문제가 발생했습니다.: {callback}");
                return;
            }

            Debug.Log($"데이터 조회에 성공했습니다. : {callback}");

            if (callback.FlattenRows().Count > 0)
            {
                inDate = callback.FlattenRows()[0]["inDate"].ToString();
            }
            else
            {
                Debug.LogError("데이터가 존재하지 않습니다.");
                return;
            }

            Param param = new Param()
            {
                { "gold", newGold }
            };

            Backend.GameData.UpdateV2(Constants.USER_DATA_TABLE, inDate, Backend.UserInDate, param, (callback) =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log($"재화 등록에 성공했습니다. : {callback}");
                }
                else
                {
                    Debug.LogError($"재화 등록에 실패했습니다. : {callback}");
                }
            });
        });
    }
    /// <summary>
    /// 구매 후 재화 등록
    /// </summary>
    /// <param name="newGold"> 테이블에 추가할 골드 </param>
    public void UpdateMyCharacterGoldData(int newGold)
    {
        Backend.GameData.GetMyData(Constants.USER_DATA_TABLE, new Where(), callback =>
        {
            if (callback.IsSuccess())
            {
                try
                {
                    LitJson.JsonData goldDataJson = callback.FlattenRows();

                    if (goldDataJson.Count <= 0)
                    {
                        Debug.LogWarning("데이터가 존재하지 않습니다.");
                    }
                    else
                    {
                        //lastGold는 현재 재화 
                        CharacterInsert(newGold);
                        Debug.Log("캐릭터 구매 후, 최종 재화 등록 완료");
                    }
                }

                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
            else
            {
                Debug.Log("재화 업데이트 부분에서 콜백 실패함");
            }
        });

    }

    /// <summary>
    /// 캐릭터 구매 후
    /// </summary>
    /// <param name="newB"></param>
    public void UpdateCharacterData(int newB, string name)
    {
        Backend.GameData.GetMyData(Constants.USER_DATA_TABLE, new Where(), callback =>
        {
            if (callback.IsSuccess())
            {
                try
                {
                    LitJson.JsonData character1DataJson = callback.FlattenRows();

                    if (character1DataJson.Count <= 0)
                    {
                        Debug.LogWarning("데이터가 존재하지 않습니다.");
                    }
                    else
                    {
                        //lastGold는 현재 재화 
                        CharacterData(newB, name);
                        Debug.Log("캐릭터 구매 후, 최종 재화 등록 완료");
                    }
                }

                catch (System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
            else
            {
                Debug.Log("재화 업데이트 부분에서 콜백 실패함");
            }
        });
    }
    /// <summary>
    /// 캐릭터 구매 후 데이터 변경
    /// </summary>
    /// <param name="newB"></param>
    public void CharacterData(int newB, string name)
    {
        string inDate = string.Empty;
        Backend.GameData.GetMyData(Constants.USER_DATA_TABLE, new Where(), callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"데이터 조회 중 문제가 발생했습니다.: {callback}");
                return;
            }

            Debug.Log($"데이터 조회에 성공했습니다. : {callback}");

            if (callback.FlattenRows().Count > 0)
            {
                inDate = callback.FlattenRows()[0]["inDate"].ToString();
            }
            else
            {
                Debug.LogError("데이터가 존재하지 않습니다.");
                return;
            }

            Param param = new Param()
            {
                { name, newB }
            };

            Backend.GameData.UpdateV2(Constants.USER_DATA_TABLE, inDate, Backend.UserInDate, param, (callback) =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log($"캐릭터 구매 후 2로 등록 성공했습니다. : {callback}");
                }
                else
                {
                    Debug.LogError($"캐릭터 구매 후 1로 등록 실패했습니다. : {callback}");
                }
            });
        });
    }
    #endregion [ 백엔드 작업 ]
}

