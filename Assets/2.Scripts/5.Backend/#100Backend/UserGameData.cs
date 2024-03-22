[System.Serializable]
public class UserGameData
{
    public int gold;            // 무료 재화
    public int jewel;           // 유료 재화
    public int heart;           // 게임 플레이에 소모되는 재화

    public int lastday;
    public int missionPlay;             // 임무 게임 플레이 횟수
    public int missionGetItem;          // 임무 아이템 획득 횟수
    public int missionNormalScore;      // 임무 노말모드 점수
    public int missionFirePass;         // 임무 불 장애물 통과
    public int missionElectronicPass;   // 임무 전기 장애물 통과
    public int missionSawBladePass;     // 임무 톱날 장애물 통과
    public int missionSafeTime;       // 임무 생존시간 충족
    public int missionWatchAD;          // 임무 광고보기
    public int missionGoGacha;          // 임무 가챠권 사용

    public int[] todayMission;          // 미션 고른 인덱스 저장

    public int normalBestScore; // 노말 최고 점수 
    public int fastBestScore;   // 패스트 최고 점수
    public int hardBestScore;   // 하드 최고 점수

    public int tutorialLevel;
    public int infiniteLevel;

    public int accessoryTicket;
    public int faceTicket;

    public bool isVibrateOn;
    public bool isBGMOn;
    public bool isSFXOn;

    public int itemCount;
    public int gameCount;
    public int useCrystal;
    public float experience;    // 배틀패스 누적 경험치

    public int selectMode;
    public int selectInfiniteLevel;
    public int selectTutorialLevel;
    public int selectFace;
    public int selectBody;
    public int selectAccessory;

    public int stone;
    public int copper;
    public int silver;
    public int realgold;
    public int ruby;
    public int sapphire;
    public int emerald;
    public int diamond;
    public int metal;
    public int crystal;

    public bool[] faces;
    public bool[] accessories;
    public int[] itemLevel;
    public bool[] successMission;
    public int successMissionGacha;
    public void Initilaize()
    {
        faces = new bool[(int)DefineHelper.eFace.Cnt];
        accessories = new bool[(int)DefineHelper.eAccessory.Cnt];
        itemLevel = new int[(int)DefineHelper.eItem.Cnt];
        todayMission = new int[5];
        successMission = new bool[3];
    }
    /// <summary>
    /// 로그인 하면 저장되는 정보
    /// </summary>
    public void Reset()
    {
        infiniteLevel = 0;
        tutorialLevel = 0;

        gold = 0;
        jewel = 500;
        heart = 0;

        accessoryTicket = 0;
        faceTicket = 0;

        itemCount = 0;
        gameCount = 0;
        useCrystal = 0;
        experience = 0;

        normalBestScore = 0;
        fastBestScore = 0;
        hardBestScore = 0;

        isVibrateOn = true;
        isBGMOn = true;
        isSFXOn = true;
        selectMode = 0;
        selectInfiniteLevel = 0;
        selectTutorialLevel = 0;
        selectFace = 0;
        selectBody = 0;
        selectAccessory = (int)DefineHelper.eAccessory.None;

        stone = 0;
        copper = 2;
        silver = 2;
        realgold = 2;
        ruby = 2;
        sapphire = 2;
        emerald = 2;
        diamond = 2;
        metal = 2;
        crystal = 2;

        lastday = 0;

        missionPlay = 0;
        missionGetItem = 0;
        missionNormalScore = 0;
        missionFirePass = 0;
        missionElectronicPass = 0;
        missionSawBladePass = 0;
        missionSafeTime = 0;
        missionWatchAD = 0;
        missionGoGacha = 0;
        successMissionGacha = 0;

        // 배열로 변경
        faces[0] = true;
        for (int i = 1; i < faces.Length; i++)
        {
            faces[i] = false;
        }
        accessories[0] = true;
        for (int i = 1; i < accessories.Length; i++)
        {
            accessories[i] = false;
        }
        for (int i = 0; i < itemLevel.Length; i++)
        {
            itemLevel[i] = 0;
        }
        for (int i = 0; i < todayMission.Length; i++)
        {
            todayMission[i] = 0;
        }
        for (int i = 0; i < successMission.Length; i++)
        {
            successMission[i] = false;
        }
    }
}