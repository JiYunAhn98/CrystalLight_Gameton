using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCanvas : MonoBehaviour
{
    const string PlayStoreLink = "market://details?id=com.DefaultCompany.CrystalLight_Demo";
    //const string AppStoreLink = "itms-apps://itunes.apple.com/app/앱ID";

    // 아래는 OpenUpdateUI 함수를 이용하여 업데이트 UI가 활성화 되었고,
    // 업데이트 UI 객체 내에 확인 버튼이 있으며
    // 해당 버튼 클릭 시 아래 함수를 호출 할 경우
    // 각 OS 환경에 따라 각각의 스토어 URL로 이동하도록 하는 함수입니다.
    public void OpenStoreLink()
    {
#if UNITY_ANDROID
        Application.OpenURL(PlayStoreLink);
        //#elif UNITY_IOS
        //Application.OpenURL(AppStoreLink);
#else
      Debug.LogError("지원하지 않는 플랫폼 입니다.");
#endif
    }

    public void OKBtn()
    {
        OpenStoreLink();
        Application.Quit();
    }
}
