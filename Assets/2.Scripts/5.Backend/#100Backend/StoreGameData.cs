[System.Serializable]
public class StoreGameData
{
    public int package4BuyNum;
    public int package5BuyNum;

    public bool crystal_1000BuyNotYet;
    public bool crystal_1500BuyNotYet;
    public bool crystal_3000BuyNotYet;
    public bool crystal_4000BuyNotYet;
    public bool crystal_5000BuyNotYet;

    public bool eraseAdNotYet;
    public bool battlepass;
    public int freePassCnt;     // 무료패스 획득한 레벨
    public int OrePassCnt;     // 무료패스를 얻은 양

    public void Reset()
    {
        package4BuyNum = 2;
        package5BuyNum = 2;

        crystal_1000BuyNotYet = true;
        crystal_1500BuyNotYet = true;
        crystal_3000BuyNotYet = true;
        crystal_4000BuyNotYet = true;
        crystal_5000BuyNotYet = true;

        eraseAdNotYet = true;
        battlepass = false;
        freePassCnt = 0;
        OrePassCnt = 0;
    }
}
