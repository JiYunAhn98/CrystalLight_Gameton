using UnityEngine.UI;
using TMPro;
using UnityEngine;
using BackEnd;

public class Heart : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textGold;
    [SerializeField]
    private TextMeshProUGUI textHeart;
    public GameObject purchaseWindow;

    public void Process()
    {
        purchaseWindow.SetActive(false);
        int heartPrice = 70;
        BackendGameData.Instance.UserGameData.gold -= heartPrice;
        BackendGameData.Instance.UserGameData.heart += 1;
        BackendGameData.Instance.GameDataUpdate();
        Debug.Log("부활 아이템 구매 완료");
        int afterGold = BackendGameData.Instance.UserGameData.gold;
        textGold.text = $"{afterGold}";
        int afterHeart = BackendGameData.Instance.UserGameData.heart;
        textHeart.text = $"{afterHeart}";
    }

    public void ShowPurchasePopup()
    {
        purchaseWindow.SetActive(true);
    }

    public void CanclePurchasing()
    {
        purchaseWindow.SetActive(false);
    }
}
