using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class Tunnel : MonoBehaviour
{
    #region [변수]
    // 참조 변수
    Transform _itemSpots;
    MeshRenderer _meshRenderer;     // 터널 외벽 material

    // 구글 변수
    GoogleSheetManager.eSheetIndex sheet;

    int _materialType;      // WallType에서 읽어올 머터리얼 타입
    eDebuffState _wallType;       // 피격 시 정보
    eDebuffState _obstacleType;   // 피격 시 정보
    eWall _tunnelType;      // 길이 가져오는 용도
    
    // 상태 변수 
    int _mySize;            // 내 길이
    GameObject _item;
    #endregion [변수]

    public int _size { get { return _mySize; } }
    public eDebuffState _wall { get { return _wallType; } }
    public eDebuffState _obstacle { get { return _obstacleType; } }

    /// <summary>
    ///  Google에 있는 정보를 자신의 자료형으로 교환
    /// </summary>
    /// <param name="sheetIndex"> Google Sheet에 있는 ObstacleType의 Index</param>
    /// <param name="distance"> 현재까지 생성된 터널의 길이 </param>
    /// <returns> 생성한 터널의 길이 </returns>
    public int StateSetting(int sheetIndex, int distance)
    {
        if (_meshRenderer == null)
        {
            sheet = GoogleSheetManager.eSheetIndex.ObstacleType;
            _meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
            _itemSpots = transform.GetChild(3).GetComponent<Transform>();
        }

        _tunnelType = eWall.None;
        _wallType = (eDebuffState)System.Enum.Parse(typeof(eDebuffState), GoogleSheetManager._instance.TakeString(sheet, sheetIndex, GoogleSheetManager.eObstacleIndex.WallType.ToString()));
        _obstacleType = (eDebuffState)System.Enum.Parse(typeof(eDebuffState), GoogleSheetManager._instance.TakeString(sheet, sheetIndex, GoogleSheetManager.eObstacleIndex.ObstacleType.ToString()));
        _tunnelType = (eWall) System.Enum.Parse(typeof(eWall), GoogleSheetManager._instance.TakeString(sheet, sheetIndex, GoogleSheetManager.eObstacleIndex.TunnelType.ToString()));

        _mySize = (int)_tunnelType;

        _materialType = GoogleSheetManager._instance.TakeInt(sheet, sheetIndex, GoogleSheetManager.eObstacleIndex.Material.ToString());
        _meshRenderer.material = PrefabManager._instance.GetMaterialByType(_materialType);

        transform.position = Vector3.forward * distance;
        transform.Rotate(Vector3.forward * Random.Range(-180, 180));
        gameObject.SetActive(true);

        return _mySize;
    }
    public void ItemSpawn(GameObject go)
    {
        _item = go;
        Transform itemGenTf = _itemSpots.GetChild(Random.Range(0, _itemSpots.childCount));
        itemGenTf.gameObject.SetActive(true);
        go.transform.parent = itemGenTf;
        go.transform.localPosition = Vector3.zero;
    }
    public void ActiveFalse()
    {
        gameObject.SetActive(false);
    }
    public void ItemActiveFalse()
    {
        if(_item != null)
            _item.SetActive(false);
    }
}
