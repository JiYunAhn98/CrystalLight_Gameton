using UnityEngine;
using DefineHelper;
public class LogoScenario : MonoBehaviour
{

    [SerializeField]
    private eSceneName nextScene;
    private void Awake()
    {
        SystemSetup();
    }

    private void SystemSetup()
    {
        //활성화되지 않은 상태에서도 게임이 계속 진행
        Application.runInBackground = true;

        ////해상도 설정 (9:18.5, 1440X2960)
        //int width = Screen.width;
        //int height = (int)(Screen.width * 18.5f / 9);
        //Screen.SetResolution(width, height, true);

        //화면이 꺼지지 않도록 설정 
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        SceneLoadManager._instance.LoadScene(nextScene);
    }
}
