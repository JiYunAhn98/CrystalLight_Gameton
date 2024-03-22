using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BackEnd;
using DefineHelper;

public class Score : MonoBehaviour
{
    readonly float NORMAL_REWARD_VAL = 0.1f;
    readonly float FAST_REWARD_VAL = 0.12f;
    readonly float HARD_REWARD_VAL = 0.14f;

    readonly int SCORE_PER_TIME = 10;
    readonly int SCORE_FOR_TUTORIAL1 = 800;
    readonly int SCORE_FOR_TUTORIAL2 = 1000;
    readonly int SCORE_FOR_TUTORIAL3 = 1200;

    public TextMeshProUGUI scoreText;
    public float elapsedTime;
    public bool isGameOver = false;         //게임오버 상태를 추적하는 변수 추가
    public GameController gameController;
    public float plusScore;

    public static Score Instance { get; private set; }
    //score클래스가 GameController와 상호작용함 
    public void Setup(GameController gameController)
    {
        this.gameController = gameController;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    public void Start()
    {
        elapsedTime = 0f;
        plusScore = 0;
        UpdateScoreText(); 
        isGameOver = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            elapsedTime += Time.deltaTime * SCORE_PER_TIME; // 초당 10점
            UpdateScoreText();
        }
    }

    public int GetScore()
    {
        return Mathf.RoundToInt(elapsedTime) + (int)plusScore;
    }
    public int GetReward()
    {
        switch (BackendGameData.Instance.UserGameData.selectInfiniteLevel)
        {
            case (int)eInfiniteLevel.NORMAL:
                return (int)(GetScore() * NORMAL_REWARD_VAL);

            case (int)eInfiniteLevel.FAST:
                return (int)(GetScore() * FAST_REWARD_VAL);

            case (int)eInfiniteLevel.HARD:
                return (int)(GetScore() * HARD_REWARD_VAL);
        }
        return 0;
    }
   
    public int GetTutorialScore()
    {
        switch (BackendGameData.Instance.UserGameData.selectTutorialLevel)
        {
            case (int)eTutorialLevel.STORY1:
                return SCORE_FOR_TUTORIAL1;

            case (int)eTutorialLevel.STORY2:
                return SCORE_FOR_TUTORIAL2;

            case (int)eTutorialLevel.STORY3:
                return SCORE_FOR_TUTORIAL3;
        }
        return 0;
    }
    void UpdateScoreText()
    {
        if (BackendGameData.Instance.UserGameData.selectMode == (int)eLevelMode.Story)
        {
            scoreText.text = ((int)(Mathf.Round(elapsedTime) + plusScore)).ToString();
        }
        else
        {
            scoreText.text = ((int)(Mathf.Round(elapsedTime) + plusScore)).ToString();
        }
    }

    public bool TutorialComplete()
    {
        switch (BackendGameData.Instance.UserGameData.selectTutorialLevel)
        {
            case (int)eTutorialLevel.STORY1:
                return GetScore() >= SCORE_FOR_TUTORIAL1;

            case (int)eTutorialLevel.STORY2:
                return GetScore() >= SCORE_FOR_TUTORIAL2;

            case (int)eTutorialLevel.STORY3:
                return GetScore() >= SCORE_FOR_TUTORIAL3;
        }
        return false;
    }

    public void BonusScore(float score)
    {
        plusScore += score;
    }

    public void EndGame(bool end)
    {
        isGameOver = end;
    }
}