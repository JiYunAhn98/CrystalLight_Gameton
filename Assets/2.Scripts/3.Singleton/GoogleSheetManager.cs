using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetManager : MonoSingleton<GoogleSheetManager>
{
    #region[���� �������� ��Ʈ ����]
    public enum eSheetNameGID
    {
        LevelProbability = 9378419,
        ObstacleType = 1596062660,
        WallType = 1442890449,
        CharacterTable = 1559102169,
        HardMode = 765763708,
        BattlePass = 1772563138,
        FaceTable = 752860082,
        AccessoryTable = 1156575194,
        QuestChart = 300934728
    }
    public enum eSheetIndex
    {
        LevelProbability = 0,
        ObstacleType,
        WallType,
        CharacterTable,
        HardMode,
        BattlePass,
        FaceTable,
        AccessoryTable,
        QuestChart,

        Cnt
    }
    public enum eLevelProbabilityIndex
    {
        TunnelCount = 0,
        Level0,
        Level1,
        Level2,
        Level3,

        Cnt
    }
    public enum eObstacleIndex
    {
        Level = 0,      // ���̵�
        Name,           // Obstacle Resource �̸�
        Material,       // ���� Material Ÿ��
        WallType,       // ���� �ε��� �� �ߵ��� �ۿ�
        ObstacleType,   // Obstacle�� �ε��� �� �ߵ��� �ۿ�
        TunnelType,     // Tunnel�� ���

        Cnt
    }
    public enum eWallTypeIndex
    {
        Type = 0,
        Name,

        Cnt
    }
    public enum eCharacterTableIndex
    {
        NickName,
        Name,
        Ability,
        Cost,
        Unlock,

        Cnt
    }
    public enum eBattlePassTableIndex
    {
        Type,
        Reward,
        Count,
        Experience,

        Cnt
    }
    public enum eNameTableIndex
    {
        Name,
        ResourceName,

        Cnt
    }
    public enum eQuestChartIndex
    {
        Type,
        Condition,
        Reward,
        Text,

        Cnt
    }

    Dictionary<string, Dictionary<int, Dictionary<string, string>>> _tables;    // <sheet�̸�, <index, < �� ����, �� �� >> >
    string[] URLs;                                                              // �ش� ��Ʈ�� �ּҰ�
    #endregion[���� �������� ��Ʈ ����]

    #region [���� ����]
    bool _loadComplete;
    public bool _isComplete { get { return _loadComplete; } }
    #endregion [���� ����]

    #region [ù ���� �� ������ ��� �Լ�]
    public void Initialize()
    {
        _loadComplete = false;
        _tables = new Dictionary<string, Dictionary<int, Dictionary<string, string>>>();
        URLs = new string[(int)eSheetIndex.Cnt];

        // ���� �������� ��Ʈ edit �������� + /export?format=tsv + &gid=SheetNameID + &range=A1:B1 
        // tsv = tap�� enter�� ������, csv = ,���� ������
        URLs[0] = string.Format("https://docs.google.com/spreadsheets/d/1BPQ5pTP7R61-FDzYadHxlG0lTFL1LdWUoMZiEKieJGs/export?format=tsv&gid={0}&range=B2:F7", (int)eSheetNameGID.LevelProbability);
        URLs[1] = string.Format("https://docs.google.com/spreadsheets/d/1BPQ5pTP7R61-FDzYadHxlG0lTFL1LdWUoMZiEKieJGs/export?format=tsv&gid={0}&range=B1:G81", (int)eSheetNameGID.ObstacleType);
        URLs[2] = string.Format("https://docs.google.com/spreadsheets/d/1BPQ5pTP7R61-FDzYadHxlG0lTFL1LdWUoMZiEKieJGs/export?format=tsv&gid={0}&range=B1:C12", (int)eSheetNameGID.WallType);
        URLs[3] = string.Format("https://docs.google.com/spreadsheets/d/1BPQ5pTP7R61-FDzYadHxlG0lTFL1LdWUoMZiEKieJGs/export?format=tsv&gid={0}&range=B2:F12", (int)eSheetNameGID.CharacterTable);
        URLs[4] = string.Format("https://docs.google.com/spreadsheets/d/1BPQ5pTP7R61-FDzYadHxlG0lTFL1LdWUoMZiEKieJGs/export?format=tsv&gid={0}&range=B2:F6", (int)eSheetNameGID.HardMode);
        URLs[5] = string.Format("https://docs.google.com/spreadsheets/d/1BPQ5pTP7R61-FDzYadHxlG0lTFL1LdWUoMZiEKieJGs/export?format=tsv&gid={0}&range=B2:E42", (int)eSheetNameGID.BattlePass);
        URLs[6] = string.Format("https://docs.google.com/spreadsheets/d/1BPQ5pTP7R61-FDzYadHxlG0lTFL1LdWUoMZiEKieJGs/export?format=tsv&gid={0}&range=B2:C33", (int)eSheetNameGID.FaceTable);
        URLs[7] = string.Format("https://docs.google.com/spreadsheets/d/1BPQ5pTP7R61-FDzYadHxlG0lTFL1LdWUoMZiEKieJGs/export?format=tsv&gid={0}&range=B2:C50", (int)eSheetNameGID.AccessoryTable);
        URLs[8] = string.Format("https://docs.google.com/spreadsheets/d/1BPQ5pTP7R61-FDzYadHxlG0lTFL1LdWUoMZiEKieJGs/export?format=tsv&gid={0}&range=B1:E24", (int)eSheetNameGID.QuestChart);
    }
    public IEnumerator SheetDataLoad()
    {
        Initialize();

        for (int sheetIndex = 0; sheetIndex < (int)eSheetIndex.Cnt; sheetIndex++)
        {
            // ���� �������� ��Ʈ �ҷ�����
            UnityWebRequest www = UnityWebRequest.Get(URLs[sheetIndex]);
            yield return www.SendWebRequest();

            if (!www.isDone)
            {
                Debug.Log("������ �޾ƿ� �� �����ϴ�.");
                continue;
            }
            // �ҷ��� �����͸� ������ �ڸ��� dictionary�� ������ �ֱ�
            string[] rowDatas = www.downloadHandler.text.Split("\r\n");
            Dictionary<int, Dictionary<string, string>> rowTable = new Dictionary<int, Dictionary<string, string>>();

            string[] columnName = rowDatas[0].Split('\t');
            int realIndex = 0;

            // ������ �ุŭ ���� Ȯ��
            for (int rowIndex = 1; rowIndex < rowDatas.Length; rowIndex++)
            {
                // ���� ����� �߶� �ֱ�
                Dictionary<string, string> valueTable = new Dictionary<string, string>();
                string[] columnDatas = rowDatas[rowIndex].Split('\t');

                // ���� ��ĭ�̶�� ���� �ʱ�
                if (columnDatas[0].Length < 1) continue;
                for (int valueIndex = 0; valueIndex < columnDatas.Length; valueIndex++)
                {
                    valueTable.Add(columnName[valueIndex], columnDatas[valueIndex].ToString());
                }
                realIndex++;

                rowTable.Add(realIndex, valueTable);
            }

            _tables.Add(((eSheetIndex)sheetIndex).ToString(), rowTable);
            www.Dispose();
        }

        _loadComplete = true;

        //Debug.Log(_loadComplete);
        //for (int sheetIndex = 0; sheetIndex < (int)eSheetIndex.Cnt; sheetIndex++)
        //{
        //    ShowData(((eSheetIndex)sheetIndex).ToString());
        //}
    }
    void ShowData(string FileName)
    {
        Debug.Log(FileName);
        // ���̺�� [�ε����ѹ� : (�÷���, ��) ...]
        foreach (int index in _tables[FileName].Keys)
        {
            string print = "";
            print += "[" + index + " :";
            foreach (KeyValuePair<string, string> data in _tables[FileName][index])
            {
                print += string.Format(" ({0}, {1})", data.Key, data.Value);
            }
            Debug.Log(print);
        }
    }
    #endregion [ù ���� �� ������ ��� �Լ�]


    #region [�ܺο��� �����͸� ����� �� �Լ�]
    public int TakeCount(eSheetIndex table)
    {
        if (_tables == null)
        {
            return -1;
        }
        if (_tables.ContainsKey(table.ToString()))
        {
            return _tables[table.ToString()].Count;
        }
        return 0;
    }
    public string TakeString(eSheetIndex table, int index, string column)
    {
        if (_tables == null)
        {
            return string.Empty;
        }
        else if (_tables.ContainsKey(table.ToString()))
        {
            Dictionary<int, Dictionary<string, string>> record = _tables[table.ToString()];
            if (record.ContainsKey(index))
            {
                Dictionary<string, string> colData = record[index];
                if (colData.ContainsKey(column))
                {
                    return colData[column];
                }
            }
            else return string.Empty;
        }
        return string.Empty;
    }
    public int TakeInt(eSheetIndex table, int index, string column)
    {
        string data = TakeString(table, index, column);
        if (data.Length == 0) return -1;
        else return int.Parse(data);
    }
    public bool TakeBool(eSheetIndex table, int index, string column)
    {
        return bool.Parse(_tables[table.ToString()][index][column]);
    }
    public float Takefloat(eSheetIndex table, int index, string column)
    {
        string data = TakeString(table, index, column);
        if (data.Length == 0) return -1;
        else return float.Parse(data);
    }
    #endregion [�ܺο��� �����͸� ����� �� �Լ�]
}
