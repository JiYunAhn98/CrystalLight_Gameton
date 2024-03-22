using UnityEngine;
using BackEnd;
using UnityEngine.Events;

public class BackendStoreData
{
	//유저 정보를 불러오는데 성공했을 때 호출할 메소드를 등록할 수 있도록 함 
	[System.Serializable]
	public class StoreDataLoadEvent : UnityEngine.Events.UnityEvent { }
	public StoreDataLoadEvent onStoreDataLoadEvent = new StoreDataLoadEvent();

	public static bool _isLoadFinish;
	private static BackendStoreData instance = null;
	public static BackendStoreData Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new BackendStoreData();
			}

			return instance;
		}
	}

	private StoreGameData storeGameData = new StoreGameData();
	public StoreGameData StoreGameData => storeGameData;

	private string StoreRowInDate = string.Empty;

	/// <summary>
	/// 뒤끝 콘솔 테이블에 새로운 유저 정보 추가 , 살 수 있냐 없냐
	/// </summary>
	public void StoreDataInsert()
	{
		// 유저 정보를 초기값으로 설정 
		storeGameData.Reset();

		// 테이블에 추가할 데이터로 가공 
		Param param = new Param()
		{
			{"package4BuyNum", storeGameData.package4BuyNum},
			{"package5BuyNum", storeGameData.package5BuyNum},

			{"crystal_1000BuyNotYet", storeGameData.crystal_1000BuyNotYet},
			{"crystal_1500BuyNotYet", storeGameData.crystal_1500BuyNotYet},
			{"crystal_3000BuyNotYet", storeGameData.crystal_3000BuyNotYet},
			{"crystal_4000BuyNotYet", storeGameData.crystal_4000BuyNotYet},
			{"crystal_5000BuyNotYet", storeGameData.crystal_5000BuyNotYet},

			{"eraseAdNotYet", storeGameData.eraseAdNotYet},
			{"battlepass", storeGameData.battlepass},
			{"freePassCnt", storeGameData.freePassCnt},
			{"OrePassCnt", storeGameData.OrePassCnt}
		};

		// 첫 번째 매개변수는 뒤끝 콘솔의 "게임 정보 관리" 탭에 생성한 테이블 이름 
		Backend.GameData.Insert("STORE_DATA", param, callback =>
		{
			// 게임 정보 추가에 성공했을 때 
			if (callback.IsSuccess())
			{
				// 게임 정보의 고유값 
				StoreRowInDate = callback.GetInDate();

				Debug.Log($"상점 정보 데이터 삽입에 성공했습니다. : {callback}");
				_isLoadFinish = true;
			}
			// 실패했을 때 
			else
			{
				Debug.LogError($"상점 정보 데이터 삽입에 실패했습니다. : {callback}");
			}
		});
	}

	/// <summary>
	/// 뒤끝 콘솔 테이블에서 유저 정보를 불러올 때 호출 
	/// </summary>
	public void StoreDataLoad()
	{
		Backend.GameData.GetMyData("STORE_DATA", new Where(), callback =>
		{
			// 게임 정보 불러오기에 성공했을 때 
			if (callback.IsSuccess())
			{
				Debug.Log($"게임 정보 데이터 불러오기에 성공했습니다. : {callback}");

				// JSON 데이터 파싱 성공 
				try
				{
					LitJson.JsonData storeDataJson = callback.FlattenRows();


					//받아온 데이터의 개수가 0이면 데이터가 없는 것 
					if (storeDataJson.Count <= 0)
					{
						Debug.LogWarning("데이터가 존재하지 않습니다.");
					}
					else
					{
						// 불러온 게임 정보의 고유값 
						StoreRowInDate = storeDataJson[0]["inDate"].ToString();

						// 불러온 게임 정보를  userData 변수에 저장 
						storeGameData.package4BuyNum = int.Parse(storeDataJson[0]["package4BuyNum"].ToString());
						storeGameData.package5BuyNum = int.Parse(storeDataJson[0]["package4BuyNum"].ToString());

						storeGameData.crystal_1000BuyNotYet = bool.Parse(storeDataJson[0]["crystal_1000BuyNotYet"].ToString());
						storeGameData.crystal_1500BuyNotYet = bool.Parse(storeDataJson[0]["crystal_1500BuyNotYet"].ToString());
						storeGameData.crystal_3000BuyNotYet = bool.Parse(storeDataJson[0]["crystal_3000BuyNotYet"].ToString());
						storeGameData.crystal_4000BuyNotYet = bool.Parse(storeDataJson[0]["crystal_4000BuyNotYet"].ToString());
						storeGameData.crystal_5000BuyNotYet = bool.Parse(storeDataJson[0]["crystal_5000BuyNotYet"].ToString());

						storeGameData.eraseAdNotYet = bool.Parse(storeDataJson[0]["eraseAdNotYet"].ToString());
						storeGameData.battlepass = bool.Parse(storeDataJson[0]["battlepass"].ToString());
						storeGameData.freePassCnt = int.Parse(storeDataJson[0]["freePassCnt"].ToString());
						storeGameData.OrePassCnt = int.Parse(storeDataJson[0]["OrePassCnt"].ToString());

						onStoreDataLoadEvent?.Invoke();

					}
					_isLoadFinish = true;
				}
				// JSON 데이터 파싱 실패 
				catch (System.Exception e)
				{
					// 유저 정보를 초기값으로 설정 
					storeGameData.Reset();
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
	public void StoreDataUpdate(UnityAction action = null)
	{
		if (storeGameData == null)
		{
			Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다." +
						   "Insert 혹은 Load를 통해 데이터를 생성해주세요.");
			return;
		}

		Param param = new Param()
		{
			{"package4BuyNum", storeGameData.package4BuyNum},
			{"package5BuyNum", storeGameData.package5BuyNum},

			{"crystal_1000BuyNotYet", storeGameData.crystal_1000BuyNotYet},
			{"crystal_1500BuyNotYet", storeGameData.crystal_1500BuyNotYet},
			{"crystal_3000BuyNotYet", storeGameData.crystal_3000BuyNotYet},
			{"crystal_4000BuyNotYet", storeGameData.crystal_4000BuyNotYet},
			{"crystal_5000BuyNotYet", storeGameData.crystal_5000BuyNotYet},

			{"eraseAdNotYet", storeGameData.eraseAdNotYet},
			{"battlepass", storeGameData.battlepass},
			{"freePassCnt", storeGameData.freePassCnt},
			{"OrePassCnt", storeGameData.OrePassCnt}
		};

		// 게임 정보의 고유값(gameDataRowInDate)이 없으면 에러 메시지 출력 
		if (string.IsNullOrEmpty(StoreRowInDate))
		{
			Debug.LogError($"유저의 inDate 정보가 없어 게임 정보 데이터 수정에 실패했습니다.");
		}
		// 게임 정보의 고유값이 있으면 테이블에 저장되어 있는 값 중 inDate 컬럼의 값과
		// 소유하는 유저의 owner_inDate가 일치하는 row를 검색하여 수정하는 UpdateV2() 호출
		else
		{
			Debug.Log($"{StoreRowInDate}의 게임 정보 데이터 수정을 요청합니다.");

			Backend.GameData.UpdateV2("STORE_DATA", StoreRowInDate, Backend.UserInDate, param, callback =>
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
}