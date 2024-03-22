using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using DefineHelper;
using TMPro;

public class RankLoader : MonoBehaviour
{
	//Rank ID 달기 
	[SerializeField] GameObject rankDataPrefab;      // 랭킹 정보 출력을 위한 UI 프리팹 원본
	[SerializeField] Scrollbar scrollbar;            // scrollBar의 value 설정 ( 활성화될 때 1위가 보이도록)
	[SerializeField] Transform rankDataParent;       // ScrollView의 Content 오브젝트
	[SerializeField] RankData myRankData;			// 내 랭킹 정보를 출력하는  UI 게임오브젝트
	[SerializeField] TextMeshProUGUI _rankMode;

	List<RankData> rankDataList;
	int _rankModeIndex;
	bool _change = false;

	public  void Initialize()
	{
		_change = true;
		rankDataList = new List<RankData>();
		_rankModeIndex = (int)eInfiniteLevel.NORMAL;
		_rankMode.text = eInfiniteLevel.NORMAL.ToString();
		// 1 ~ 20위 랭킹 출력을 위한 UI 오브젝트 생성 
		for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
		{
			GameObject clone = Instantiate(rankDataPrefab, rankDataParent);
			rankDataList.Add(clone.GetComponent<RankData>());
		}
	}

	private void Update()
	{
		// 1위 랭킹이 보이도록 scroll 값 설정 
		// 1 ~ 20위의 랭킹 정보 불러오기 
		// 내 랭킹 정보 불러오기 
		if (_change)
		{
			scrollbar.value = 1;
			switch (_rankModeIndex)
			{
				case (int)eInfiniteLevel.NORMAL:
					_rankMode.text = eInfiniteLevel.NORMAL.ToString();
					GetRankList(Constants.NORMAL_RANK_UUID);
					GetMyRank(Constants.NORMAL_RANK_UUID);
					break;
				case (int)eInfiniteLevel.FAST:
					_rankMode.text = eInfiniteLevel.FAST.ToString();
					GetRankList(Constants.FAST_RANK_UUID);
					GetMyRank(Constants.FAST_RANK_UUID);
					break;
				case (int)eInfiniteLevel.HARD:
					_rankMode.text = eInfiniteLevel.HARD.ToString();
					GetRankList(Constants.HARD_RANK_UUID);
					GetMyRank(Constants.HARD_RANK_UUID);
					break;
			}
			_change = false;
		}
	}

	public void RankChangeBtn(int value)
	{
		_change = true;
		if (_rankModeIndex + value < 0) _rankModeIndex = (int)eInfiniteLevel.HARD;
		else _rankModeIndex = (_rankModeIndex + value) % (int)eInfiniteLevel.Cnt;
	}

	/// <summary>
	/// normal
	/// </summary>
	/// <param name="normalRankData"></param>
	/// <param name="normalRank"></param>
	/// <param name="normalNickname"></param>
	/// <param name="normalScore"></param>
	private void SetRankData(RankData rankData, string rank, string nickname, string score)
	{
		rankData.Rank = rank;
		rankData.Nickname = nickname;
		rankData.Score = score;
	}

	private void GetRankList(string uuid)
	{
		// 1 ~ 20위 랭킹 정보 불러오기
		Backend.URank.User.GetRankList(uuid, Constants.MAX_RANK_LIST, callback =>
		{
			if (callback.IsSuccess())

			{
				// JSON 데이터 파싱 성공 
				try
				{
					Debug.Log($"랭킹 조회에 성공했습니다. : {callback}");

					LitJson.JsonData rankDataJson = callback.FlattenRows();

					// 받아온 데이터의 개수가 0이면 데이터가 없는 것 
					if (rankDataJson.Count <= 0)
					{
						// 1 ~ 20위까지 데이터를 빈 데이터로 설정 
						for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
						{
							SetRankData(rankDataList[i], "-", "-", "-");
						}

						Debug.LogWarning("데이터가 존재하지 않습니다.");
					}
					else
					{
						int rankerCount = rankDataJson.Count;

						// 랭킹 정보를 불러와 출력할 수 있도록 설정 
						for (int i = 0; i < rankerCount; ++i)
						{
							rankDataList[i].Rank = rankDataJson[i]["rank"].ToString();
							rankDataList[i].Score = rankDataJson[i]["score"].ToString();

							// 닉네임은 별도로 설정하지 않은 유저도 존재할 수 있기 때문에 
							// 닉네임이 존재하지 않는 유저는 닉네임 대신 게임아이디로 출력 
							rankDataList[i].Nickname = rankDataJson[i].ContainsKey("nickname") == true ?
													   rankDataJson[i]["nickname"]?.ToString() : "NoName";
						}
						// 만약 rankerCount가 20위까지 존재하지 않으면 나머지 랭킹에는 빈 데이터를 설정 
						for (int i = rankerCount; i < Constants.MAX_RANK_LIST; ++i)
						{
							SetRankData(rankDataList[i], "-", "-", "-");
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
				// 1 ~ 20위까지 데이터를 빈 데이터로 설정 
				for (int i = 0; i < Constants.MAX_RANK_LIST; ++i)
				{
					SetRankData(rankDataList[i], "-", "-", "-");
				}

				Debug.LogError($"랭킹 조회 중 오류가 발생했습니다. : {callback}");
			}
		});
	}
	/// <summary>
	/// normal
	/// </summary>
	private void GetMyRank(string uuid)
	{
		// 내 랭킹 정보 불러오기 
		Backend.URank.User.GetMyRank(uuid, callback =>
		{
			// 닉네임이 없으면 gamerId, 닉네임이 있으면 nickname 사용 
			string nickname = UserInfo.Data.nickname == null ? UserInfo.Data.gamerId : UserInfo.Data.nickname;

			if (callback.IsSuccess())
			{
				// JSON 데이터 파싱 성공 
				try
				{
					LitJson.JsonData rankDataJson = callback.FlattenRows();

					// 받아온 데이터의 개수가 0이면 데이터가 없는 것 
					if (rankDataJson.Count <= 0)
					{
						// ["순위에 없음", "닉네임", 0]과 같이 출력 
						SetRankData(myRankData, "-", nickname, "-");

						Debug.LogWarning("데이터가 존재하지 않습니다.");
					}
					else
					{
						myRankData.Rank = rankDataJson[0]["rank"].ToString();
						myRankData.Score = rankDataJson[0]["score"].ToString();

						// 닉네임은 별도로 설정하지 않은 유저도 존재할 수 있기 때문에 
						// 닉네임이 존재하지 않는 유저는 닉네임 대신 gamerId를 출력 
						myRankData.Nickname = rankDataJson[0].ContainsKey("nickname") == true ?
											  rankDataJson[0]["nickname"]?.ToString() : "NoName";
					}
				}

				//자신의 랭킹 정보 JSON 데이터 파싱에 실패했을 때 
				catch (System.Exception e)
				{
					// ["순위에 없음', "닉네임", 0]과 같이 출력 
					SetRankData(myRankData, "-", nickname, "-");

					// try-catch 에러 출력 
					Debug.LogError(e);
				}
			}
			else
			{
				//자신의 랭킹 정보 데이터가 존재하지 않을 때 
				if (callback.GetMessage().Contains("userRank"))
				{
					// ["순위에 없음', "닉네임", 0]과 같이 출력 
					SetRankData(myRankData, "-", nickname, "-");
				}
			}
		});
	}
}