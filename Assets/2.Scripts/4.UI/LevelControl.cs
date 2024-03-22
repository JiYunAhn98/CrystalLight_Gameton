using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DefineHelper;

public class LevelControl : MonoBehaviour
{
    const int MIN_DIFFICULTY = 0;
    const int MAX_DIFFICULTY = 2;

    [SerializeField] Button increaseButton;
    [SerializeField] Button decreaseButton;
    [SerializeField] TextMeshProUGUI difficultyLabel;

    //난이도업 버튼 비활성화
    public void IncreaseDifficulty()
    {
        // 해금이 안됐다면 실행 X
        if (BackendGameData.Instance.UserGameData.selectMode == (int)eLevelMode.Infinite)
        {
            if (BackendGameData.Instance.UserGameData.selectInfiniteLevel < MAX_DIFFICULTY
                && BackendGameData.Instance.UserGameData.selectInfiniteLevel < BackendGameData.Instance.UserGameData.infiniteLevel)
            {
                BackendGameData.Instance.UserGameData.selectInfiniteLevel++;
            }
        }
        else
        {
            if (BackendGameData.Instance.UserGameData.selectTutorialLevel < MAX_DIFFICULTY
                && BackendGameData.Instance.UserGameData.selectTutorialLevel < BackendGameData.Instance.UserGameData.tutorialLevel)
            {
                BackendGameData.Instance.UserGameData.selectTutorialLevel++;
                //UpdateLevelStates();
            }
        }
    }
    //난이도다운버튼비활성화
    public void DecreaseDifficulty()
    {
        // 해금이 안됐다면 실행 X
        if (BackendGameData.Instance.UserGameData.selectMode == (int)eLevelMode.Infinite)
        {
            if (BackendGameData.Instance.UserGameData.selectInfiniteLevel > MIN_DIFFICULTY)
            {
                BackendGameData.Instance.UserGameData.selectInfiniteLevel--;
            }
        }
        else
        {
            if (BackendGameData.Instance.UserGameData.selectTutorialLevel > MIN_DIFFICULTY)
            {
                BackendGameData.Instance.UserGameData.selectTutorialLevel--;
            }
        }
    }

    public void UpdateLevelStates()
    {
        if (BackendGameData.Instance.UserGameData.selectMode == (int)eLevelMode.Infinite)
        {
            // 해금이 안됐다면 실행 interactable = false
            if (BackendGameData.Instance.UserGameData.selectInfiniteLevel >= MAX_DIFFICULTY
                || BackendGameData.Instance.UserGameData.selectInfiniteLevel >= BackendGameData.Instance.UserGameData.infiniteLevel)
            {
                increaseButton.interactable = false;
            }
            else
            {
                increaseButton.interactable = true;
            }
            if (BackendGameData.Instance.UserGameData.selectInfiniteLevel <= MIN_DIFFICULTY)
            {
                decreaseButton.interactable = false;
            }
            else
            {
                decreaseButton.interactable = true;
            }
            difficultyLabel.text = ((eInfiniteLevel)BackendGameData.Instance.UserGameData.selectInfiniteLevel).ToString();
        }
        else
        {
            // 해금이 안됐다면 실행 interactable = false
            if (BackendGameData.Instance.UserGameData.selectTutorialLevel >= MAX_DIFFICULTY
                || BackendGameData.Instance.UserGameData.selectTutorialLevel >= BackendGameData.Instance.UserGameData.tutorialLevel)
            {
                increaseButton.interactable = false;
            }
            else
            {
                increaseButton.interactable = true;
            }
            if (BackendGameData.Instance.UserGameData.selectTutorialLevel <= MIN_DIFFICULTY)
            {
                decreaseButton.interactable = false;
            }
            else
            {
                decreaseButton.interactable = true;
            }
            difficultyLabel.text = ((eTutorialLevel)BackendGameData.Instance.UserGameData.selectTutorialLevel).ToString();
        }
    }
}