using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCanvas : MonoBehaviour
{
    const string PlayStoreLink = "market://details?id=com.DefaultCompany.CrystalLight_Demo";
    //const string AppStoreLink = "itms-apps://itunes.apple.com/app/��ID";

    // �Ʒ��� OpenUpdateUI �Լ��� �̿��Ͽ� ������Ʈ UI�� Ȱ��ȭ �Ǿ���,
    // ������Ʈ UI ��ü ���� Ȯ�� ��ư�� ������
    // �ش� ��ư Ŭ�� �� �Ʒ� �Լ��� ȣ�� �� ���
    // �� OS ȯ�濡 ���� ������ ����� URL�� �̵��ϵ��� �ϴ� �Լ��Դϴ�.
    public void OpenStoreLink()
    {
#if UNITY_ANDROID
        Application.OpenURL(PlayStoreLink);
        //#elif UNITY_IOS
        //Application.OpenURL(AppStoreLink);
#else
      Debug.LogError("�������� �ʴ� �÷��� �Դϴ�.");
#endif
    }

    public void OKBtn()
    {
        OpenStoreLink();
        Application.Quit();
    }
}
