using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class MapController : MonoBehaviour
{
    #region [ 변수 ]
    // 상수
    readonly int _minBlockCount = 6;

    //변수
    int _nowDistance;       // 현재까지 생성한 터널의 거리, 새로 생성하는 터널의 위치에 사용
    int _tunnelCount;       // 현재까지 생성한 터널 개수, 레벨파악에 사용
    int _nowLevel;          // 현재 터널 생성 레벨
    int _nextLevelCount;    // 다음 레벨로 오르기 위한 터널 개수

    //자료구조
    Queue<Tunnel> _activeObject;                    // 현재 활성화되어 있는 객체들
    Tunnel _beforeObject;

    // Resources에 담긴 프리팹 <index, 생성객체모음>
    Dictionary<int, List<GameObject>> _tunnelPool;  // 생성된 객체들
    Dictionary<eItem, List<GameObject>> _items;  // 생성된 객체들
    int[] _percentlevel;                            // 레벨에 다른 확률
    int _itemTimeStack;
    bool _hardModeOn;

    public int _obstacleFireCnt { get; set; }
    public int _obstacleSawCnt { get; set; }
    public int _obstacleElectronicCnt { get; set; }

    public Tunnel _nowTunnel { get { return _activeObject.Peek(); } }
    public int _nowTunnelCnt { get { return _tunnelCount; } }
    #endregion [ 변수 ]

    #region [ 내부 함수 ]
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
        //level에 따른 확률로 터널생성
        int randomNum = Random.Range(0, 100);
        int index = 0, level = 0, poolIndex = 1;
        GameObject go = null;

        // 레벨에 걸맞는 랜덤 장애물 선정
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

        // 오브젝트 풀에 값이 있다면 active가 아닌 친구를 가져오고 없다면 새로 만들기
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

        // 터널이 null이라는 건 꺼져있던 해당 오브젝트를 찾지 못했기에 새로 만들어서 넣어주기
        if (go == null)
        {
            go = Instantiate(PrefabManager._instance.GetObstacle(level, index));
            _tunnelPool[poolIndex].Add(go);
        }

        // 고른 터널에 대한 정보 초기화
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

        // 터널을 생성했기 때문에 정보 업데이트
        if (_tunnelCount >= _nextLevelCount)
        {
            LevelSetting();
        }
    }
    void GenStoryTunnel(float time)
    {
        //level에 따른 확률로 터널생성
        int index = 0, level = 0, poolIndex = 1;
        GameObject go = null;

        level = BackendGameData.Instance.UserGameData.selectTutorialLevel + 1;
        // 레벨에 걸맞는 랜덤 장애물 선정
        for (int i = 0; i < level; i++)
        {
            poolIndex += PrefabManager._instance.LevelTunnelCount(i);
        }
        index = Random.Range(0, PrefabManager._instance.LevelTunnelCount(level));
        poolIndex += index;

        // 오브젝트 풀에 값이 있다면 active가 아닌 친구를 가져오고 없다면 새로 만들기
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

        // 터널이 null이라는 건 꺼져있던 해당 오브젝트를 찾지 못했기에 새로 만들어서 넣어주기
        if (go == null)
        {
            go = Instantiate(PrefabManager._instance.GetObstacle(level, index));
            _tunnelPool[poolIndex].Add(go);
        }

        // 고른 터널에 대한 정보 초기화
        Tunnel tunnel = go.GetComponentInChildren<Tunnel>();
        _nowDistance += tunnel.StateSetting(poolIndex, _nowDistance);
        _activeObject.Enqueue(tunnel);
        _tunnelCount++;

        // 스토리 모드에서 아이템이 나온다?
        // 유저가 우리 요소를 알아버린다, 


        if (BackendGameData.Instance.UserGameData.selectTutorialLevel > (int)eTutorialLevel.STORY1 && time / 20 >= _itemTimeStack)
        {
            GenItem((eItem)Random.Range(1, (int)eItem.Cnt), tunnel);
            _itemTimeStack++;
        }
        else if (_tunnelCount > 2)
        {
            GenItem(eItem.Crystal, tunnel);
        }

        //터널을 생성했기 때문에 정보 업데이트
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

    #endregion [ 내부 함수]

    #region [ 외부 함수 ]
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
    // 터널을 랜덤으로 교체
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
    #endregion [ 외부 함수]
}
