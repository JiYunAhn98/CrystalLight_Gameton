using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class PrefabManager : MonoSingleton<PrefabManager>
{
    #region [���� ����]
    // Resources�� ��� ������ <Level, List(index, Name)>
    Dictionary<int, List<GameObject>> _obstaclePrefabs;

    // Resources�� ��� Material <Type, Name> & <Name, Material>
    Dictionary<string, Material> _materials;
    Dictionary<int, List<string>> _materialNames;

    // WallPrefab
    List<GameObject> _wallPrefabs;

    // Resources�� ��� ĳ���� ������Ʈ
    List<Material> _characterMaterialPrefabs;
    GameObject _character;
    GameObject _characterModel;
    List<Sprite> _characterThumbnails;

    // �Ǽ��縮�� �� ǥ��
    GameObject _faceModel;
    List<Material> _facePrefabs;
    List<Sprite> _faceThumbnails;
    List<GameObject> _accessaryPrefabs;
    Sprite[] _accessaryThumbnails;
    // ����Ʈ
    List<GameObject> _particlePrefabs;
    // ������
    List<GameObject> _ItemPrefabs;


    #endregion [���� ����]

    #region [�ܺ� ����]
    public int LevelTunnelCount(int level)
    {
        return _obstaclePrefabs[level].Count;
    }
    public int LevelTunnelIndex(int level)
    {
        int count = 0;
        for (int i = 0; i < level; i++)
        {
            count+= _obstaclePrefabs[i].Count;
        }
        return count + _obstaclePrefabs[level].Count;
    }
    /// <summary>
    /// Prefab�������� �ʼ� ������Ʈ �����α�
    /// </summary>
    public void Initialize()
    {
        _obstaclePrefabs = new Dictionary<int, List<GameObject>>();
        _materials = new Dictionary<string, Material>();
        _materialNames = new Dictionary<int, List<string>>();
        _wallPrefabs = new List<GameObject>();
        _characterMaterialPrefabs = new List<Material>();
        _facePrefabs = new List<Material>();
        _particlePrefabs = new List<GameObject>();
        _accessaryPrefabs = new List<GameObject>();
        _ItemPrefabs = new List<GameObject>();
        _faceThumbnails = new List<Sprite>();
        _characterThumbnails = new List<Sprite>();

        SetObstacle(GoogleSheetManager.eSheetIndex.ObstacleType);
        SetWallType(GoogleSheetManager.eSheetIndex.WallType);
        SetCharacter();
        SetAccessary();
        SetFace();
        SetParticle();
        SetItem();

    }
    public GameObject GetCharacterModel()
    {
        return _characterModel;
    }
    public GameObject GetfaceModel()
    {
        return _faceModel;
    }
    public Sprite GetCharacterThumbnail(eCharacter index)
    {
        return _characterThumbnails[(int)index];
    }
    public Material GetFaceMaterial(eFace index)
    {
        return _facePrefabs[(int)index];
    }
    public Sprite GetFaceThumbnail(eFace index)
    {
        return _faceThumbnails[(int)index];
    }
    public GameObject GetItem(eItem index)
    {
        return _ItemPrefabs[(int)index];
    }
    public GameObject GetAccessary(eAccessory index)
    {
        return _accessaryPrefabs[(int)index];
    }
    public Sprite GetAccessaryThumbnail(eAccessory index)
    {
        return _accessaryThumbnails[(int)index];
    }
    /// <summary>
    /// �ش� Obstacle ��������
    /// </summary>
    /// <param name="name"></param>
    /// <param num="num"> ���� Google Sheet Manager���� �޾ƾ��� ���� index </param>
    /// <returns></returns>
    public GameObject GetObstacle(int level, int index)
    {
        return _obstaclePrefabs[level][index];
    }
    public Material GetMaterialByType(int type)
    {
        string index = _materialNames[type][Random.Range(0, _materialNames[type].Count)];

        return _materials[index];
    }
    public Material GetCharacterMaterial(eCharacter type)
    {
        return _characterMaterialPrefabs[(int)type];
    }
    public GameObject GetParticle(eParticle particle)
    {
        return _particlePrefabs[(int)particle];
    }
    /// <summary>
    /// ���͸��� �ҷ�����
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    //public Material GetMat(string name)
    //{
    //    return _materials[name];
    //}
    #endregion [�ܺ� ����]

    #region [���� �Լ�]
    void SetItem()
    {
        for (int i = 0; i < (int)eItem.Cnt; i++)
        {
            _ItemPrefabs.Add(Resources.Load("Items/" + ((eItem)i).ToString()) as GameObject);
        }
    }
    void SetParticle()
    {
        for (int i = 0; i < (int)eParticle.Cnt; i++)
        {
            _particlePrefabs.Add(Resources.Load("Particles/" + ((eParticle)i).ToString()) as GameObject) ;
        }
    }
    void SetCharacter()
    {
        _character = Resources.Load("Characters/Character") as GameObject;
        _characterModel = Resources.Load("Characters/CharacterModel") as GameObject;
        for (int i = 0; i < (int)eCharacter.Cnt; i++)
        {
            _characterMaterialPrefabs.Add(Resources.Load<Material>("Characters/" + ((eCharacter)i).ToString()));
            _characterThumbnails.Add(Resources.Load<Sprite>("Characters/CharacterSprite/" + ((eCharacter)i).ToString()));
        }
    }
    void SetFace()
    {
        _faceModel = Resources.Load("Faces/Face") as GameObject;
        for (int i = 0; i < (int)eFace.Cnt; i++)
        {
            _facePrefabs.Add(Resources.Load<Material>("Faces/Materials/" + ((eFace)i).ToString()));
            _faceThumbnails.Add(Resources.Load<Sprite>("Faces/" + ((eFace)i).ToString()));
        }
    }
    void SetAccessary()
    {
        for (int i = 0; i < (int)eAccessory.Cnt; i++)
        {
            _accessaryPrefabs.Add(Resources.Load("Accessories/" + ((eAccessory)i).ToString()) as GameObject);
        }
        _accessaryThumbnails = Resources.LoadAll<Sprite>("Accessories/AccessoryThumbnail");
    }
    void SetObstacle(GoogleSheetManager.eSheetIndex sheet)
    {
        int count = GoogleSheetManager._instance.TakeCount(sheet);
        int index = 1, level;
        string name;

        while (index < count + 1)
        {
            level = GoogleSheetManager._instance.TakeInt(sheet, index, GoogleSheetManager.eObstacleIndex.Level.ToString());

            if (!_obstaclePrefabs.ContainsKey(level))
            {
                _obstaclePrefabs.Add(level, new List<GameObject>());
            }

            do
            {
                name = GoogleSheetManager._instance.TakeString(sheet, index, GoogleSheetManager.eObstacleIndex.Name.ToString());
                _obstaclePrefabs[level].Add(Resources.Load("Tunnels/" + name) as GameObject);
                index++;
            }
            while (index < count && GoogleSheetManager._instance.TakeInt(sheet, index, GoogleSheetManager.eObstacleIndex.Level.ToString()) == level);

        }
    }
    void SetWallType(GoogleSheetManager.eSheetIndex sheet)
    {
        int count = GoogleSheetManager._instance.TakeCount(sheet);
        string name;
        int type;

        for (int index = 1; index <= count; index++)
        {
            name = GoogleSheetManager._instance.TakeString(sheet, index, GoogleSheetManager.eWallTypeIndex.Name.ToString());
            type = GoogleSheetManager._instance.TakeInt(sheet, index, GoogleSheetManager.eWallTypeIndex.Type.ToString());

            if (!_materialNames.ContainsKey(type))
            {
                _materialNames.Add(type, new List<string>());
            }
            _materialNames[type].Add(name);

            if (!_materials.ContainsKey(name))
            {
                _materials.Add(name, Resources.Load<Material>("Materials/" + name));
            }

        }
    }
    #endregion [���� �Լ�]
}
