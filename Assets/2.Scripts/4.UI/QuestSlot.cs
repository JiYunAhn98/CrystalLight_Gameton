using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using BackEnd;
using DefineHelper;

public class QuestSlot : MonoBehaviour
{
    [SerializeField] TopPanelViewer _topview;
    [SerializeField] TextMeshProUGUI _questTxt;
    [SerializeField] TextMeshProUGUI _processTxt;
    [SerializeField] TextMeshProUGUI _rewardTxt;
    [SerializeField] Button _getRewardBtns;
    [SerializeField] GameObject _successMark;

    int _myGoogleIndex;
    int _mySlotIndex;
    eMissionType _type;

    public void Initialize(int myIndex)
    {
        _mySlotIndex = myIndex;

        _myGoogleIndex = BackendGameData.Instance.UserGameData.todayMission[_mySlotIndex];
        _questTxt.text = GoogleSheetManager._instance.TakeString(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Text.ToString());
        _rewardTxt.text = GoogleSheetManager._instance.TakeString(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Reward.ToString());
        _type = (eMissionType)Enum.Parse(typeof(eMissionType), GoogleSheetManager._instance.TakeString(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Type.ToString()));

        _processTxt.text = ProcessMission();
        // 아직 달성 전이면 전부 false
        if (_mySlotIndex == 3) // 광고는 그냥 보면 재화를 얻기 때문에 받았는지 확인할 필요가 없다.
        {
            _getRewardBtns.interactable = !CheckClearMission();
            _successMark.gameObject.SetActive(CheckClearMission());
        }
        else if (_mySlotIndex == 4) // 가챠권
        {
            _getRewardBtns.interactable = (BackendGameData.Instance.UserGameData.successMissionGacha < BackendGameData.Instance.UserGameData.missionGoGacha);
            _successMark.gameObject.SetActive(CheckClearMission());
        }
        else // 일일 미션
        {
            _getRewardBtns.interactable = (CheckClearMission() && !BackendGameData.Instance.UserGameData.successMission[_mySlotIndex]);         // 성공 후 획득 안하면 true
            _successMark.gameObject.SetActive(BackendGameData.Instance.UserGameData.successMission[_mySlotIndex]);
        }
    }
    public void GetRewardBtn()
    {
        if (_mySlotIndex == 3)
        {
            _getRewardBtns.interactable = false;
            ADManager.Instance._rewardAD.ShowRewardAd();
        }
        else
        {
            // 광고는 ADReward가 해준다.
            if (_mySlotIndex == 4) // 가챠권
            {
                BackendGameData.Instance.UserGameData.heart += (BackendGameData.Instance.UserGameData.missionGoGacha - BackendGameData.Instance.UserGameData.successMissionGacha);
                BackendGameData.Instance.UserGameData.successMissionGacha = BackendGameData.Instance.UserGameData.missionGoGacha;
            }
            else // 일일 미션
            {
                BackendGameData.Instance.UserGameData.successMission[_mySlotIndex] = true;
                BackendGameData.Instance.UserGameData.gold += GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Reward.ToString());
            }
            Initialize(_mySlotIndex);
            _topview.UpdateGameData();

            BackendGameData.Instance.GameDataUpdate();
        }
    }
    public IEnumerator GetRewardAD()
    {
        BackendGameData.Instance.UserGameData.gold += GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Reward.ToString());
        BackendGameData.Instance.UserGameData.missionWatchAD++;
        Initialize(_mySlotIndex);
        _topview.UpdateGameData();
        BackendGameData.Instance.GameDataUpdate();
        ADManager.Instance._rewardAD.LoadRewardAd();
        yield return new WaitForSeconds(10);
        _getRewardBtns.interactable = true;
    }
    public bool CheckClearMission()
    {
        switch (_type)
        {
            case eMissionType.PlayGame:
                //Debug.Log("PlayGame " + BackendGameData.Instance.UserGameData.missionPlay + " : " + GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Condition.ToString()));
                return BackendGameData.Instance.UserGameData.missionPlay >= GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Condition.ToString());

            case eMissionType.GetItem:
                //Debug.Log("GetItem " + BackendGameData.Instance.UserGameData.missionGetItem + " : " + GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Condition.ToString()));
                return BackendGameData.Instance.UserGameData.missionGetItem >= GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Condition.ToString());

            case eMissionType.NormalScore:
                //Debug.Log("NormalScore " + BackendGameData.Instance.UserGameData.missionNormalScore + " : " + GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Condition.ToString()));
                return  BackendGameData.Instance.UserGameData.missionNormalScore >= GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Condition.ToString());

            case eMissionType.FirePass:
                //Debug.Log("FirePass " + BackendGameData.Instance.UserGameData.missionFirePass + " : " + GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Condition.ToString()));
                return BackendGameData.Instance.UserGameData.missionFirePass >= GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Condition.ToString());

            case eMissionType.ElectronicPass:
                //Debug.Log("ElectronicPass " + BackendGameData.Instance.UserGameData.missionElectronicPass + " : " + GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Condition.ToString()));
                return BackendGameData.Instance.UserGameData.missionElectronicPass >= GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Condition.ToString());

            case eMissionType.SawBladePass:
                //Debug.Log("SawBladePass " + BackendGameData.Instance.UserGameData.missionSawBladePass + " : " + GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Condition.ToString()));
                return BackendGameData.Instance.UserGameData.missionSawBladePass >= GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Condition.ToString());

            case eMissionType.SafeTime:
                //Debug.Log("SafeTime " + BackendGameData.Instance.UserGameData.missionSafeTime + " : " + GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Condition.ToString()));
                return BackendGameData.Instance.UserGameData.missionSafeTime >= GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Condition.ToString());

            case eMissionType.WatchAD:
                //Debug.Log("WatchAD " + BackendGameData.Instance.UserGameData.missionWatchAD + " : " + GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Condition.ToString()));
                return BackendGameData.Instance.UserGameData.missionWatchAD >= GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Condition.ToString());

            case eMissionType.GoGacha:
                //Debug.Log("GoGacha " + BackendGameData.Instance.UserGameData.successMissionGacha + " : " + GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Condition.ToString()));
                return BackendGameData.Instance.UserGameData.successMissionGacha >= GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Condition.ToString());

        }
        return false;
    }
    public string ProcessMission()
    {
        int condition = GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.QuestChart, _myGoogleIndex, GoogleSheetManager.eQuestChartIndex.Condition.ToString());

        switch (_type)
        {
            case eMissionType.PlayGame:
                //Debug.Log("PlayGame " + _myGoogleIndex + " : " + BackendGameData.Instance.UserGameData.missionPlay);
                return  string.Format("{0}/{1}", BackendGameData.Instance.UserGameData.missionPlay, condition);

            case eMissionType.GetItem:
                //Debug.Log("GetItem " + _myGoogleIndex + " : " + BackendGameData.Instance.UserGameData.missionGetItem);
                return string.Format("{0}/{1}", BackendGameData.Instance.UserGameData.missionGetItem, condition);

            case eMissionType.NormalScore:
                ////Debug.Log("NormalScore " + _myGoogleIndex + " : " + BackendGameData.Instance.UserGameData.missionNormalScore);
                return string.Format("{0}/{1}", BackendGameData.Instance.UserGameData.missionNormalScore, condition);

            case eMissionType.FirePass:
                //Debug.Log("FirePass" + _myGoogleIndex + " : " + BackendGameData.Instance.UserGameData.missionFirePass);
                return string.Format("{0}/{1}", BackendGameData.Instance.UserGameData.missionFirePass, condition);

            case eMissionType.ElectronicPass:
                //Debug.Log("ElectronicPass" + _myGoogleIndex + " : " + BackendGameData.Instance.UserGameData.missionElectronicPass);
                return string.Format("{0}/{1}", BackendGameData.Instance.UserGameData.missionElectronicPass, condition);

            case eMissionType.SawBladePass:
                //Debug.Log("SawBladePass" + _myGoogleIndex + " : " + BackendGameData.Instance.UserGameData.missionSawBladePass);
                return string.Format("{0}/{1}", BackendGameData.Instance.UserGameData.missionSawBladePass, condition);

            case eMissionType.SafeTime:
                //Debug.Log("SafeTime" + _myGoogleIndex + " : " + BackendGameData.Instance.UserGameData.missionSafeTime);
                return string.Format("{0}/{1}", BackendGameData.Instance.UserGameData.missionSafeTime, condition);

            case eMissionType.WatchAD:
                //Debug.Log("WatchAD" + _myGoogleIndex + " : " + BackendGameData.Instance.UserGameData.missionWatchAD);
                return string.Format("{0}/{1}", BackendGameData.Instance.UserGameData.missionWatchAD, condition);

            case eMissionType.GoGacha:
                //Debug.Log("GoGacha" + _myGoogleIndex + " : " + BackendGameData.Instance.UserGameData.missionGoGacha);
                return string.Format("{0}/{1}", BackendGameData.Instance.UserGameData.missionGoGacha, condition);
        }
        return null;
    }
}
