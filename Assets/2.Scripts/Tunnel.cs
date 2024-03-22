using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class Tunnel : MonoBehaviour
{
    #region [����]
    // ���� ����
    Transform _itemSpots;
    MeshRenderer _meshRenderer;     // �ͳ� �ܺ� material

    // ���� ����
    GoogleSheetManager.eSheetIndex sheet;

    int _materialType;      // WallType���� �о�� ���͸��� Ÿ��
    eDebuffState _wallType;       // �ǰ� �� ����
    eDebuffState _obstacleType;   // �ǰ� �� ����
    eWall _tunnelType;      // ���� �������� �뵵
    
    // ���� ���� 
    int _mySize;            // �� ����
    GameObject _item;
    #endregion [����]

    public int _size { get { return _mySize; } }
    public eDebuffState _wall { get { return _wallType; } }
    public eDebuffState _obstacle { get { return _obstacleType; } }

    /// <summary>
    ///  Google�� �ִ� ������ �ڽ��� �ڷ������� ��ȯ
    /// </summary>
    /// <param name="sheetIndex"> Google Sheet�� �ִ� ObstacleType�� Index</param>
    /// <param name="distance"> ������� ������ �ͳ��� ���� </param>
    /// <returns> ������ �ͳ��� ���� </returns>
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
