using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineHelper;

public class InGameManager : DestroySingleton<InGameManager>
{
    [SerializeField] MapController _mapController;
    [SerializeField] PlayerMove _player;
    [SerializeField] ResultPopup _result;
    [SerializeField] IngameMainCanvas _canvas;
    [SerializeField] StopPopup _stopWnd;
    [SerializeField] CheckTryInform _infrom;

    public GameController _backendGameController;

    eProgress _nowProg;
    eProgress _beforeProg;

    float _distance;            // ���ݱ��� �޸� �Ÿ�
    float _nowTunnelDistance;   // ���� �ͳο��� �޸� �Ÿ�
    float _genDistance;         // ���� MapUpdate�� �Ϸ��� �Ÿ�
    float _time;                // ������� �޸� �ð�
    bool _isReplay;             // �ٽ��ϱ⸦ �ߴ°�?
    bool _isCrystalReplay;         // ũ����Ż�ΰ�?
    float _delayTime;           // ������ �ð�

    // �����۰� �ɷ� ����
    //float _slowStack;

    public eProgress _nowProgress { get { return _nowProg; } }
    public eDebuffState _wallHitInform { get { return _mapController._nowTunnel._wall; } }
    public eDebuffState _obstacleHitInform { get { return _mapController._nowTunnel._obstacle; } }
    public float _itemScore { get; set; }
    public int _itemProbability { get; set; }     // ������ ���� �ͳ� ��

    #region[ Unity Life Cycle]
    void Start()
    {
        ProgInit();
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause && _nowProg == eProgress.Play)
        {
            _canvas.StopBtn();
        }

    }
    void Update()
    {
        switch (_nowProg)
        {
            case eProgress.Ready:

                // �Է��� ����� ����
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (Input.GetMouseButton(0))
                {
                    ProgPlay();
                }
#elif UNITY_ANDROID || UNITY_IOS
                if (Input.touchCount >= 1)
                {
                    ProgPlay();
                }
#endif
                break;

            case eProgress.Replay:
                if (_canvas.CountDown(3) <= 0)
                {
                    ProgPlay();
                }
                break;

            case eProgress.Stop:

                if (Time.timeScale <= 1)
                {
                    if (_canvas.CountDown(3) <= 0)
                    {
                        ProgPlay();
                    }
                }
                break;
            case eProgress.Play:
                _infrom.SetTryInform(_mapController._nowTunnel._obstacle.ToString(), _mapController._nowTunnel._wall.ToString(), (_mapController._nowTunnelCnt - 6).ToString(), ((int)_time).ToString(), _player._nowSpeed.ToString());

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                if (Input.GetMouseButton(0))
                {
                    _player.Move();
                }
                else
                {
                    _player.ResetPosition();
                }
#elif UNITY_ANDROID || UNITY_IOS
                if (Input.touchCount < 1)
                {
                    _player.ResetPosition();
                }
                else
                {
                    _player.Move();
                }

                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Menu) || Input.GetKeyDown(KeyCode.Home))
                {
                    _canvas.StopBtn();
                }
#endif
                float deltime = Time.deltaTime;
                _time += deltime;
                //float dis = _player.ForwardMove(_time + _slowStack);
                float dis = _player.ForwardMove(deltime);
                _distance += dis;

                if (_nowTunnelDistance + dis >= _mapController._nowTunnel._size)
                {
                    _nowTunnelDistance -= _mapController._nowTunnel._size;
                }
                _nowTunnelDistance += dis;

                if (_distance > _genDistance)
                {
                    _mapController.MapUpdate(_time);
                    _genDistance += _mapController._nowTunnel._size;
                }

                if (_player._isWallTrigger)
                {
                    _player.HitWall();
                    _player._isWallTrigger = false;
                }
                if (_player._isObstacleTrigger)
                {
                    _player.HitCollider();
                    _player._isObstacleTrigger = false;
                }
                
                if (BackendGameData.Instance.UserGameData.selectMode == (int)eLevelMode.Story && Score.Instance.TutorialComplete())
                {
                    ProgResult();
                }

                break;
            case eProgress.End:
                _delayTime -= Time.deltaTime;
                if (_delayTime <= 0)
                {
                    ProgResult();
                }
                break;
        }
    }
    #endregion[ Unity Life Cycle]

    #region[ Prog FSM ]
    void ProgInit()
    {
        _nowProg = eProgress.Init;
        SoundManager._instance.PlayBGM(eBGMSound.Ingame);

        if (BackendGameData.Instance.UserGameData.selectMode == (int)eLevelMode.Infinite)
        {
            if (BackendGameData.Instance.UserGameData.heart > 0)
            {
                _isReplay = true;                              // �ٽý��� Ƽ�� Ȯ��
                _canvas.SetHeart(true);
            }
            else
            {
                _isReplay = false;                              // �ٽý��� Ƽ�� Ȯ��
                _canvas.SetHeart(false);
            }
        }
        else
        {
            _isReplay = false;                              // �ٽý��� Ƽ�� Ȯ��
            _canvas.SetHeart(false);
            _canvas.SetTutorialMaxScore();
        }

        _isCrystalReplay = BackendGameData.Instance.UserGameData.selectBody == (int)eCharacter.Crystal;
        _canvas.SetCrystalLife(_isCrystalReplay);  // ũ����Ż ĳ���� ������ �����ϱ�
        _canvas.SetDeadImmune(BackendGameData.Instance.UserGameData.selectBody == (int)eCharacter.Silver);  // �ǹ� ĳ���� ������ �����ϱ�
        _canvas.SetStartMessage(true);

        // ���� �ʱ�ȭ
        _time = 1;
        _itemScore = Constants.BASE_CRYSTAL_SCORE + (float)BackendGameData.Instance.UserGameData.itemLevel[(int)eItem.Crystal] * Constants.UPGRADE_CRYSTAL_SCORE;
        _distance = 0;
        _itemProbability = 20;
        //_slowStack = 0;

        // ��ũ��Ʈ Initialize
        EffectManager._instance.Initialize();       // ����Ʈ �Ŵ��� Lazy Instance
        _player.Initialize();                       // �÷��̾� �ʱ�ȭ
        _mapController.Initialize();                // �� �ʱ�ȭ
        _stopWnd.Initialize();                      // ��ž ������ �ʱ�ȭ
        _stopWnd.gameObject.SetActive(false);
        _canvas.Initialize();

        _genDistance = _mapController._nowTunnel._size;

        StartCoroutine(_result.BGFill(false));      // ���� �Ѿ�� ��� ǥ��

        ProgReady();
    }

    void ProgReady()
    {
        _nowProg = eProgress.Ready;
    }

    void ProgPlay()
    {
        _nowProg = eProgress.Play;      // ���� ���� ��
        Score.Instance.EndGame(false);  // ���� ���� ����
        _player.RotateCharacter(Vector3.right);
        _canvas.SetStartMessage(false);
    }
    public void ProgEnd(float time)
    {
        _nowProg = eProgress.End;       // ���� ����
        EffectManager._instance.Vibrate();
        Score.Instance.EndGame(true);
        _delayTime = time;

        // �ٽ��� �� ����
    }
    void ProgResult()
    {
        _nowProg = eProgress.Result;        // ���� ��� â

        Score.Instance.EndGame(true);

        // ������ ����
        if (_isReplay || _isCrystalReplay)
        {
            ProgReplay();
        }
        else
        {
            _backendGameController.GameOver();
            if (BackendGameData.Instance.UserGameData.selectMode == (int)eLevelMode.Story)
            {
                StartCoroutine(_result.TutorialResult(Score.Instance.TutorialComplete()));
            }
            else
            {
                if (BackendGameData.Instance.UserGameData.gameCount % 3 == 2 && BackendStoreData.Instance.StoreGameData.eraseAdNotYet)
                {
                    ADManager.Instance._interstitialAD.ShowInterstitialAd();
                }
                BackendGameData.Instance.DoingMission(eMissionType.PlayGame);
                BackendGameData.Instance.DoingMission(eMissionType.SafeTime, (int)_time);
                BackendGameData.Instance.DoingMission(eMissionType.FirePass, _mapController._obstacleFireCnt);
                BackendGameData.Instance.DoingMission(eMissionType.ElectronicPass, _mapController._obstacleElectronicCnt);
                BackendGameData.Instance.DoingMission(eMissionType.SawBladePass, _mapController._obstacleSawCnt);
                BackendGameData.Instance.DoingMission(eMissionType.GetItem, _player._itemGetCount);
                StartCoroutine(_result.BGFill(true));
            }
        }
    }
    public void ProgStop()
    {
        Time.timeScale = 0;
        if (_nowProg == eProgress.Play) _nowProg = eProgress.Stop;
        _player.ResetPosition();
        Score.Instance.EndGame(true);
    }
    public void ProgReplay()
    {
        _nowProg = eProgress.Replay;        // ���� �ٽ� ����

        if (_isCrystalReplay)
        {
            _isCrystalReplay = false;
            _canvas.SetCrystalLife(false);
        }
        else if (_isReplay)
        {
            _isReplay = false;                  // �ٽý��� Ƽ�� Ȯ��
            _canvas.SetHeart(false);
            BackendGameData.Instance.UserGameData.heart--;
        }

        _player.ResetPosition();
        BackendGameData.Instance.GameDataUpdate();
        StopAllCoroutines();
        StartCoroutine(_result.BGFill(false));
        _player.Active(true);

        StartCoroutine(_player.Super(13));
        StartCoroutine(EffectManager._instance.PlayEffectTimer(eParticle.Super, _player.transform, 13));
        SoundManager._instance.PlayEffect(eEffectSound.Super);

        // Update���� 3�� �ڿ� Play�� �̵�
    }
    #endregion[ Prog FSM ]

    #region[ ���� �Լ� ]
    #endregion[ ���� �Լ� ]

    #region[ �ܺ� �Լ� ]
    public void SlowDown(float time)
    {
        //_slowStack-= time;    //�鿣�� ���ο� ������ ��������
        
    }
    #endregion[ �ܺ� �Լ� ]
}
