using BackEnd;
using UnityEngine;

public class RankRegister : MonoBehaviour
{
	/// <summary>
	/// Normal
	/// </summary>
	/// <param name="newScore"></param>
	public void NormalProcess(int newScore)
	{
		UpdateMyNormalBestRankData(newScore);
	}

	/// <summary>
	/// Fast
	/// </summary>
	/// <param name="newScore"></param>
	public void FastProcess(int newScore)
	{
		UpdateMyFastBestRankData(newScore);
	}

	/// <summary>
	/// Hard
	/// </summary>
	/// <param name="newScore"></param>
	public void HardProcess(int newScore)
	{
		UpdateMyHardBestRankData(newScore);
	}

	/// <summary>
	/// Normal
	/// </summary>
	/// <param name="newScore"></param>
	private void UpdateMyNormalRankData(int newScore)
	{
		string rowInDate = string.Empty;

		// 랭킹 데이터를 업데이트하려면 겡미 데이터에서 사용하는 데이터의 inDate 값이 필요 
		Backend.GameData.GetMyData(Constants.USER_DATA_TABLE, new Where(), callback =>
		{
			if (!callback.IsSuccess())
			{
				Debug.LogError($"데이터 조회 중 문제가 발생했습니다.: {callback}");
				return;
			}

			Debug.Log($"데이터 조회에 성공했습니다. : {callback}");

			if (callback.FlattenRows().Count > 0)
			{
				rowInDate = callback.FlattenRows()[0]["inDate"].ToString();
			}
			else
			{
				Debug.LogError("데이터가 존재하지 않습니다.");
				return;
			}
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기 dailyBestScore
			Param param = new Param()
			{
				{ "normalBestScore", newScore }
			};
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기 DAILY_RANK_UUID
			Backend.URank.User.UpdateUserScore(Constants.NORMAL_RANK_UUID, Constants.USER_DATA_TABLE, rowInDate, param, callback =>
			{
				if (callback.IsSuccess())
				{
					Debug.Log($"랭킹 등록에 성공했습니다. : {callback}");
				}
				else
				{
					Debug.LogError($"랭킹 등록에 실패했습니다. : {callback}");
				}
			});
		});
	}

	/// <summary>
	/// Fast
	/// </summary>
	/// <param name="newScore"></param>
	private void UpdateMyFastRankData(int newScore)
	{
		string rowInDate = string.Empty;

		// 랭킹 데이터를 업데이트하려면 겡미 데이터에서 사용하는 데이터의 inDate 값이 필요 
		Backend.GameData.GetMyData(Constants.USER_DATA_TABLE, new Where(), callback =>
		{
			if (!callback.IsSuccess())
			{
				Debug.LogError($"데이터 조회 중 문제가 발생했습니다.: {callback}");
				return;
			}

			Debug.Log($"데이터 조회에 성공했습니다. : {callback}");

			if (callback.FlattenRows().Count > 0)
			{
				rowInDate = callback.FlattenRows()[0]["inDate"].ToString();
			}
			else
			{
				Debug.LogError("데이터가 존재하지 않습니다.");
				return;
			}
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기 dailyBestScore
			Param param = new Param()
			{
				{ "fastBestScore", newScore }
			};
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기 DAILY_RANK_UUID
			Backend.URank.User.UpdateUserScore(Constants.FAST_RANK_UUID, Constants.USER_DATA_TABLE, rowInDate, param, callback =>
			{
				if (callback.IsSuccess())
				{
					Debug.Log($"랭킹 등록에 성공했습니다. : {callback}");
				}
				else
				{
					Debug.LogError($"랭킹 등록에 실패했습니다. : {callback}");
				}
			});
		});
	}

	/// <summary>
	/// Hard
	/// </summary>
	/// <param name="newScore"></param>
	private void UpdateMyHardRankData(int newScore)
	{
		string rowInDate = string.Empty;

		// 랭킹 데이터를 업데이트하려면 겡미 데이터에서 사용하는 데이터의 inDate 값이 필요 
		Backend.GameData.GetMyData(Constants.USER_DATA_TABLE, new Where(), callback =>
		{
			if (!callback.IsSuccess())
			{
				Debug.LogError($"데이터 조회 중 문제가 발생했습니다.: {callback}");
				return;
			}

			Debug.Log($"데이터 조회에 성공했습니다. : {callback}");

			if (callback.FlattenRows().Count > 0)
			{
				rowInDate = callback.FlattenRows()[0]["inDate"].ToString();
			}
			else
			{
				Debug.LogError("데이터가 존재하지 않습니다.");
				return;
			}
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기 dailyBestScore
			Param param = new Param()
			{
				{ "hardBestScore", newScore }
			};
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기
			//여기 DAILY_RANK_UUID
			Backend.URank.User.UpdateUserScore(Constants.HARD_RANK_UUID, Constants.USER_DATA_TABLE, rowInDate, param, callback =>
			{
				if (callback.IsSuccess())
				{
					Debug.Log($"랭킹 등록에 성공했습니다. : {callback}");
				}
				else
				{
					Debug.LogError($"랭킹 등록에 실패했습니다. : {callback}");
				}
			});
		});
	}

	/// <summary>
	/// Normal
	/// </summary>
	/// <param name="newScore"></param>
	private void UpdateMyNormalBestRankData(int newScore)
	{
		//여기
		//여기
		//여기
		//여기
		//여기
		//여기
		//여기
		//여기 DAILY_RANK_UUID
		Backend.URank.User.GetMyRank(Constants.NORMAL_RANK_UUID, callback =>
		{
			if (callback.IsSuccess())
			{
				// JSON 데이터 파싱 성공 
				try
				{
					LitJson.JsonData rankDataJson = callback.FlattenRows();

					//받아온 데이터의 개수가 0이면 데이터가 없는 것 
					if (rankDataJson.Count <= 0)
					{
						Debug.LogWarning("데이터가 존재하지 않습니다.");
					}
					else
					{
						// 랭킹을 등록할 때는 컬럼명을 "dailyBestScore"로 저장했지만
						// 랭킹을 불러올 때는 컬럼명이 "score"로 통일되어 있다.

						//추가로 등록한 항목은 컬럼명을 그대로 사용 
						int bestScore = int.Parse(rankDataJson[0]["score"].ToString());

						// 현재 점수가 최고 점수보다 높으면 
						if (newScore > bestScore)
						{
							// 현재점수를 새로운 최고 점수로 설정하고, 랭킹에 등록 
							UpdateMyNormalRankData(newScore);

							Debug.Log($"최고 점수 갱신  {bestScore} -> {newScore}");
						}
					}
				}
				// JSON 데이터 파싱 실패 
				catch (System.Exception e)
				{
					// try-catch 에러 출력 
					Debug.LogError(e);
				}
			}
			else
			{
				// 자신의 랭킹 정보가 존재하지 않을 때는 현재 점수를 새로운 랭킹으로 등록  
				if (callback.GetMessage().Contains("userRank"))
				{
					UpdateMyNormalRankData(newScore);

					Debug.Log($"새로운 랭킹 데이터 생성 및 등록 : {callback}");
				}
			}
		});
	}

	/// <summary>
	/// Fast
	/// </summary>
	/// <param name="newScore"></param>
	private void UpdateMyFastBestRankData(int newScore)
	{
		//여기
		//여기
		//여기
		//여기
		//여기
		//여기
		//여기
		//여기 DAILY_RANK_UUID
		Backend.URank.User.GetMyRank(Constants.FAST_RANK_UUID, callback =>
		{
			if (callback.IsSuccess())
			{
				// JSON 데이터 파싱 성공 
				try
				{
					LitJson.JsonData rankDataJson = callback.FlattenRows();

					//받아온 데이터의 개수가 0이면 데이터가 없는 것 
					if (rankDataJson.Count <= 0)
					{
						Debug.LogWarning("데이터가 존재하지 않습니다.");
					}
					else
					{
						// 랭킹을 등록할 때는 컬럼명을 "dailyBestScore"로 저장했지만
						// 랭킹을 불러올 때는 컬럼명이 "score"로 통일되어 있다.

						//추가로 등록한 항목은 컬럼명을 그대로 사용 
						int bestScore = int.Parse(rankDataJson[0]["score"].ToString());

						// 현재 점수가 최고 점수보다 높으면 
						if (newScore > bestScore)
						{
							// 현재점수를 새로운 최고 점수로 설정하고, 랭킹에 등록 
							UpdateMyFastRankData(newScore);

							Debug.Log($"최고 점수 갱신  {bestScore} -> {newScore}");
						}
					}
				}
				// JSON 데이터 파싱 실패 
				catch (System.Exception e)
				{
					// try-catch 에러 출력 
					Debug.LogError(e);
				}
			}
			else
			{
				// 자신의 랭킹 정보가 존재하지 않을 때는 현재 점수를 새로운 랭킹으로 등록  
				if (callback.GetMessage().Contains("userRank"))
				{
					UpdateMyFastRankData(newScore);

					Debug.Log($"새로운 랭킹 데이터 생성 및 등록 : {callback}");
				}
			}
		});
	}

	/// <summary>
	/// Hard
	/// </summary>
	/// <param name="newScore"></param>
	private void UpdateMyHardBestRankData(int newScore)
	{
		//여기
		//여기
		//여기
		//여기
		//여기
		//여기
		//여기
		//여기 DAILY_RANK_UUID
		Backend.URank.User.GetMyRank(Constants.HARD_RANK_UUID, callback =>
		{
			if (callback.IsSuccess())
			{
				// JSON 데이터 파싱 성공 
				try
				{
					LitJson.JsonData rankDataJson = callback.FlattenRows();

					//받아온 데이터의 개수가 0이면 데이터가 없는 것 
					if (rankDataJson.Count <= 0)
					{
						Debug.LogWarning("데이터가 존재하지 않습니다.");
					}
					else
					{
						// 랭킹을 등록할 때는 컬럼명을 "dailyBestScore"로 저장했지만
						// 랭킹을 불러올 때는 컬럼명이 "score"로 통일되어 있다.

						//추가로 등록한 항목은 컬럼명을 그대로 사용 
						int bestScore = int.Parse(rankDataJson[0]["score"].ToString());

						// 현재 점수가 최고 점수보다 높으면 
						if (newScore > bestScore)
						{
							// 현재점수를 새로운 최고 점수로 설정하고, 랭킹에 등록 
							UpdateMyHardRankData(newScore);

							Debug.Log($"최고 점수 갱신  {bestScore} -> {newScore}");
						}
					}
				}
				// JSON 데이터 파싱 실패 
				catch (System.Exception e)
				{
					// try-catch 에러 출력 
					Debug.LogError(e);
				}
			}
			else
			{
				// 자신의 랭킹 정보가 존재하지 않을 때는 현재 점수를 새로운 랭킹으로 등록  
				if (callback.GetMessage().Contains("userRank"))
				{
					UpdateMyHardRankData(newScore);

					Debug.Log($"새로운 랭킹 데이터 생성 및 등록 : {callback}");
				}
			}
		});
	}
}