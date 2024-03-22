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
        //��ư �� Ȱ�� , ��Ȱ�� ����
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
        Debug.Log("�ʱ�ȭ ���� : " + error);
    }
    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.Log("�ʱ�ȭ ���� : " + error + message);
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log("���� ����");
        stateText.text = "���� ����...";
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        var product = purchaseEvent.purchasedProduct;
        Debug.Log("���ż��� : " + product.definition.id);

        if (product.definition.id == package_1)             // ��Ȱ��Ű��
        {
            BackendGameData.Instance.UserGameData.heart += 20;
            BackendGameData.Instance.UserGameData.gold += 500;
            stateText.text = "��Ű�� 1 ���� ����";
        }
        else if (product.definition.id == package_2)        // ǥ���� ��Ű��
        {
            BackendGameData.Instance.UserGameData.heart += 2;
            BackendGameData.Instance.UserGameData.faceTicket += 5;
            BackendGameData.Instance.UserGameData.gold += 500;
            stateText.text = "��Ű�� 2 ���� ����";
        }
        else if (product.definition.id == package_3)        // �Ǽ��� ��Ű��
        {
            BackendGameData.Instance.UserGameData.heart += 2;
            BackendGameData.Instance.UserGameData.accessoryTicket += 5;
            BackendGameData.Instance.UserGameData.gold += 500;
            stateText.text = "��Ű�� 3 ���� ����";
        }
        else if (product.definition.id == package_4)        // �ٲ� ��Ű��
        {
            BackendGameData.Instance.UserGameData.faceTicket += 5;
            BackendGameData.Instance.UserGameData.accessoryTicket += 5;
            BackendGameData.Instance.UserGameData.gold += 800;
            BackendStoreData.Instance.StoreGameData.package4BuyNum--;
            stateText.text = "��Ű�� 4 ���� ����";
        }
        else if (product.definition.id == package_5)        // �������� ��Ű��
        {
            BackendGameData.Instance.UserGameData.heart += 5;
            BackendGameData.Instance.UserGameData.faceTicket += 5;
            BackendGameData.Instance.UserGameData.accessoryTicket += 5;
            BackendGameData.Instance.UserGameData.gold += 5000;
            BackendStoreData.Instance.StoreGameData.package5BuyNum--;
            stateText.text = "��Ű�� 5 ���� ����";
        }

        else if (product.definition.id == crystal_500)        // �ٲ� ��Ű��
        {
            BackendGameData.Instance.UserGameData.gold += 500;
            stateText.text = "ũ����Ż 500 ���� ����";
        }
        else if (product.definition.id == crystal_800)        // �ٲ� ��Ű��
        {
            BackendGameData.Instance.UserGameData.gold += 800;

            stateText.text = "ũ����Ż 800 ���� ����";
        }
        else if (product.definition.id == crystal_1000)        // �ٲ� ��Ű��
        {
            BackendGameData.Instance.UserGameData.gold += 1000;

            if (BackendStoreData.Instance.StoreGameData.crystal_1000BuyNotYet)
            {
                BackendGameData.Instance.UserGameData.gold += 1000;
                BackendStoreData.Instance.StoreGameData.crystal_1000BuyNotYet = false;
            }
            stateText.text = "ũ����Ż 1000 ���� ����";
        }
        else if (product.definition.id == crystal_1500)        // �ٲ� ��Ű��
        {
            BackendGameData.Instance.UserGameData.gold += 1500;

            if (BackendStoreData.Instance.StoreGameData.crystal_1500BuyNotYet)
            {
                BackendGameData.Instance.UserGameData.gold += 1500;
                BackendStoreData.Instance.StoreGameData.crystal_1500BuyNotYet = false;
            }
            stateText.text = "ũ����Ż 1500 ���� ����";
        }
        else if (product.definition.id == crystal_3000)        // �ٲ� ��Ű��
        {
            BackendGameData.Instance.UserGameData.gold += 3000;

            if (BackendStoreData.Instance.StoreGameData.crystal_3000BuyNotYet)
            {
                BackendGameData.Instance.UserGameData.gold += 3000;
                BackendStoreData.Instance.StoreGameData.crystal_3000BuyNotYet = false;
            }
            stateText.text = "ũ����Ż 3000 ���� ����";
        }
        else if (product.definition.id == crystal_4000)        // �ٲ� ��Ű��
        {
            BackendGameData.Instance.UserGameData.gold += 4000;

            if (BackendStoreData.Instance.StoreGameData.crystal_4000BuyNotYet)
            {
                BackendGameData.Instance.UserGameData.gold += 4000;
                BackendStoreData.Instance.StoreGameData.crystal_4000BuyNotYet = false;
            }
            stateText.text = "ũ����Ż 4000 ���� ����";
        }
        else if (product.definition.id == crystal_5000)        // �ٲ� ��Ű��
        {
            BackendGameData.Instance.UserGameData.gold += 5000;
            if (BackendStoreData.Instance.StoreGameData.crystal_5000BuyNotYet)
            {
                BackendGameData.Instance.UserGameData.gold += 5000;
                BackendStoreData.Instance.StoreGameData.crystal_5000BuyNotYet = false;
            }
            stateText.text = "ũ����Ż 5000 ���� ����";
        }
        else if (product.definition.id == crystal_10000)        // �ٲ� ��Ű��
        {
            BackendGameData.Instance.UserGameData.gold += 10000;
            stateText.text = "ũ����Ż 10000 ���� ����";
        }

        else if (product.definition.id == remove_ad)
        {
            stateText.text = "�������� ���� ����";
            BackendStoreData.Instance.StoreGameData.eraseAdNotYet = false;
            ADManager.Instance._bannerAD.HideBannerAd();
            //ADBanner._instance.HideAd();
        }
        else if (product.definition.id == battle_pass)
        {
            stateText.text = "��Ʋ�н� ���� ����";
            BackendStoreData.Instance.StoreGameData.battlepass = true;
        }
        else
        {
            stateText.text = "���� ����...";
        }
        BackendGameData.Instance.GameDataUpdate();
        BackendStoreData.Instance.StoreDataUpdate();
        return PurchaseProcessingResult.Complete;
    }

    public void Purchase(string productID)
    {
        //��ư �̺�Ʈ ����
        //�����ϴ� ��ǰ ���̵� ������ ���������� �ʱ�ȭ�� 
        storeController.InitiatePurchase(productID);
        _block.gameObject.SetActive(true);
        stateText.text = "���� ��.....";
    }

    private void CheckNonConsumable(string id)
    {
        //���� ������ Ȯ��
        var product = storeController.products.WithID(id);

        if (product != null)
        {
            bool isCheck = product.hasReceipt;
        }
    }
}
