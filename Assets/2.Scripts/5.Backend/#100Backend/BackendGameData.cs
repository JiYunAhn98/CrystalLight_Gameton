using UnityEngine;
using BackEnd;
using UnityEngine.Events;
using DefineHelper;
using System.Collections.Generic;

public class BackendGameData
{
	[System.Serializable]
	public class GameDataLoadEvent : UnityEngine.Events.UnityEvent { }
	public GameDataLoadEvent onGameDataLoadEvent = new GameDataLoadEvent();

	[SerializeField]
	private CharacterSystem characterSystem;

	private static BackendGameData instance = null;
	public static BackendGameData Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new BackendGameData();
			}

			return instance;
		}
	}
	private UserGameData userGameData = new UserGameData();
	public UserGameData UserGameData => userGameData;
	public static bool _isLoadFinish;

	private string gameDataRowInDate = string.Empty;

	public void Initilaize()
	{
		userGameData.Initilaize();
		_isLoadFinish = false;
	}
	/// <summary>
	/// 뒤끝 콘솔 테이블에 새로운 유저 정보 추가, 내가 설정을 어떻게 했냐, 지금 내 성장상태
	/// </summary>
	public void GameDataInsert()
	{
		// 유저 정보를 초기값으로 설정 
		userGameData.Reset();

		// 테이블에 추가할 데이터로 가공 
		Param param = new Param()
		{
			// 유저 성장 상태
			{ "infiniteLevel",      userGameData.infiniteLevel },
			{ "tutorialLevel",      userGameData.tutorialLevel },

			// 재화 상태
			{ "gold",       userGameData.gold },
			{ "jewel",      userGameData.jewel },
			{ "heart",      userGameData.heart },

			// 티켓
			{"accessoryTicket", userGameData.accessoryTicket},
			{"faceTicket", userGameData.faceTicket},

			// 일일 임무 상태
			{ "lastday",       userGameData.lastday },

			{ "missionPlay", userGameData.missionPlay },
			{ "missionGetItem", userGameData.missionGetItem },
			{ "missionNormalScore", userGameData.missionNormalScore },
			{ "missionFirePass", userGameData.missionFirePass },
			{ "missionElectronicPass", userGameData.missionElectronicPass },
			{ "missionSawBladePass", userGameData.missionSawBladePass },
			{ "missionWatchAD", userGameData.missionWatchAD },
			{ "missionSafeTime", userGameData.missionSafeTime },
			{ "missionGoGacha", userGameData.missionGoGacha },

			// 일일 임무 인덱스값
			{ "todayMission0", userGameData.todayMission[0] },
			{ "todayMission1", userGameData.todayMission[1] },
			{ "todayMission2", userGameData.todayMission[2] },
			{ "todayMission3", userGameData.todayMission[3] },
			{ "todayMission4", userGameData.todayMission[4] },
			
			// 일일 임무 수령확인 값
			{ "successMission0", userGameData.successMission[0] },
			{ "successMission1", userGameData.successMission[1] },
			{ "successMission2", userGameData.successMission[2] },
			{ "successMissionGacha", userGameData.successMissionGacha },
			
			// 해금용 임무 상태
			{ "itemCount",       userGameData.itemCount },
			{ "gameCount",      userGameData.gameCount },
			{ "useCrystal",      userGameData.useCrystal },
			{ "experience", userGameData.experience},

			// 랭크 데이터
			{ "normalBestScore", userGameData.normalBestScore},
			{ "fastBestScore", userGameData.fastBestScore},
			{ "hardBestScore", userGameData.hardBestScore},

			// 아이템 효과
			{ "crystalScore", userGameData.itemLevel[0]},
			{ "shieldTime", userGameData.itemLevel[1]},
			{ "magnetTime", userGameData.itemLevel[2]},
			{ "timeControlTime", userGameData.itemLevel[3]},

			// 유저 세팅 상태
			{ "isVibrateOn", userGameData.isVibrateOn },
			{ "isBGMOn", userGameData.isBGMOn },
			{ "isSFXOn", userGameData.isSFXOn },
			{ "selectMode", userGameData.selectMode },
			{ "selectInfiniteLevel", userGameData.selectInfiniteLevel },
			{ "selectTutorialLevel", userGameData.selectTutorialLevel },
			{ "selectFace", userGameData.selectFace },
			{ "selectBody", userGameData.selectBody },
			{ "selectAccessory", userGameData.selectAccessory },

			// 해금 및 구매 상태
			{ "Stone", userGameData.stone},
			{ "Copper", userGameData.copper},
			{ "Silver", userGameData.silver},
			{ "Realgold", userGameData.realgold},
			{"Ruby", userGameData.ruby},
			{"Sapphire", userGameData.sapphire },
			{"Emerald", userGameData.emerald},
			{"Diamond", userGameData.diamond },
			{"Metal", userGameData.metal },
			{"Crystal", userGameData.crystal },

			{ "face0", userGameData.faces[0] },
			{ "face1", userGameData.faces[1] },
			{ "face2", userGameData.faces[2] },
			{ "face3", userGameData.faces[3] },
			{ "face4", userGameData.faces[4] },
			{ "face5", userGameData.faces[5] },
			{ "face6", userGameData.faces[6] },
			{ "face7", userGameData.faces[7] },
			{ "face8", userGameData.faces[8] },
			{ "face9", userGameData.faces[9] },
			{ "face10", userGameData.faces[10] },
			{ "face11", userGameData.faces[11] },
			{ "face12", userGameData.faces[12] },
			{ "face13", userGameData.faces[13] },
			{ "face14", userGameData.faces[14] },
			{ "face15", userGameData.faces[15] },
			{ "face16", userGameData.faces[16] },
			{ "face17", userGameData.faces[17] },
			{ "face18", userGameData.faces[18] },
			{ "face19", userGameData.faces[19] },
			{ "face20", userGameData.faces[20] },
			{ "face21", userGameData.faces[21] },
			{ "face22", userGameData.faces[22] },
			{ "face23", userGameData.faces[23] },
			{ "face24", userGameData.faces[24] },
			{ "face25", userGameData.faces[25] },
			{ "face26", userGameData.faces[26] },
			{ "face27", userGameData.faces[27] },
			{ "face28", userGameData.faces[28] },
			{ "face29", userGameData.faces[29] },
			{ "face30", userGameData.faces[30] },

			{"accessory0", userGameData.accessories[0]},
			{"accessory1", userGameData.accessories[1]},
			{"accessory2", userGameData.accessories[2]},
			{"accessory3", userGameData.accessories[3]},
			{"accessory4", userGameData.accessories[4]},
			{"accessory5", userGameData.accessories[5]},
			{"accessory6", userGameData.accessories[6]},
			{"accessory7", userGameData.accessories[7]},
			{"accessory8", userGameData.accessories[8]},
			{"accessory9", userGameData.accessories[9]},
			{"accessory10", userGameData.accessories[10]},
			{"accessory11", userGameData.accessories[11]},
			{"accessory12", userGameData.accessories[12]},
			{"accessory13", userGameData.accessories[13]},
			{"accessory14", userGameData.accessories[14]},
			{"accessory15", userGameData.accessories[15]},
			{"accessory16", userGameData.accessories[16]},
			{"accessory17", userGameData.accessories[17]},
			{"accessory18", userGameData.accessories[18]},
			{"accessory19", userGameData.accessories[19]},
			{"accessory20", userGameData.accessories[20]},
			{"accessory21", userGameData.accessories[21]},
			{"accessory22", userGameData.accessories[22]},
			{"accessory23", userGameData.accessories[23]},
			{"accessory24", userGameData.accessories[24]},
			{"accessory25", userGameData.accessories[25]},
			{"accessory26", userGameData.accessories[26]},
			{"accessory27", userGameData.accessories[27]},
			{"accessory28", userGameData.accessories[28]},
			{"accessory29", userGameData.accessories[29]},
			{"accessory30", userGameData.accessories[30]},
			{"accessory31", userGameData.accessories[31]},
			{"accessory32", userGameData.accessories[32]},
			{"accessory33", userGameData.accessories[33]},
			{"accessory34", userGameData.accessories[34]},
			{"accessory35", userGameData.accessories[35]},
			{"accessory36", userGameData.accessories[36]},
			{"accessory37", userGameData.accessories[37]},
			{"accessory38", userGameData.accessories[38]},
			{"accessory39", userGameData.accessories[39]},
			{"accessory40", userGameData.accessories[40]},
			{"accessory41", userGameData.accessories[41]},
			{"accessory42", userGameData.accessories[42]},
			{"accessory43", userGameData.accessories[43]},
			{"accessory44", userGameData.accessories[44]},
			{"accessory45", userGameData.accessories[45]},
			{"accessory46", userGameData.accessories[46]},
			{"accessory47", userGameData.accessories[47]}
		};

		// 첫 번째 매개변수는 뒤끝 콘솔의 "게임 정보 관리" 탭에 생성한 테이블 이름 
		Backend.GameData.Insert("USER_DATA", param, callback =>
		{
			// 게임 정보 추가에 성공했을 때 
			if (callback.IsSuccess())
			{
				// 게임 정보의 고유값 
				gameDataRowInDate = callback.GetInDate();

				Debug.Log($"게임 정보 데이터 삽입에 성공했습니다. : {callback}");
				_isLoadFinish = true;
			}
			// 실패했을 때 
			else
			{
				Debug.LogError($"게임 정보 데이터 삽입에 실패했습니다. : {callback}");
			}
		});
	}

	/// <summary>
	/// 뒤끝 콘솔 테이블에서 유저 정보를 불러올 때 호출 
	/// </summary>
	public void GameDataLoad()
	{
		Backend.GameData.GetMyData("USER_DATA", new Where(), callback =>
		{
			// 게임 정보 불러오기에 성공했을 때 
			if (callback.IsSuccess())
			{
				Debug.Log($"게임 정보 데이터 불러오기에 성공했습니다. : {callback}");

				// JSON 데이터 파싱 성공 
				try
				{
					LitJson.JsonData gameDataJson = callback.FlattenRows();


					//받아온 데이터의 개수가 0이면 데이터가 없는 것 
					if (gameDataJson.Count <= 0)
					{
						Debug.LogWarning("데이터가 존재하지 않습니다.");
					}
					else
					{
						// 불러온 게임 정보의 고유값 
						gameDataRowInDate = gameDataJson[0]["inDate"].ToString();
						// 불러온 게임 정보를  userData 변수에 저장 
						userGameData.infiniteLevel = int.Parse(gameDataJson[0]["infiniteLevel"].ToString());
						userGameData.tutorialLevel = int.Parse(gameDataJson[0]["tutorialLevel"].ToString());

						userGameData.gold = int.Parse(gameDataJson[0]["gold"].ToString());
						userGameData.jewel = int.Parse(gameDataJson[0]["jewel"].ToString());
						userGameData.heart = int.Parse(gameDataJson[0]["heart"].ToString());

						userGameData.faceTicket = int.Parse(gameDataJson[0]["faceTicket"].ToString());
						userGameData.accessoryTicket = int.Parse(gameDataJson[0]["accessoryTicket"].ToString());

						userGameData.lastday = int.Parse(gameDataJson[0]["lastday"].ToString());
						userGameData.missionPlay = int.Parse(gameDataJson[0]["missionPlay"].ToString());
						userGameData.missionSafeTime = int.Parse(gameDataJson[0]["missionSafeTime"].ToString());
						userGameData.missionGoGacha = int.Parse(gameDataJson[0]["missionGoGacha"].ToString());
						userGameData.missionFirePass = int.Parse(gameDataJson[0]["missionFirePass"].ToString());
						userGameData.missionElectronicPass = int.Parse(gameDataJson[0]["missionElectronicPass"].ToString());
						userGameData.missionGetItem = int.Parse(gameDataJson[0]["missionGetItem"].ToString());
						userGameData.missionSawBladePass = int.Parse(gameDataJson[0]["missionSawBladePass"].ToString());
						userGameData.missionWatchAD = int.Parse(gameDataJson[0]["missionWatchAD"].ToString());
						userGameData.missionNormalScore = int.Parse(gameDataJson[0]["missionNormalScore"].ToString());

						userGameData.itemLevel[0] = int.Parse(gameDataJson[0]["crystalScore"].ToString());
						userGameData.itemLevel[1] = int.Parse(gameDataJson[0]["shieldTime"].ToString());
						userGameData.itemLevel[2] = int.Parse(gameDataJson[0]["magnetTime"].ToString());
						userGameData.itemLevel[3] = int.Parse(gameDataJson[0]["timeControlTime"].ToString());

						userGameData.successMission[0] = bool.Parse(gameDataJson[0]["successMission0"].ToString());
						userGameData.successMission[1] = bool.Parse(gameDataJson[0]["successMission1"].ToString());
						userGameData.successMission[2] = bool.Parse(gameDataJson[0]["successMission2"].ToString());
						userGameData.successMissionGacha = int.Parse(gameDataJson[0]["successMissionGacha"].ToString());

						userGameData.useCrystal = int.Parse(gameDataJson[0]["useCrystal"].ToString());
						userGameData.gameCount = int.Parse(gameDataJson[0]["gameCount"].ToString());
						userGameData.itemCount = int.Parse(gameDataJson[0]["itemCount"].ToString());
						userGameData.experience = int.Parse(gameDataJson[0]["experience"].ToString());

						userGameData.normalBestScore = int.Parse(gameDataJson[0]["normalBestScore"].ToString());
						userGameData.fastBestScore = int.Parse(gameDataJson[0]["fastBestScore"].ToString());
						userGameData.hardBestScore = int.Parse(gameDataJson[0]["hardBestScore"].ToString());

						userGameData.isVibrateOn = bool.Parse(gameDataJson[0]["isVibrateOn"].ToString());
						userGameData.isBGMOn = bool.Parse(gameDataJson[0]["isBGMOn"].ToString());
						userGameData.isSFXOn = bool.Parse(gameDataJson[0]["isSFXOn"].ToString());
						userGameData.selectMode = int.Parse(gameDataJson[0]["selectMode"].ToString());
						userGameData.selectInfiniteLevel = int.Parse(gameDataJson[0]["selectInfiniteLevel"].ToString());
						userGameData.selectTutorialLevel = int.Parse(gameDataJson[0]["selectTutorialLevel"].ToString());
						userGameData.selectFace = int.Parse(gameDataJson[0]["selectFace"].ToString());
						userGameData.selectBody = int.Parse(gameDataJson[0]["selectBody"].ToString());
						userGameData.selectAccessory = int.Parse(gameDataJson[0]["selectAccessory"].ToString());

						userGameData.stone = int.Parse(gameDataJson[0]["Stone"].ToString());
						userGameData.copper = int.Parse(gameDataJson[0]["Copper"].ToString());
						userGameData.silver = int.Parse(gameDataJson[0]["Silver"].ToString());
						userGameData.realgold = int.Parse(gameDataJson[0]["Realgold"].ToString());
						userGameData.ruby = int.Parse(gameDataJson[0]["Ruby"].ToString());
						userGameData.sapphire = int.Parse(gameDataJson[0]["Sapphire"].ToString());
						userGameData.emerald = int.Parse(gameDataJson[0]["Emerald"].ToString());
						userGameData.diamond = int.Parse(gameDataJson[0]["Diamond"].ToString());
						userGameData.metal = int.Parse(gameDataJson[0]["Metal"].ToString());
						userGameData.crystal = int.Parse(gameDataJson[0]["Crystal"].ToString());

						for (int i = 0; i < userGameData.faces.Length; i++)
						{
							userGameData.faces[i] = bool.Parse(gameDataJson[0][string.Format("face{0}", i)].ToString());
						}
						for (int i = 0; i < userGameData.accessories.Length; i++)
						{
							userGameData.accessories[i] = bool.Parse(gameDataJson[0][string.Format("accessory{0}", i)].ToString());
						}
						for (int i = 0; i < 5; i++)
						{
							userGameData.todayMission[i] = int.Parse(gameDataJson[0][string.Format("todayMission{0}", i)].ToString());
						}

						onGameDataLoadEvent?.Invoke();

					}
					_isLoadFinish = true;
				}
				// JSON 데이터 파싱 실패 
				catch (System.Exception e)
				{
					// 유저 정보를 초기값으로 설정 
					userGameData.Reset();
					// try-catch 에러 출력 
					Debug.LogError(e);
				}
			}
			// 실패했을 때 
			else
			{
				Debug.LogError($"게임 정보 데이터 불러오기에 실패했습니다. : {callback}");
			}
		});
	}

	/// <summary>
	/// 뒤끝 콘솔 테이블에 있는 유저 데이터 갱신 
	/// </summary>
	/// <param name="action"></param>
	public void GameDataUpdate(UnityAction action = null)
	{
		if (userGameData == null)
		{
			Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다." +
						   "Insert 혹은 Load를 통해 데이터를 생성해주세요.");
			return;
		}

		Param param = new Param()
		{
			{ "tutorialLevel",      userGameData.tutorialLevel },
			{ "infiniteLevel",      userGameData.infiniteLevel },

			{ "gold",       userGameData.gold },
			{ "jewel",      userGameData.jewel },
			{ "heart",      userGameData.heart },

			{ "lastday",       userGameData.lastday },
			{ "missionPlay", userGameData.missionPlay },
			{ "missionGetItem", userGameData.missionGetItem },
			{ "missionNormalScore", userGameData.missionNormalScore },
			{ "missionFirePass", userGameData.missionFirePass },
			{ "missionElectronicPass", userGameData.missionElectronicPass },
			{ "missionSawBladePass", userGameData.missionSawBladePass },
			{ "missionWatchAD", userGameData.missionWatchAD },
			{ "missionSafeTime", userGameData.missionSafeTime },
			{ "missionGoGacha", userGameData.missionGoGacha },

			{ "todayMission0", userGameData.todayMission[0] },
			{ "todayMission1", userGameData.todayMission[1] },
			{ "todayMission2", userGameData.todayMission[2] },
			{ "todayMission3", userGameData.todayMission[3] },
			{ "todayMission4", userGameData.todayMission[4] },

			{ "successMission0", userGameData.successMission[0] },
			{ "successMission1", userGameData.successMission[1] },
			{ "successMission2", userGameData.successMission[2] },
			{ "successMissionGacha", userGameData.successMissionGacha },

			{ "accessoryTicket", userGameData.accessoryTicket},
			{"faceTicket", userGameData.faceTicket},

			{ "itemCount",       userGameData.itemCount },
			{ "gameCount",      userGameData.gameCount },
			{ "useCrystal",      userGameData.useCrystal },
			{ "experience", userGameData.experience},

			{ "normalBestScore", userGameData.normalBestScore },
			{ "fastBestScore", userGameData.fastBestScore },
			{ "hardBestScore", userGameData.hardBestScore },

			{ "isVibrateOn", userGameData.isVibrateOn },
			{ "isBGMOn", userGameData.isBGMOn },
			{ "isSFXOn", userGameData.isSFXOn },
			{ "selectMode", userGameData.selectMode },
			{ "selectInfiniteLevel", userGameData.selectInfiniteLevel },
			{ "selectTutorialLevel", userGameData.selectTutorialLevel },
			{ "selectFace", userGameData.selectFace },
			{ "selectBody", userGameData.selectBody },
			{ "selectAccessory", userGameData.selectAccessory },
			
			{ "crystalScore", userGameData.itemLevel[0]},
			{ "shieldTime", userGameData.itemLevel[1]},
			{ "magnetTime", userGameData.itemLevel[2]},
			{ "timeControlTime", userGameData.itemLevel[3]},

			{ "Stone", userGameData.stone },
			{ "Copper", userGameData.copper },
			{ "Silver", userGameData.silver },
			{ "Realgold", userGameData.realgold },
			{ "Ruby", userGameData.ruby },
			{ "Sapphire", userGameData.sapphire },
			{ "Emerald", userGameData.emerald },
			{ "Diamond", userGameData.diamond },
			{ "Metal", userGameData.metal },
			{ "Crystal", userGameData.crystal },

			{ "face0", userGameData.faces[0] },
			{ "face1", userGameData.faces[1] },
			{ "face2", userGameData.faces[2] },
			{ "face3", userGameData.faces[3] },
			{ "face4", userGameData.faces[4] },
			{ "face5", userGameData.faces[5] },
			{ "face6", userGameData.faces[6] },
			{ "face7", userGameData.faces[7] },
			{ "face8", userGameData.faces[8] },
			{ "face9", userGameData.faces[9] },
			{ "face10", userGameData.faces[10] },
			{ "face11", userGameData.faces[11] },
			{ "face12", userGameData.faces[12] },
			{ "face13", userGameData.faces[13] },
			{ "face14", userGameData.faces[14] },
			{ "face15", userGameData.faces[15] },
			{ "face16", userGameData.faces[16] },
			{ "face17", userGameData.faces[17] },
			{ "face18", userGameData.faces[18] },
			{ "face19", userGameData.faces[19] },
			{ "face20", userGameData.faces[20] },
			{ "face21", userGameData.faces[21] },
			{ "face22", userGameData.faces[22] },
			{ "face23", userGameData.faces[23] },
			{ "face24", userGameData.faces[24] },
			{ "face25", userGameData.faces[25] },
			{ "face26", userGameData.faces[26] },
			{ "face27", userGameData.faces[27] },
			{ "face28", userGameData.faces[28] },
			{ "face29", userGameData.faces[29] },
			{ "face30", userGameData.faces[30] },

			{"accessory0", userGameData.accessories[0]},
			{"accessory1", userGameData.accessories[1]},
			{"accessory2", userGameData.accessories[2]},
			{"accessory3", userGameData.accessories[3]},
			{"accessory4", userGameData.accessories[4]},
			{"accessory5", userGameData.accessories[5]},
			{"accessory6", userGameData.accessories[6]},
			{"accessory7", userGameData.accessories[7]},
			{"accessory8", userGameData.accessories[8]},
			{"accessory9", userGameData.accessories[9]},
			{"accessory10", userGameData.accessories[10]},
			{"accessory11", userGameData.accessories[11]},
			{"accessory12", userGameData.accessories[12]},
			{"accessory13", userGameData.accessories[13]},
			{"accessory14", userGameData.accessories[14]},
			{"accessory15", userGameData.accessories[15]},
			{"accessory16", userGameData.accessories[16]},
			{"accessory17", userGameData.accessories[17]},
			{"accessory18", userGameData.accessories[18]},
			{"accessory19", userGameData.accessories[19]},
			{"accessory20", userGameData.accessories[20]},
			{"accessory21", userGameData.accessories[21]},
			{"accessory22", userGameData.accessories[22]},
			{"accessory23", userGameData.accessories[23]},
			{"accessory24", userGameData.accessories[24]},
			{"accessory25", userGameData.accessories[25]},
			{"accessory26", userGameData.accessories[26]},
			{"accessory27", userGameData.accessories[27]},
			{"accessory28", userGameData.accessories[28]},
			{"accessory29", userGameData.accessories[29]},
			{"accessory30", userGameData.accessories[30]},
			{"accessory31", userGameData.accessories[31]},
			{"accessory32", userGameData.accessories[32]},
			{"accessory33", userGameData.accessories[33]},
			{"accessory34", userGameData.accessories[34]},
			{"accessory35", userGameData.accessories[35]},
			{"accessory36", userGameData.accessories[36]},
			{"accessory37", userGameData.accessories[37]},
			{"accessory38", userGameData.accessories[38]},
			{"accessory39", userGameData.accessories[39]},
			{"accessory40", userGameData.accessories[40]},
			{"accessory41", userGameData.accessories[41]},
			{"accessory42", userGameData.accessories[42]},
			{"accessory43", userGameData.accessories[43]},
			{"accessory44", userGameData.accessories[44]},
			{"accessory45", userGameData.accessories[45]},
			{"accessory46", userGameData.accessories[46]},
			{"accessory47", userGameData.accessories[47]}
		};

		// 게임 정보의 고유값(gameDataRowInDate)이 없으면 에러 메시지 출력 
		if (string.IsNullOrEmpty(gameDataRowInDate))
		{
			Debug.LogError($"유저의 inDate 정보가 없어 게임 정보 데이터 수정에 실패했습니다.");
		}
		// 게임 정보의 고유값이 있으면 테이블에 저장되어 있는 값 중 inDate 컬럼의 값과
		// 소유하는 유저의 owner_inDate가 일치하는 row를 검색하여 수정하는 UpdateV2() 호출
		else
		{
			Debug.Log($"{gameDataRowInDate}의 게임 정보 데이터 수정을 요청합니다.");

			Backend.GameData.UpdateV2("USER_DATA", gameDataRowInDate, Backend.UserInDate, param, callback =>
			{
				if (callback.IsSuccess())
				{
					Debug.Log($"게임 정보 데이터 수정에 성공했습니다. : {callback}");

					action?.Invoke();
				}
				else
				{
					Debug.LogError($"게임 정보 데이터 수정에 실패했습니다. : {callback}");
				}
			});
		}
	}

	public int UnlockCharacterNumber()
	{
		if (UserGameData.ruby <= 1) return 0;

		int cnt = 0;
		if (UserGameData.copper <= 1) cnt++;
		if (UserGameData.silver <= 1) cnt++;
		if (UserGameData.realgold <= 1) cnt++;
		if (UserGameData.emerald <= 1) cnt++;
		if (UserGameData.metal <= 1) cnt++;
		if (UserGameData.diamond <= 1) cnt++;
		if (UserGameData.sapphire <= 1) cnt++;
		if (UserGameData.crystal <= 1) cnt++;

		return cnt;
	}
	public void SetBodyState(int myPick, int state)
	{
		switch ((eCharacter)myPick)
		{
			case eCharacter.Stone:
				UserGameData.stone = state;
				break;
			case eCharacter.Copper:
				UserGameData.copper = state;
				break;
			case eCharacter.Silver:
				UserGameData.silver = state;
				break;
			case eCharacter.RealGold:
				UserGameData.realgold = state;
				break;
			case eCharacter.Emerald:
				UserGameData.emerald = state;
				break;
			case eCharacter.Diamond:
				UserGameData.diamond = state;
				break;
			case eCharacter.Metal:
				UserGameData.metal = state;
				break;
			case eCharacter.Ruby:
				UserGameData.ruby = state;
				break;
			case eCharacter.Sapphire:
				UserGameData.sapphire = state;
				break;
			case eCharacter.Crystal:
				UserGameData.crystal = state;
				break;
		}
	}
	public int StateChoiceCharacter(int myPick)
	{
		switch ((eCharacter)myPick)
		{
			case eCharacter.Stone:
				return UserGameData.stone;
			case eCharacter.Copper:
				return UserGameData.copper;
			case eCharacter.Silver:
				return UserGameData.silver;
			case eCharacter.RealGold:
				return UserGameData.realgold;
			case eCharacter.Emerald:
				return UserGameData.emerald;
			case eCharacter.Diamond:
				return UserGameData.diamond;
			case eCharacter.Metal:
				return UserGameData.metal;
			case eCharacter.Ruby:
				return UserGameData.ruby;
			case eCharacter.Sapphire:
				return UserGameData.sapphire;
			case eCharacter.Crystal:
				return UserGameData.crystal;
		}

		return -1;
	}
	public bool StateChoiceFace(int myPick)
	{
		return userGameData.faces[myPick];
	}
	public bool StateChoiceAccessory(int myPick)
	{
		return userGameData.accessories[myPick];
	}

	public void DoingMission(eMissionType mission, int num = 0)
	{
		switch (mission)
		{
			case eMissionType.PlayGame:
				UserGameData.missionPlay++;
				break;
			case eMissionType.GetItem:
				UserGameData.missionGetItem = num;
				break;
			case eMissionType.NormalScore:
				UserGameData.missionNormalScore = num;
				break;
			case eMissionType.FirePass:
				UserGameData.missionFirePass = num;
				break;
			case eMissionType.ElectronicPass:
				UserGameData.missionElectronicPass = num;
				break;
			case eMissionType.SawBladePass:
				UserGameData.missionSawBladePass = num;
				break;
			case eMissionType.SafeTime:
				UserGameData.missionSafeTime = num;
				break;
			case eMissionType.WatchAD:
				if(UserGameData.missionWatchAD < 3)
					UserGameData.missionWatchAD++;
				break;
			case eMissionType.GoGacha:
				if (UserGameData.missionGoGacha < 3)
					UserGameData.missionGoGacha++;
				break;
		}
	}
	public void MissionReset(int day)
	{
		UserGameData.lastday = day;
		UserGameData.missionPlay = 0;
		UserGameData.missionGetItem = 0;
		UserGameData.missionNormalScore = 0;
		UserGameData.missionFirePass = 0;
		UserGameData.missionElectronicPass = 0;
		UserGameData.missionSawBladePass = 0;
		UserGameData.missionSafeTime = 0;
		UserGameData.missionWatchAD = 0;
		UserGameData.missionGoGacha = 0;
		UserGameData.successMissionGacha = 0;

		List<int> _selectedNumbers = new List<int>();

		while (_selectedNumbers.Count < 3)
		{
			int num = Random.Range(0, 7);
			if (!_selectedNumbers.Contains(num))
			{
				_selectedNumbers.Add(num);
			}
		}

		for (int i = 0; i < _selectedNumbers.Count; i++)
		{
			int index = _selectedNumbers[i] * 3 + i + 1;
			UserGameData.todayMission[i] = index;
			UserGameData.successMission[i] = false;
		}

		UserGameData.todayMission[3] = 22;
		UserGameData.todayMission[4] = 23;

		GameDataUpdate();
	}
}

