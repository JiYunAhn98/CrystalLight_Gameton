using UnityEngine;
using BackEnd;
using LitJson;

public class UserInfo : MonoBehaviour
{
	[System.Serializable]
	public class UserInfoEvent : UnityEngine.Events.UnityEvent { }
	public UserInfoEvent onUserInfoEvent = new UserInfoEvent();

	private static UserInfoData data = new UserInfoData();
	public static UserInfoData Data => data;
	public bool _isFinish = false;

	public void GetUserInfoFromBackend()
	{
		// 현재 로그인한 사용자 정보 불러오기 
		// https://developer.thebackend.io/unity3d/guide/bmember/userInfo/
		Backend.BMember.GetUserInfo(callback =>
		{
			// 정보 불러오기 성공 
			if (callback.IsSuccess())
			{
				// JSON 데이터 파싱 성공 
				try
				{
					JsonData json = callback.GetReturnValuetoJSON()["row"];

					data.gamerId = json["gamerId"].ToString();
					data.countryCode = json["countryCode"]?.ToString();
					data.nickname = json["nickname"]?.ToString();
					data.inDate = json["inDate"].ToString();
					data.emailForFindPassword = json["emailForFindPassword"]?.ToString();
					data.subscriptionType = json["subscriptionType"].ToString();
					data.federationId = json["federationId"]?.ToString();
				}
				// JSON 데이터 파싱 성공 
				catch (System.Exception e)
				{
					// 유저 정보를 기본 상태로 설정 
					data.Reset();
					// try-catch 에러 출유
					Debug.LogError(e);
				}
				_isFinish = true;
			}
			// 정보 불러오기 실패 
			else
			{
				// 유저 정보를 기본으로 설정 
				// Tip. 일반적으로 오프라인 상태를 대비해 기본적인 정보를 저장해두고 오프라인일 때 불러와서 사용 
				data.Reset();
				Debug.LogError(callback.GetMessage());
			}

			// 유저 정보 불러오기가 완료되었을 때 onUserInfoEvenet에 등록되어 있는 이벤트 메소드 호출 
			onUserInfoEvent?.Invoke();
		});
	}
}

public class UserInfoData
{
	public string gamerId;              // 유저의 gamerID
	public string countryCode;          // 국가코드 , 설정 안했으면 null
	public string nickname;             // 닉네. 설정 안했으면  null
	public string inDate;                   // 유저의 inDate
	public string emailForFindPassword; // 이메일주, 설정 안했으면 null
	public string subscriptionType;     // 커스, 페더레이션 타입
	public string federationId;         // 구글, 애플 , 페이스북 페더레이션 아이. 커스텀 계정은  null

	public void Reset()
	{
		gamerId = "Offline";
		countryCode = "Unknown";
		nickname = "Noname";
		inDate = string.Empty;
		emailForFindPassword = string.Empty;
		subscriptionType = string.Empty;
		federationId = string.Empty;
	}
}

