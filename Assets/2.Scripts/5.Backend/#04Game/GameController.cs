using UnityEngine;
using UnityEngine.Events;
using DefineHelper;
using TMPro;

public class GameController : MonoBehaviour
{
	[SerializeField] UnityEvent onGameOver;   
	[SerializeField] RankRegister rankRegister; 
	[SerializeField] Gold gold;
	[SerializeField] TextMeshProUGUI _scoreTxt;
	public bool IsGameOver { set; get; } = false;

	public void GameOver()
	{
		//중복 처리 되지 않도록 bool 변수로 제어 즉 이미 게임이 끝난 상황이라면 그 밑에 있는 것들을 실행 안함  
		if (IsGameOver == true) return;
		IsGameOver = true;

		if (BackendGameData.Instance.UserGameData.gameCount % 3 == 2 && BackendStoreData.Instance.StoreGameData.eraseAdNotYet)
		{
			ADManager.Instance._interstitialAD.LoadInterstitialAd();
		}
		BackendGameData.Instance.UserGameData.gameCount++;

		// 게임오버 되었을 때 호출할 메소드들을 실행
		onGameOver.Invoke();

		if (BackendGameData.Instance.UserGameData.selectMode == (int)eLevelMode.Story) return;

		if (BackendGameData.Instance.UserGameData.selectMode == (int)eLevelMode.Infinite)
		{
			int currentScore = Score.Instance.GetScore();
			int goldCnt = 0;

			goldCnt = Score.Instance.GetReward();
			BackendGameData.Instance.UserGameData.experience += goldCnt / 2;

			// 현재 점수 정보를 바탕으로 랭킹 데이터 갱신
			if (BackendGameData.Instance.UserGameData.selectInfiniteLevel == (int)eInfiniteLevel.NORMAL)
			{
				rankRegister.NormalProcess(currentScore);
				BackendGameData.Instance.DoingMission(eMissionType.NormalScore, currentScore);
			}
			else if (BackendGameData.Instance.UserGameData.selectInfiniteLevel == (int)eInfiniteLevel.FAST)
			{
				rankRegister.FastProcess(currentScore);
			}
			else
			{ 
				rankRegister.HardProcess(currentScore);
			}
			_scoreTxt.text = goldCnt.ToString();
			gold.Process(goldCnt);
			BackendGameData.Instance.UserGameData.gold += goldCnt;
			BackendGameData.Instance.GameDataUpdate();
		}
	}
}
