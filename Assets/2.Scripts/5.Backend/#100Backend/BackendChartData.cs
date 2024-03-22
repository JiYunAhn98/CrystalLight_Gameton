using System.Collections.Generic;
using UnityEngine;
using BackEnd;

public static class BackendChartData
{

	//레벨별 레벨업 필요 경험치와 보상 정보 
	public static List<LevelChartData> levelChart;
	public static List<CharacterChartData> characterChart;
	//public static List<FaceChartData> faceChart;
	public static List<QuestChartData> questChart;

	static int _finishLoad;

	public static bool _isLoadFinish
	{
		get { return _finishLoad >= 2; }
	}

	static BackendChartData()
	{
		_finishLoad = 0;
		levelChart = new List<LevelChartData>();
		characterChart = new List<CharacterChartData>();
		//faceChart = new List<FaceChartData>();
		//questChart = new List<QuestChartData>();
	}

	public static void LoadAllChart()
	{
		LoadLevelChart();
		LoadCharacterChart();
		//LoadQuestChart();
	}

	// 아마 안 씀
	public static void LoadLevelChart()
	{
		Backend.Chart.GetChartContents(Constants.LEVEL_CHART, callback =>
		{
			if (callback.IsSuccess())
			{
				// JSON 데이터 파싱 성공 
				try
				{
					LitJson.JsonData jsonData = callback.FlattenRows();

					//받아온 데이터의 개수가 0이면 데이터가 없는 것 
					if (jsonData.Count <= 0)
					{
						Debug.LogWarning("데이터가 존재하지 않습니다.");
					}
					else
					{
						for (int i = 0; i < jsonData.Count; ++i)
						{
							LevelChartData newChart = new LevelChartData();
							newChart.level = int.Parse(jsonData[i]["level"].ToString());
							newChart.maxExperience = int.Parse(jsonData[i]["maxExperience"].ToString());
							newChart.rewardGold = int.Parse(jsonData[i]["rewardGold"].ToString());

							levelChart.Add(newChart);
						}
					}
					_finishLoad++;
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
				Debug.LogError($"{Constants.LEVEL_CHART}의 차트 불러오기에 에러 발생 : {callback}");
			}
		});
	}

	public static void LoadCharacterChart()
	{
		Backend.Chart.GetChartContents(Constants.CHARACTER_CHART, callback =>
		{
			if (callback.IsSuccess())
			{
				try
				{
					LitJson.JsonData jsonData = callback.FlattenRows();

					if (jsonData.Count <= 0)
					{
						Debug.LogWarning("데이터가 존재하지 않습니다.");
					}
					else
					{
						for (int i = 0; i < jsonData.Count; ++i)
						{
							CharacterChartData newChart = new CharacterChartData();
							newChart.name = jsonData[i]["name"].ToString();
							newChart.price = int.Parse(jsonData[i]["price"].ToString());
							newChart.condition = int.Parse(jsonData[i]["condition"].ToString());

							characterChart.Add(newChart);

							Debug.Log($"name : {newChart.name}, price : {newChart.price}," +
									  $"condition : {newChart.condition}");
						}
					}
					_finishLoad++;
				}
				catch (System.Exception e)
				{
					// try-catch 에러 출력 
					Debug.LogError(e);
				}
			}
			else
			{
				Debug.LogError($"{Constants.CHARACTER_CHART}의 차트 불러오기에 에러 발생 : {callback}");
			}
		});
	}
	//public static void LoadFaceChart()
	//{
	//	Backend.Chart.GetChartContents(Constants.FACE_CHART, callback =>
	//	{
	//		if (callback.IsSuccess())
	//		{
	//			try
	//			{
	//				LitJson.JsonData jsonData = callback.FlattenRows();

	//				if (jsonData.Count <= 0)
	//				{
	//					Debug.LogWarning("데이터가 존재하지 않습니다.");
	//				}
	//				else
	//				{
	//					for (int i = 0; i < jsonData.Count; ++i)
	//					{
	//						CharacterChartData newChart = new CharacterChartData();
	//						newChart.name = jsonData[i]["name"].ToString();
	//						newChart.price = int.Parse(jsonData[i]["price"].ToString());
	//						newChart.condition = int.Parse(jsonData[i]["condition"].ToString());

	//						characterChart.Add(newChart);

	//						Debug.Log($"name : {newChart.name}, price : {newChart.price}," +  $"condition : {newChart.condition}");
	//					}
	//				}
	//				_finishLoad++;
	//			}
	//			catch (System.Exception e)
	//			{
	//				// try-catch 에러 출력 
	//				Debug.LogError(e);
	//			}
	//		}
	//		else
	//		{
	//			Debug.LogError($"{Constants.FACE_CHART}의 차트 불러오기에 에러 발생 : {callback}");
	//		}
	//	});
	//}

	public static void LoadQuestChart()
	{
		Backend.Chart.GetChartContents(Constants.QUEST_CHART, callback =>
		{
			if (callback.IsSuccess())
			{
				try
				{
					LitJson.JsonData jsonData = callback.FlattenRows();

					if (jsonData.Count <= 0)
					{
						Debug.LogWarning("데이터가 존재하지 않습니다.");
					}
					else
					{
						for (int i = 0; i < jsonData.Count; ++i)
						{
							QuestChartData newChart = new QuestChartData();
							newChart.itemId = int.Parse(jsonData[i]["itemId"].ToString());
							newChart.itemName = jsonData[i]["itemName"].ToString();
							newChart.itemGold = int.Parse(jsonData[i]["itemGold"].ToString());

							questChart.Add(newChart);

							Debug.Log($"itemId : {newChart.itemId}, itemName : {newChart.itemName}," + $"itemGold : {newChart.itemGold}");
						}
					}
					_finishLoad++;
				}
				catch (System.Exception e)
				{
					// try-catch 에러 출력 
					Debug.LogError(e);
				}
			}
			else
			{
				Debug.LogError($"{Constants.QUEST_CHART}의 차트 불러오기에 에러 발생 : {callback}");
			}
		});
	}



}

[System.Serializable]
public class LevelChartData
{
	public int level;
	public int maxExperience;
	public int rewardGold;

}

[System.Serializable]
public class CharacterChartData
{
	public string name;
	public int price;
	public int condition;

}

[System.Serializable]
public class QuestChartData
{
	public int itemId;
	public string itemName;
	public int itemGold;
}

//[System.Serializable]
//public class FaceChartData
//{
//	public string name;
//	public int price;
//	public int condition;
//}