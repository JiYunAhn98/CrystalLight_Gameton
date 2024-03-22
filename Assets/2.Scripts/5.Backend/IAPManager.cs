using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using TMPro;

public class IAPManager : MonoBehaviour, IStoreListener
{
    [SerializeField] GameObject _block;
    [SerializeField] TextMeshProUGUI stateText;

    private IStoreController storeController;

    private readonly string package_1 = "package1";
    private readonly string package_2 = "package2";
    private readonly string package_3 = "package3";
    private readonly string package_4 = "package4";
    private readonly string package_5 = "package5";

    private readonly string crystal_500 = "crystal_500";
    private readonly string crystal_800 = "crystal_800";
    private readonly string crystal_1000 = "crystal_1000";
    private readonly string crystal_1500 = "crystal_1500";
    private readonly string crystal_3000 = "crystal_3000";
    private readonly string crystal_4000 = "crystal_4000";
    private readonly string crystal_5000 = "crystal_5000";
    private readonly string crystal_10000 = "crystal_10000";

    private string remove_ad = "remove_ad";
    private string battle_pass = "battlepass";

    void Start()
    {
        InitIAP();
        _block.gameObject.SetActive(false);
    }

    private void InitIAP()
    {
        //버튼 등 활성 , 비활성 설정
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(package_1, ProductType.Consumable);
        builder.AddProduct(package_2, ProductType.Consumable);
        builder.AddProduct(package_3, ProductType.Consumable);
        builder.AddProduct(package_4, ProductType.Consumable);
        builder.AddProduct(package_5, ProductType.Consumable);

        builder.AddProduct(crystal_500, ProductType.Consumable);
        builder.AddProduct(crystal_800, ProductType.Consumable);
        builder.AddProduct(crystal_1000, ProductType.Consumable);
        builder.AddProduct(crystal_1500, ProductType.Consumable);
        builder.AddProduct(crystal_3000, ProductType.Consumable);
        builder.AddProduct(crystal_4000, ProductType.Consumable);
        builder.AddProduct(crystal_5000, ProductType.Consumable);
        builder.AddProduct(crystal_10000, ProductType.Consumable);

        builder.AddProduct(remove_ad, ProductType.NonConsumable);
        builder.AddProduct(battle_pass, ProductType.Consumable);

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;

        CheckNonConsumable(remove_ad);

    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("초기화 실패 : " + error);
    }
    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("초기화 실패 : " + error + message);
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log("구매 실패");
        stateText.text = "구매 실패...";
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var product = purchaseEvent.purchasedProduct;
        Debug.Log("구매성공 : " + product.definition.id);

        if (product.definition.id == package_1)             // 부활패키지
        {
            BackendGameData.Instance.UserGameData.heart += 20;
            BackendGameData.Instance.UserGameData.gold += 500;
            stateText.text = "패키지 1 구매 성공";
        }
        else if (product.definition.id == package_2)        // 표정권 패키지
        {
            BackendGameData.Instance.UserGameData.heart += 2;
            BackendGameData.Instance.UserGameData.faceTicket += 5;
            BackendGameData.Instance.UserGameData.gold += 500;
            stateText.text = "패키지 2 구매 성공";
        }
        else if (product.definition.id == package_3)        // 악세권 패키지
        {
            BackendGameData.Instance.UserGameData.heart += 2;
            BackendGameData.Instance.UserGameData.accessoryTicket += 5;
            BackendGameData.Instance.UserGameData.gold += 500;
            stateText.text = "패키지 3 구매 성공";
        }
        else if (product.definition.id == package_4)        // 꾸꾸 패키지
        {
            BackendGameData.Instance.UserGameData.faceTicket += 5;
            BackendGameData.Instance.UserGameData.accessoryTicket += 5;
            BackendGameData.Instance.UserGameData.gold += 800;
            BackendStoreData.Instance.StoreGameData.package4BuyNum--;
            stateText.text = "패키지 4 구매 성공";
        }
        else if (product.definition.id == package_5)        // 보물상자 패키지
        {
            BackendGameData.Instance.UserGameData.heart += 5;
            BackendGameData.Instance.UserGameData.faceTicket += 5;
            BackendGameData.Instance.UserGameData.accessoryTicket += 5;
            BackendGameData.Instance.UserGameData.gold += 5000;
            BackendStoreData.Instance.StoreGameData.package5BuyNum--;
            stateText.text = "패키지 5 구매 성공";
        }

        else if (product.definition.id == crystal_500)        // 꾸꾸 패키지
        {
            BackendGameData.Instance.UserGameData.gold += 500;
            stateText.text = "크리스탈 500 구매 성공";
        }
        else if (product.definition.id == crystal_800)        // 꾸꾸 패키지
        {
            BackendGameData.Instance.UserGameData.gold += 800;

            stateText.text = "크리스탈 800 구매 성공";
        }
        else if (product.definition.id == crystal_1000)        // 꾸꾸 패키지
        {
            BackendGameData.Instance.UserGameData.gold += 1000;

            if (BackendStoreData.Instance.StoreGameData.crystal_1000BuyNotYet)
            {
                BackendGameData.Instance.UserGameData.gold += 1000;
                BackendStoreData.Instance.StoreGameData.crystal_1000BuyNotYet = false;
            }
            stateText.text = "크리스탈 1000 구매 성공";
        }
        else if (product.definition.id == crystal_1500)        // 꾸꾸 패키지
        {
            BackendGameData.Instance.UserGameData.gold += 1500;

            if (BackendStoreData.Instance.StoreGameData.crystal_1500BuyNotYet)
            {
                BackendGameData.Instance.UserGameData.gold += 1500;
                BackendStoreData.Instance.StoreGameData.crystal_1500BuyNotYet = false;
            }
            stateText.text = "크리스탈 1500 구매 성공";
        }
        else if (product.definition.id == crystal_3000)        // 꾸꾸 패키지
        {
            BackendGameData.Instance.UserGameData.gold += 3000;

            if (BackendStoreData.Instance.StoreGameData.crystal_3000BuyNotYet)
            {
                BackendGameData.Instance.UserGameData.gold += 3000;
                BackendStoreData.Instance.StoreGameData.crystal_3000BuyNotYet = false;
            }
            stateText.text = "크리스탈 3000 구매 성공";
        }
        else if (product.definition.id == crystal_4000)        // 꾸꾸 패키지
        {
            BackendGameData.Instance.UserGameData.gold += 4000;

            if (BackendStoreData.Instance.StoreGameData.crystal_4000BuyNotYet)
            {
                BackendGameData.Instance.UserGameData.gold += 4000;
                BackendStoreData.Instance.StoreGameData.crystal_4000BuyNotYet = false;
            }
            stateText.text = "크리스탈 4000 구매 성공";
        }
        else if (product.definition.id == crystal_5000)        // 꾸꾸 패키지
        {
            BackendGameData.Instance.UserGameData.gold += 5000;
            if (BackendStoreData.Instance.StoreGameData.crystal_5000BuyNotYet)
            {
                BackendGameData.Instance.UserGameData.gold += 5000;
                BackendStoreData.Instance.StoreGameData.crystal_5000BuyNotYet = false;
            }
            stateText.text = "크리스탈 5000 구매 성공";
        }
        else if (product.definition.id == crystal_10000)        // 꾸꾸 패키지
        {
            BackendGameData.Instance.UserGameData.gold += 10000;
            stateText.text = "크리스탈 10000 구매 성공";
        }

        else if (product.definition.id == remove_ad)
        {
            stateText.text = "광고제거 구매 성공";
            BackendStoreData.Instance.StoreGameData.eraseAdNotYet = false;
            ADManager.Instance._bannerAD.HideBannerAd();
            //ADBanner._instance.HideAd();
        }
        else if (product.definition.id == battle_pass)
        {
            stateText.text = "배틀패스 구매 성공";
            BackendStoreData.Instance.StoreGameData.battlepass = true;
        }
        else
        {
            stateText.text = "구매 실패...";
        }
        BackendGameData.Instance.GameDataUpdate();
        BackendStoreData.Instance.StoreDataUpdate();
        return PurchaseProcessingResult.Complete;
    }

    public void Purchase(string productID)
    {
        //버튼 이벤트 연결
        //구매하는 상품 아이디를 전달해 결제과정을 초기화함 
        storeController.InitiatePurchase(productID);
        _block.gameObject.SetActive(true);
        stateText.text = "구매 중.....";
    }

    private void CheckNonConsumable(string id)
    {
        //구매 영수증 확인
        var product = storeController.products.WithID(id);

        if (product != null)
        {
            bool isCheck = product.hasReceipt;
        }
    }
}
