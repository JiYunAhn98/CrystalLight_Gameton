using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;
using UnityEngine.UI;
using TMPro;

public class LobbyPanel : ACPanel
{
    readonly int SIZE = 5;
    readonly int SIZE_SPEED = 10;

    [SerializeField] GameObject _StartHole;
    [SerializeField] GameObject _character;
    [SerializeField] LevelControl _levelInform;

    [Header("Ç¥Çö Ä³¸¯ÅÍ")]
    [SerializeField] MeshRenderer _nowPickCharacter;    // ¸öÅë
    [SerializeField] MeshRenderer _nowPickFace;         // ¾ó±¼
    [SerializeField] Transform _nowPickAccessory;       // ¾Ç¼¼

    RectTransform _rt;
    public override void Initialize()
    {
        _beforePanelClose = true;

        _rt = GetComponent<RectTransform>();
        _rt.localScale = Vector3.one;
        _StartHole.transform.position = Vector3.zero;
        _character.SetActive(true);
        _levelInform.UpdateLevelStates();
        gameObject.SetActive(true);
        for (int i = 0; i < (int)eAccessory.Cnt; i++)
        {
            Instantiate(PrefabManager._instance.GetAccessary((eAccessory)i), _nowPickAccessory).SetActive(false);
        }

        _nowPickCharacter.material = PrefabManager._instance.GetCharacterMaterial((eCharacter)BackendGameData.Instance.UserGameData.selectBody);
        _nowPickFace.GetComponent<MeshRenderer>().material = PrefabManager._instance.GetFaceMaterial((eFace)BackendGameData.Instance.UserGameData.selectFace);
        _nowPickAccessory.GetChild(BackendGameData.Instance.UserGameData.selectAccessory).gameObject.SetActive(true);
    }
    public override IEnumerator OpenMove()
    {
        float num;
        _nowPickCharacter.material = PrefabManager._instance.GetCharacterMaterial((eCharacter)BackendGameData.Instance.UserGameData.selectBody);
        _nowPickFace.GetComponent<MeshRenderer>().material = PrefabManager._instance.GetFaceMaterial((eFace)BackendGameData.Instance.UserGameData.selectFace);
        _nowPickAccessory.GetChild(BackendGameData.Instance.UserGameData.selectAccessory).gameObject.SetActive(true);
        _levelInform.UpdateLevelStates();

        while (_rt.localScale.x > 1)
        {
            yield return null;
            num = SIZE_SPEED * Time.deltaTime;
            _rt.localScale -= Vector3.one * num;
            _StartHole.transform.position += Vector3.up * num;
        }
        _rt.localScale = Vector3.one;
        _character.SetActive(true);
    }
    public override IEnumerator CloseMove()
    {
        float num;
        while (_rt.localScale.x < SIZE)
        {
            yield return null;
            num = SIZE_SPEED * Time.deltaTime;
            _rt.localScale += Vector3.one * num;
            _StartHole.transform.position += Vector3.down * num;
        }
        _rt.localScale = Vector3.one * SIZE;
    }
    public override void PanelUpdate()
    {
        _levelInform.UpdateLevelStates();
     }

    public void StartBtn()
    {
        BackendGameData.Instance.GameDataUpdate();
        if (BackendStoreData.Instance.StoreGameData.eraseAdNotYet)
        {
            ADManager.Instance._bannerAD.HideBannerAd();
        }

        // ¶³¾îÁö´Â ÀÌÆåÆ®
        _character.GetComponent<Rigidbody>().useGravity = true;

        if (BackendGameData.Instance.UserGameData.selectMode == (int)eLevelMode.Infinite)
        {
            SceneLoadManager._instance.LoadScene(eSceneName.InGame, true);
        }
        else
            SceneLoadManager._instance.LoadScene(eSceneName.Story, true);
    }
}
