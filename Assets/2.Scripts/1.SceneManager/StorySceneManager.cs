using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class StorySceneManager : MonoBehaviour
{
    [SerializeField] StoryTeller[] _stories;
    [SerializeField] GameObject _skipBtn;
    [SerializeField] float _speed;

    eSceneName _goSetting;

    void Start()
    {
        SoundManager._instance.PlayBGM(eBGMSound.Story1 + BackendGameData.Instance.UserGameData.selectTutorialLevel);

        if (InformManager._instance._tutorialComplete)
        {
            _stories[3].gameObject.SetActive(true);
            _skipBtn.SetActive(BackendGameData.Instance.UserGameData.tutorialLevel < (int)eTutorialLevel.Cnt);
        }
        else
        {
            _stories[BackendGameData.Instance.UserGameData.selectTutorialLevel].gameObject.SetActive(true);
            _skipBtn.SetActive(BackendGameData.Instance.UserGameData.selectTutorialLevel < BackendGameData.Instance.UserGameData.tutorialLevel);
        }

    }

    // Update is called once per frame
    void Update()
    {
        bool isOn;
        if (InformManager._instance._tutorialComplete)
        {
            isOn = _stories[3].UpdatePanel(_speed);
        }
        else
        {
            isOn = _stories[BackendGameData.Instance.UserGameData.selectTutorialLevel].UpdatePanel(_speed);
        }

        if (!_skipBtn.gameObject.activeSelf)
        {
            _skipBtn.SetActive(isOn);
        }
    }

    public void QuickScroll()
    {
        if (InformManager._instance._tutorialComplete)
        {
            InformManager._instance._tutorialComplete = false;
            if(BackendGameData.Instance.UserGameData.tutorialLevel < (int)eTutorialLevel.Cnt)
                BackendGameData.Instance.UserGameData.tutorialLevel++;
            BackendGameData.Instance.GameDataUpdate();
            SceneLoadManager._instance.LoadScene(eSceneName.Lobby);
        }
        else
        {
            SceneLoadManager._instance.LoadScene(eSceneName.InGame);
        }
    }
}
