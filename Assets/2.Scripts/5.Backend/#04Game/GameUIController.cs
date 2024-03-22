using UnityEngine;
using TMPro;

public class GameUIController : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;

    [Header("Game Over")]
    [SerializeField]
    private GameObject panelGameOver;

    public void OnGameOver()
    {
        //게임오버패널UI 활성화
        panelGameOver.SetActive(true);
    }

    public void BtnClickGoToLobby()
    {
        SceneLoadManager._instance.LoadScene(DefineHelper.eSceneName.Lobby);
    }
}
