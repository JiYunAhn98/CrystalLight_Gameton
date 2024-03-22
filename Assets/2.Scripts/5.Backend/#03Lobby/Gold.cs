using System.Collections.Generic;
using System.Text;
using UnityEngine;
using BackEnd;

public class Gold : MonoBehaviour
{

	public void Process(int newGold)
    {
		UpdateMyFinalGoldData(newGold);
    }

    public void GoldInsert(int newGold)
    {
        string inDate = string.Empty;
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
				inDate = callback.FlattenRows()[0]["inDate"].ToString();
			}
			else
			{
				Debug.LogError("데이터가 존재하지 않습니다.");
				return;
			}

			Param param = new Param()
			{
				{ "gold", newGold }
			};

			Backend.GameData.UpdateV2("USER_DATA", inDate, Backend.UserInDate, param, (callback) =>
			{
				if (callback.IsSuccess())
				{
					Debug.Log($"재화 등록에 성공했습니다. : {callback}");
				}
				else
				{
					Debug.LogError($"재화 등록에 실패했습니다. : {callback}");
				}
			});
		});
	}

	public void UpdateMyFinalGoldData(int newGold)
    {
		Backend.GameData.GetMyData(Constants.USER_DATA_TABLE, new Where(), callback =>
		{
			if (callback.IsSuccess())
            {
                try
                {
					LitJson.JsonData goldDataJson = callback.FlattenRows();

					if (goldDataJson.Count <= 0)
                    {
						Debug.LogWarning("데이터가 존재하지 않습니다.");
                    }
                    else
                    {
						//lastGold는 현재 재화 
						int lastGold = int.Parse(goldDataJson[0]["gold"].ToString());
						int finalGold = lastGold + newGold;

						GoldInsert(finalGold);
						Debug.Log("게임 끝난 후, 최종 재화 등록 완료");
					}
                }

				catch (System.Exception e)
                {
					Debug.LogError(e);
                }
            }
            else
            {
				Debug.Log("재화 업데이트 부분에서 콜백 실패함");
            }
		});

	}

	

}
