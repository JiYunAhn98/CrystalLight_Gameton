using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class MapController : MonoBehaviour
{
    #region [ ���� ]
    // ���
    readonly int _minBlockCount = 6;

    //����
    int _nowDistance;       // ������� ������ �ͳ��� �Ÿ�, ���� �����ϴ� �ͳ��� ��ġ�� ���
    int _tunnelCount;       // ������� ������ �ͳ� ����, �����ľǿ� ���
    int _nowLevel;          // ���� �ͳ� ���� ����
    int _nextLevelCount;    // ���� ������ ������ ���� �ͳ� ����

    //�ڷᱸ��
    Queue<Tunnel> _activeObject;                    // ���� Ȱ��ȭ�Ǿ� �ִ� ��ü��
    Tunnel _beforeObject;

    // Resources�� ��� ������ <index, ������ü����>
    Dictionary<int, List<GameObject>> _tunnelPool;  // ������ ��ü��
    Dictionary<eItem, List<GameObject>> _items;  // ������ ��ü��
    int[] _percentlevel;                            // ������ �ٸ� Ȯ��
    int _itemTimeStack;
    bool _hardModeOn;

    public int _obstacleFireCnt { get; set; }
    public int _obstacleSawCnt { get; set; }
    public int _obstacleElectronicCnt { get; set; }

    public Tunnel _nowTunnel { get { return _activeObject.Peek(); } }
    public int _nowTunnelCnt { get { return _tunnelCount; } }
    #endregion [ ���� ]

    #region [ ���� �Լ� ]
    void ModeSetting()
    {
        _hardModeOn = (BackendGameData.Instance.UserGameData.selectMode == (int)eLevelMode.Infinite && BackendGameData.Instance.UserGameData.selectInfiniteLevel == (int)eInfiniteLevel.HARD);
    }
    void LevelSetting()
    {
        _nowLevel++;
        if (_hardModeOn)
        {
            _nextLevelCount = GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.HardMode, _nowLevel, GoogleSheetManager.eLevelProbabilityIndex.TunnelCount.ToString());
            for (int i = 0; i < _percentlevel.Length; i++)
            {
                _percentlevel[i] = GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.HardMode, _nowLevel, ((GoogleSheetManager.eLevelProbabilityIndex)i + 1).ToString());
            }

            if (GoogleSheetManager._instance.TakeCount(GoogleSheetManager.eSheetIndex.HardMode) <= _nowLevel)
            {
                _nextLevelCount = int.MaxValue;
            }
        }
        else
        {
            _nextLevelCount = GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.LevelProbability, _nowLevel, GoogleSheetManager.eLevelProbabilityIndex.TunnelCount.ToString());
            for (int i = 0; i < _percentlevel.Length; i++)
            {
                _percentlevel[i] = GoogleSheetManager._instance.TakeInt(GoogleSheetManager.eSheetIndex.LevelProbability, _nowLevel, ((GoogleSheetManager.eLevelProbabilityIndex)i + 1).ToString());
            }

            if (GoogleSheetManager._instance.TakeCount(GoogleSheetManager.eSheetIndex.LevelProbability) <= _nowLevel)
            {
                _nextLevelCount = int.MaxValue;
            }
        }
    }
    void GenTunnel(float time)
    {
        //level�� ���� Ȯ���� �ͳλ���
        int randomNum = Random.Range(0, 100);
        int index = 0, level = 0, poolIndex = 1;
        GameObject go = null;

        // ������ �ɸ´� ���� ��ֹ� ����
        for (level = 0; level < _percentlevel.Length; level++)
        {
            if (randomNum < _percentlevel[level])
            {
                break;
            }
            else
            {
                poolIndex += PrefabManager._instance.LevelTunnelCount(level);
            }
        }
        index = Random.Range(0, PrefabManager._instance.LevelTunnelCount(level));
        poolIndex += index;

        // ������Ʈ Ǯ�� ���� �ִٸ� active�� �ƴ� ģ���� �������� ���ٸ� ���� �����
        if (_tunnelPool.ContainsKey(poolIndex))
        {
            for (int i = 0; i < _tunnelPool[poolIndex].Count; i++)
            {
                if (!_tunnelPool[poolIndex][i].activeSelf)
                {
                    go = _tunnelPool[poolIndex][i];
                    break;
                }
            }
        }
        else
        {
            _tunnelPool.Add(poolIndex, new List<GameObject>());
        }

        // �ͳ��� null�̶�� �� �����ִ� �ش� ������Ʈ�� ã�� ���߱⿡ ���� ���� �־��ֱ�
        if (go == null)
        {
            go = Instantiate(PrefabManager._instance.GetObstacle(level, index));
            _tunnelPool[poolIndex].Add(go);
        }

        // �� �ͳο� ���� ���� �ʱ�ȭ
        Tunnel tunnel = go.GetComponentInChildren<Tunnel>();
        _nowDistance += tunnel.StateSetting(poolIndex, _nowDistance);
        _activeObject.Enqueue(tunnel);
        _tunnelCount++;

        if (time / 20 >= _itemTimeStack)
        {
            GenItem((eItem)Random.Range(1, (int)eItem.Cnt), tunnel);
            _itemTimeStack++;
        }
        else if (_tunnelCount > 2)
        {
            GenItem(eItem.Crystal, tunnel);
        }

        // �ͳ��� �����߱� ������ ���� ������Ʈ
        if (_tunnelCount >= _nextLevelCount)
        {
            LevelSetting();
        }
    }
    void GenStoryTunnel(float time)
    {
        //level�� ���� Ȯ���� �ͳλ���
        int index = 0, level = 0, poolIndex = 1;
        GameObject go = null;

        level = BackendGameData.Instance.UserGameData.selectTutorialLevel + 1;
        // ������ �ɸ´� ���� ��ֹ� ����
        for (int i = 0; i < level; i++)
        {
            poolIndex += PrefabManager._instance.LevelTunnelCount(i);
        }
        index = Random.Range(0, PrefabManager._instance.LevelTunnelCount(level));
        poolIndex += index;

        // ������Ʈ Ǯ�� ���� �ִٸ� active�� �ƴ� ģ���� �������� ���ٸ� ���� �����
        if (_tunnelPool.ContainsKey(poolIndex))
        {
            for (int i = 0; i < _tunnelPool[poolIndex].Count; i++)
            {
                if (!_tunnelPool[poolIndex][i].activeSelf)
                {
                    go = _tunnelPool[poolIndex][i];
                    break;
                }
            }
        }
        else
        {
            _tunnelPool.Add(poolIndex, new List<GameObject>());
        }

        // �ͳ��� null�̶�� �� �����ִ� �ش� ������Ʈ�� ã�� ���߱⿡ ���� ���� �־��ֱ�
        if (go == null)
        {
            go = Instantiate(PrefabManager._instance.GetObstacle(level, index));
            _tunnelPool[poolIndex].Add(go);
        }

        // �� �ͳο� ���� ���� �ʱ�ȭ
        Tunnel tunnel = go.GetComponentInChildren<Tunnel>();
        _nowDistance += tunnel.StateSetting(poolIndex, _nowDistance);
        _activeObject.Enqueue(tunnel);
        _tunnelCount++;

        // ���丮 ��忡�� �������� ���´�?
        // ������ �츮 ��Ҹ� �˾ƹ�����, 


        if (BackendGameData.Instance.UserGameData.selectTutorialLevel > (int)eTutorialLevel.STORY1 && time / 20 >= _itemTimeStack)
        {
            GenItem((eItem)Random.Range(1, (int)eItem.Cnt), tunnel);
            _itemTimeStack++;
        }
        else if (_tunnelCount > 2)
        {
            GenItem(eItem.Crystal, tunnel);
        }

        //�ͳ��� �����߱� ������ ���� ������Ʈ
        if (_tunnelCount >= _nextLevelCount)
        {
            LevelSetting();
        }
    }
    void GenItem(eItem item, Tunnel tunnel)
    {
        GameObject go = null;
        if (_items.ContainsKey(item))
        {
            int i;
            for (i = 0; i < _items[item].Count; i++)
            {
                if (!_items[item][i].activeSelf) break;
            }
            if (i < _items[item].Count)
            {
                go = _items[item][i];
                go.SetActive(true);
            }
            else
            {
                go = Instantiate(PrefabManager._instance.GetItem(item));
                _items[item].Add(go);
            }
        }
        else
        {
            _items.Add(item, new List<GameObject>());
            go = Instantiate(PrefabManager._instance.GetItem(item));
            _items[item].Add(go);
        }
        tunnel.ItemSpawn(go);
    }

    #endregion [ ���� �Լ�]

    #region [ �ܺ� �Լ� ]
    public void Initialize()
    {
        _activeObject = new Queue<Tunnel>();
        _tunnelPool = new Dictionary<int, List<GameObject>>();
        _percentlevel = new int[(int)GoogleSheetManager.eLevelProbabilityIndex.Cnt - 2];
        _items = new Dictionary<eItem, List<GameObject>>();
        _nowLevel = 0;
        _nowDistance = 0;
        _tunnelCount = 0;
        _itemTimeStack = 1;
        _obstacleFireCnt = 0;
        _obstacleElectronicCnt = 0;
        _obstacleSawCnt = 0;
        ModeSetting();
        LevelSetting();
        _beforeObject = Instantiate(PrefabManager._instance.GetObstacle(0, 0), Vector3.back * (int)eWall.WallSmall, Quaternion.identity).GetComponent<Tunnel>();

        if (BackendGameData.Instance.UserGameData.selectMode == (int)eLevelMode.Infinite)
        {
            for (int i = 0; i < _minBlockCount; i++)
            {
                GenTunnel(0);
            }
        }
        else
        {
            for (int i = 0; i < _minBlockCount; i++)
            {
                GenStoryTunnel(0);
            }
        }
    }

    // <summary>
    // �ͳ��� �������� ��ü
    // </summary>
    public void MapUpdate(float time)
    {
        if (_beforeObject != null)
        {
            if (BackendGameData.Instance.UserGameData.selectMode == (int)eLevelMode.Infinite)
                PassObstacleForMission(_beforeObject);
            _beforeObject.ActiveFalse();
        }
        _beforeObject = _activeObject.Peek();
        _activeObject.Peek().ItemActiveFalse();
        _activeObject.Dequeue();

        if (BackendGameData.Instance.UserGameData.selectMode == (int)eLevelMode.Infinite)
        {
            GenTunnel(time);
        }
        else
        {
            GenStoryTunnel(time);
        }
    }

    public void PassObstacleForMission(Tunnel nowTunnel)
    {
        switch (nowTunnel._obstacle)
        {
            case eDebuffState.SawBlade:
                _obstacleSawCnt++;
                break;
            case eDebuffState.Fire:
                _obstacleFireCnt++;
                break;
            case eDebuffState.Electronic:
                _obstacleElectronicCnt++;
                break;
        }
    }
    #endregion [ �ܺ� �Լ�]
}
