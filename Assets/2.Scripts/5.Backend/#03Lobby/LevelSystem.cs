using UnityEngine;
using DefineHelper;

public class LevelSystem : MonoBehaviour
{
	/// <summary>
	/// 끝나고 백엔드 임무, 정보 저장 용
	/// </summary>
	public void Process()
	{
		if (BackendGameData.Instance.UserGameData.silver > 1 && BackendGameData.Instance.UserGameData.gameCount >= 50)
		{
			InformManager._instance.EnqueueCharacter(eCharacter.Silver);
		}
		if (BackendGameData.Instance.UserGameData.emerald > 1 && BackendGameData.Instance.UserGameData.itemCount >= 600)
		{
			InformManager._instance.EnqueueCharacter(eCharacter.Emerald);
		}

		// 게임이 끝나면 작업할 내용
		if (BackendGameData.Instance.UserGameData.selectMode == (int)eLevelMode.Infinite)
		{
			switch (BackendGameData.Instance.UserGameData.selectInfiniteLevel)
			{
				case (int)eInfiniteLevel.NORMAL:		
					if (BackendGameData.Instance.UserGameData.selectInfiniteLevel >= BackendGameData.Instance.UserGameData.infiniteLevel)
					{
						if (Score.Instance.GetScore() >= 3000)		// 패스트모드 열기
						{
							BackendGameData.Instance.UserGameData.infiniteLevel++;
							InformManager._instance.EnqueueOthers(eLockThings.FastMode);
						}
					}
					if (BackendGameData.Instance.UserGameData.sapphire > 1)
					{
						if (Score.Instance.GetScore() >= 2500)      // 사파이어 열기
						{
							InformManager._instance.EnqueueCharacter(eCharacter.Sapphire);
						}
					}
					break;
				case (int)eInfiniteLevel.FAST:
					if (BackendGameData.Instance.UserGameData.selectInfiniteLevel >= BackendGameData.Instance.UserGameData.infiniteLevel)
					{
						if (Score.Instance.GetScore() >= 3000)      // 패스트모드 열기
						{
							BackendGameData.Instance.UserGameData.infiniteLevel++;
							InformManager._instance.EnqueueOthers(eLockThings.HardMode);
						}
					}
					if (BackendGameData.Instance.UserGameData.metal > 1)
					{
						if (Score.Instance.GetScore() >= 3000)      // 철 열기
						{
							InformManager._instance.EnqueueCharacter(eCharacter.Metal);
						}
					}
					break;
				case (int)eInfiniteLevel.HARD:
					if (BackendGameData.Instance.UserGameData.crystal > 1)
					{
						if (Score.Instance.GetScore() >= 3500)      // 크리스탈 열기
						{
							InformManager._instance.EnqueueCharacter(eCharacter.Crystal);
						}
					}
					break;
			}
		}

		else if (BackendGameData.Instance.UserGameData.selectMode == (int)eLevelMode.Story)
		{
			if (Score.Instance.TutorialComplete())
			{
				// 1 = 2해금, 2 = 3해금, 3 = 캐릭터해금
				if (BackendGameData.Instance.UserGameData.selectTutorialLevel >= BackendGameData.Instance.UserGameData.tutorialLevel)
				{
					switch (BackendGameData.Instance.UserGameData.selectTutorialLevel)
					{
						case (int)eTutorialLevel.STORY1:
							BackendGameData.Instance.UserGameData.accessoryTicket++;
							InformManager._instance.EnqueueOthers(eLockThings.STORY2);
							break;
						case (int)eTutorialLevel.STORY2:
							BackendGameData.Instance.UserGameData.faceTicket++;
							InformManager._instance.EnqueueOthers(eLockThings.STORY3);
							break;
						case (int)eTutorialLevel.STORY3:
							InformManager._instance.EnqueueCharacter(eCharacter.Copper);
							break;
					}
					BackendGameData.Instance.UserGameData.tutorialLevel++;
				}

				if (BackendGameData.Instance.UserGameData.selectTutorialLevel == (int)eTutorialLevel.STORY3)
				{
					InformManager._instance._tutorialComplete = true;
				}
			}
		}
	}
}

