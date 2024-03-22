using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharacterUnlockUI : MonoBehaviour
{
    [SerializeField] Sprite[] _characters;
    [SerializeField] Image _characterImg;
    [SerializeField] TextMeshProUGUI _txt;

    public void GetCharacter(DefineHelper.eCharacter character)
    {
        BackendGameData.Instance.SetBodyState((int)character, 1);
        _characterImg.sprite = PrefabManager._instance.GetCharacterThumbnail(character);
        _txt.text = GoogleSheetManager._instance.TakeString(GoogleSheetManager.eSheetIndex.CharacterTable, (int)character + 1, GoogleSheetManager.eCharacterTableIndex.NickName.ToString()) + " ÇØ±Ý!";
    }
}
