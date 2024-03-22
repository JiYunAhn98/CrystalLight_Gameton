using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class IngameMainCanvas : MonoBehaviour
{
    [SerializeField] GameObject _start;
    [SerializeField] TextMeshProUGUI _score;
    [SerializeField] TextMeshProUGUI _countDown;
    [SerializeField] GameObject _stopPopup;
    [SerializeField] GameObject _heart;
    [SerializeField] GameObject _crystalLife;
    [SerializeField] GameObject _deadImmune;
    [SerializeField] RectTransform _circle;
    [SerializeField] TextMeshProUGUI _tutorialMaxScore;
    float _time;

    public void Initialize()
    {
        _countDown.gameObject.SetActive(false);
        _circle.gameObject.SetActive(false);
        _time = 3;
    }
    public void SetStartMessage(bool isOn)
    {
        _start.SetActive(isOn);
    }
    public void SetScore(int value)
    {
        _score.text = value.ToString();
    }
    public void SetTutorialMaxScore()
    {
        _tutorialMaxScore.gameObject.SetActive(true);
        _tutorialMaxScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = Score.Instance.GetTutorialScore().ToString();
    }
    public void SetHeart(bool isOn)
    {
        _heart.SetActive(isOn);
    }
    public void SetCrystalLife(bool isOn)
    {
        _crystalLife.SetActive(isOn);
    }
    public void SetDeadImmune(bool isOn)
    {
        _deadImmune.SetActive(isOn);
    }
    public void SetCirclePos(Vector2 pos)
    {
        CanvasScaler cs = GetComponentInChildren<CanvasScaler>();
        RectTransform rt = GetComponent<RectTransform>();

        float wRatio = cs.referenceResolution.x / Screen.width;
        float hRatio = cs.referenceResolution.y / Screen.height;

        // 결과 비율값
        float ratio = wRatio * (1f - cs.matchWidthOrHeight) + hRatio * (cs.matchWidthOrHeight);

        // 현재 스크린에서 RectTransform의 실제 너비, 높이
        float pixelWidth = Screen.width * ratio;
        float pixelHeight = Screen.height * ratio;

        _circle.gameObject.SetActive(true);
        _circle.anchoredPosition = new Vector2(pos.x / Screen.width * pixelWidth, pos.y / Screen.height * pixelHeight);
    }
    public void CircleActiveFalse()
    {
        _circle.gameObject.SetActive(false);
    }
    public float CountDown(float time)
    {
        _countDown.gameObject.SetActive(true);
        if (_time <= 0) _time = time;
        _countDown.text = ((int)_time).ToString();
        _time -= Time.deltaTime;
        if (_time <= 0) _countDown.gameObject.SetActive(false);
        return _time;
    }
    public void StopBtn()
    {
        _stopPopup.SetActive(true);
        InGameManager._instance.ProgStop();
    }
}
