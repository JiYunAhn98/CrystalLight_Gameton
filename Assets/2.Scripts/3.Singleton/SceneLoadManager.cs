using UnityEngine.SceneManagement;
using DefineHelper;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneLoadManager : MonoSingleton<SceneLoadManager>
{
    // Start is called before the first frame update
    [Header("Fade BG")]
    [SerializeField] RectTransform _oreBG;

    [Header("GameName Alpha")]
    [SerializeField] Image _BG;
    [SerializeField] Image _BGLight;
    [SerializeField] TextMeshProUGUI _gameName;

    public string GetActiveScene()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void LoadScene(string sceneName = "")
    {
        if (sceneName == "")
        {
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void LoadScene(eSceneName sceneName, bool isEffect = false)
    {
        //씬네임 열거형으로 매개변수를 받아온 경우 Tostring() 처리
        if (isEffect)
        {
            StartCoroutine(SceneLoading(sceneName.ToString()));
        }
        else
        {
            SceneManager.LoadScene(sceneName.ToString());
        }
    }

    public IEnumerator SceneLoading(string sceneName)
    {
        _oreBG.gameObject.SetActive(true);
        _BG.transform.parent.gameObject.SetActive(true);

        while (_oreBG.localScale.x > 1)
        {
            yield return null;

            _oreBG.localScale -= Time.deltaTime * 15 * Vector3.one;
            _gameName.alpha += Time.deltaTime;
            _BG.color += Color.black * Time.deltaTime;
            _BGLight.color += Color.black * Time.deltaTime;
        }

        if (_oreBG.localScale.x < 1)
        {
            _oreBG.localScale = Vector3.one;
        }
        _gameName.alpha = 1;

        SceneManager.LoadScene(sceneName);
        yield return new WaitForSeconds(0.1f);

        while (_oreBG.localScale.x < 20)
        {
            _oreBG.localScale += Time.deltaTime * 15 * Vector3.one;
            _gameName.alpha -= Time.deltaTime;
            _BG.color -= Color.black * Time.deltaTime;
            _BGLight.color -= Color.black * Time.deltaTime;
            yield return null;
        }

        _gameName.alpha = 0;
        _oreBG.gameObject.SetActive(false);
        _BG.transform.parent.gameObject.SetActive(false);
    }
}
