using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using DefineHelper;

public class PlayerMove : MonoBehaviour
{
	readonly int SHADOW_SIZE = 10;

	#region [ 직렬화 변수 ]
	[SerializeField] IngameMainCanvas _mainCanvas;
	[Header("이동 조절 기능")]
	[SerializeField] float _radius = 5;
	[SerializeField] float _shadowOffset = 1.5f;
	[SerializeField] float _startSpeed = 50;
	[SerializeField] float _accelration = 0.25f;
	[Header("카메라 조절 기능")]
	[SerializeField] [Range(0, 20)] float _offsetZ = 18;
	[SerializeField] [Range(1, 10)] float _mulVal = 1.5f;

	[Header("속도 이펙트")]
	[SerializeField] ParticleSystem _lightLine;
	[SerializeField] ParticleSystem _focusLine;

	[Header("하위 오브젝트")]
	[SerializeField] GameObject _character;
	[SerializeField] SphereCollider _itemCollider;
	[SerializeField] Transform _accessaryPos;
    #endregion [ 직렬화 변수 ]

    #region [ 내부 변수 ]
    // 참조 변수
    MeshRenderer[] _material;
	Animator _animController;
	GameObject _shadow;
	Rigidbody _rigid;

	// 정보 변수
	Vector2 _point;                             // 첫 터치 저장
	Vector2 _fixPosition;                       // 첫 터치 시 기준이 될 벡터
	float _speed;                               // 현재 플레이어 속도
	eCharacterState _nowWallState;              // 현재 플레이어 타격 모습
	eCharacterState _nowObstacleState;          // 현재 플레이어 타격 모습

	Vector3 _bounceVec;
	bool _isBounce;
	bool _notDoubleDie;

	// 상태 변수
	float _fixItemTime = 10;
	Vector2 _vec2;              // 회전 보간 수

	public int _itemGetCount { get; set; }

	public bool _isWallTrigger { get; set; }
	public bool _isObstacleTrigger { get; set; }

	// 아이템 효과
	public bool _isMagnet { get; set; }
	public bool _isShield { get; set; }
	public bool _isDoubleMagnet { get; set; }
	public bool _isDoubleShield { get; set; }

	// 자체 효과
	public bool _isSuper { get; set; }
	public bool _isIceImmune { get; set; }
	public bool _isFireImmune { get; set; }
	public bool _isDeadImmune { get; set; }
	#endregion [ 내부 변수 ]

	#region [ 외부 함수 ]
	public float _itemTime { get { return _fixItemTime; } }
	public Vector3 _bounceVector { set { _bounceVec = value; } }
    public eCharacterState _wallState { get { return _nowWallState; } set { if(_nowWallState < value) _nowWallState = value; } }
	public eCharacterState _ObstacleState { get { return _nowObstacleState; } set { if (_nowObstacleState < value) _nowObstacleState = value; } }
	public int _nowSpeed { get { return (int)_speed; } }

	public void Active(bool isOn)
	{
		_nowWallState = eCharacterState.Idle;
		_nowObstacleState = eCharacterState.Idle;
		gameObject.SetActive(isOn);
		_character.SetActive(isOn);
		ResetPosition();
		_isSuper = true;
	}
	public void ResetPosition()
	{
		_point = Vector2.zero;
	}
	public void Initialize()
	{
		_material = GetComponentsInChildren<MeshRenderer>();
		_animController = GetComponentInChildren<Animator>();
		_rigid = transform.GetChild(0).GetComponent<Rigidbody>();
		_accessaryPos = transform.GetChild(0).GetChild(0).GetChild(0);
		_point = Vector2.zero;
		_shadow = transform.GetChild(1).gameObject;
		_material[(int)eDictionaryCategory.Character].material = PrefabManager._instance.GetCharacterMaterial((eCharacter)BackendGameData.Instance.UserGameData.selectBody);
		_material[(int)eDictionaryCategory.Face].material = PrefabManager._instance.GetFaceMaterial((eFace)BackendGameData.Instance.UserGameData.selectFace);
		_nowWallState = eCharacterState.Idle;
		_nowObstacleState = eCharacterState.Idle;
		_isWallTrigger = false;
		_isObstacleTrigger = false;
		_notDoubleDie = false;
		_itemGetCount = 0;

		//CameraMove();
		//ShadowMove(); 
		StartCoroutine(SettingSpeed());

		_animController.Play("HardJiggle", -1, 0f);

		_isFireImmune = false;
		_isIceImmune = false;
		_isDeadImmune = false;


		switch ((eCharacter)BackendGameData.Instance.UserGameData.selectBody)
		{
			case eCharacter.Copper:         // 느린 시작
				_startSpeed *= 0.8f;
				break;

			case eCharacter.Emerald:        // 아이템 등장확률 증가
				InGameManager._instance._itemProbability = 16;
				break;

			case eCharacter.Metal:          // 화염 내성
				_isFireImmune = true;
				break;

			case eCharacter.RealGold:       // 아이템으로 먹는 스코어 + 2
				InGameManager._instance._itemScore += 2;
				break;

			case eCharacter.Ruby:           // 작은 크기
				gameObject.transform.localScale *= 0.8f;
				break;

			case eCharacter.Sapphire:       // 얼음 내성
				_isIceImmune = true;
				break;

			case eCharacter.Silver:         // 데드 히트박스 한번 면역
				_isDeadImmune = true;
				break;
		}

		Instantiate(PrefabManager._instance.GetAccessary((eAccessory)BackendGameData.Instance.UserGameData.selectAccessory), _accessaryPos);
	}

	/// <summary>
	/// 앞으로 이동
	/// </summary>
	/// <param name="time"> 이동할 거리 계산을 위한 시간 </param>
	/// <returns> 이동한 거리 </returns>
	public float ForwardMove(float time)
	{
		_speed += _accelration * time;
		float moveDis = _speed * time;
		transform.position += Vector3.forward * moveDis;
		CameraMove();
		MoveEffect();
		ShadowMove();
		return moveDis;
	}
	public void HitCollider()
	{
		if (_ObstacleState == eCharacterState.Idle || _notDoubleDie) return;

		if (_isSuper)
		{
			SoundManager._instance.PlayEffect(eEffectSound.TouchToCharacter);
			if (InGameManager._instance._obstacleHitInform != eDebuffState.Oil)
				StartCoroutine(EffectManager._instance.PlayEffectOneShotPos(eParticle.Bounce, transform.position));
			StartCoroutine(Bounce());
			return;
		}

		switch (InGameManager._instance._obstacleHitInform)
		{
			case eDebuffState.Bounce:
				if (_ObstacleState == eCharacterState.Bounce)
				{
					if (!_isBounce)
					{
						StartCoroutine(EffectManager._instance.PlayEffectOneShotPos(eParticle.Bounce, transform.position));
						StartCoroutine(Bounce());
					}
				}
				else
				{
					if (_isShield)
					{
						ShieldOut();
					}
					else if (_isDeadImmune)
					{
						DeadImmune();
					}
					else
					{
						BrokenDead();
					}
				}
				break;

			case eDebuffState.SawBlade:
			case eDebuffState.OnlyDead:
				if (_isShield)
				{
					ShieldOut();
					break;
				}
				else if (_isDeadImmune)
				{
					DeadImmune();
					break;
				}
				else
				{
					BrokenDead();
				}
				break;

			case eDebuffState.Fire:
				//if (_isFireImmune)
				//{
				//	break;
				//}
				if (_isShield)
				{
					ShieldOut();
				}
				else
				{
					StartCoroutine(EffectManager._instance.PlayEffectOneShot(eParticle.Fire, transform));
					StartCoroutine(Fire());
				}
				break;

			case eDebuffState.Electronic:
				if (_isShield)
				{
					ShieldOut();
					break;
				}
				StartCoroutine(EffectManager._instance.PlayEffectOneShot(eParticle.Electric, transform));
				Electric();
				break;

			case eDebuffState.Ice:
				if (_ObstacleState == eCharacterState.Bounce)
				{
					if (_isIceImmune) break;
					StartCoroutine(Bounce());
					StopCoroutine(EffectManager._instance.Ice());
					StartCoroutine(EffectManager._instance.Ice());
				}
				else
				{
					if (_isShield)
					{
						ShieldOut();
						break;
					}
					else if (_isDeadImmune)
					{
						DeadImmune();
						break;
					}
					BrokenDead();
				}
				break;

			case eDebuffState.Oil:
				SoundManager._instance.PlayEffect(eEffectSound.Oil);
				StartCoroutine(EffectManager._instance.PlayEffectOneShot(eParticle.Oil, Camera.main.transform, 3));
				StartCoroutine(EffectTurn());
				break;
		}
	}
	public void HitWall()
	{
		if (_wallState == eCharacterState.Idle || _notDoubleDie) return;

		if (_isSuper)
		{
			if (_nowWallState != eCharacterState.Bounce)
			{
				StartCoroutine(Bounce());
				StartCoroutine(EffectManager._instance.PlayEffectOneShotPos(eParticle.Bounce, transform.position));
			}
			return;
		}

		switch (InGameManager._instance._wallHitInform)
		{
			case eDebuffState.Bounce:
				if (!_isBounce)
				{
					StartCoroutine(Bounce());
					StartCoroutine(EffectManager._instance.PlayEffectOneShotPos(eParticle.Bounce, transform.position));
				}
				break;

			case eDebuffState.Fire:
				if (_isFireImmune)
				{
					SoundManager._instance.PlayEffect(eEffectSound.TouchToCharacter);
				}
				else if (_isShield)
				{
					ShieldOut();
				}
				else
				{
					StartCoroutine(EffectManager._instance.PlayEffectOneShot(eParticle.Fire, transform));
					StartCoroutine(Fire());
				}
				break;

			case eDebuffState.Ice:
				if (_isIceImmune) break;

				if (EffectManager._instance._ableFrost)
				{
					StartCoroutine(Bounce());
					StopCoroutine(EffectManager._instance.Ice());
					StartCoroutine(EffectManager._instance.Ice());
				}
				break;
		}
	}
    #endregion [ 외부 함수 ]

    #region [ 상태 함수 ]
    public void Move(Vector2 _point)
    {

		//Vector2 direction = Vector2.zero;
		////#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		//if (Input.GetKey(KeyCode.UpArrow))
		//{
		//	direction += Vector2.up * 2;
		//}
		//if (Input.GetKey(KeyCode.DownArrow))
		//{
		//	direction += Vector2.down * 2;
		//}
		//if (Input.GetKey(KeyCode.LeftArrow))
		//{
		//	direction += Vector2.left * 2;
		//}
		//if (Input.GetKey(KeyCode.RightArrow))
		//{
		//	direction += Vector2.right * 2;
		//}
		//_player.Move(direction);

		Vector2 destination = (Vector2)transform.position + _point.normalized * 0.1f;
        Debug.Log(_point);

        transform.position = new Vector3(destination.x, destination.y, transform.position.z);
    }

    /// <summary>
    /// 상하좌우 이동
    /// </summary>
    public void Move()
    {
        //마우스 커서 위치 가져오기
        if (_point == Vector2.zero)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            _point = new Vector2(Input.mousePosition.x - Screen.currentResolution.width, Input.mousePosition.y - Screen.currentResolution.height);
            //_circlePos.SetCirclePos(Input.mousePosition);
#elif UNITY_ANDROID || UNITY_IOS
    			_point = new Vector2(Input.GetTouch(0).position.x - Screen.currentResolution.width / 2, Input.GetTouch(0).position.y - Screen.currentResolution.height);
    			//_circlePos.SetCirclePos(Input.GetTouch(0).position);
#endif
            _point.x /= Screen.currentResolution.width;
            _point.y /= Screen.currentResolution.width;
            _fixPosition = transform.position;
        }
        else
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            Vector2 tmp = new Vector2(Input.mousePosition.x - Screen.currentResolution.width, Input.mousePosition.y - Screen.currentResolution.height);
#elif UNITY_ANDROID || UNITY_IOS
    		Vector2 tmp = new Vector2(Input.GetTouch(0).position.x - Screen.currentResolution.width / 2, Input.GetTouch(0).position.y - Screen.currentResolution.height);
#endif
            tmp.x /= Screen.currentResolution.width;
            tmp.y /= Screen.currentResolution.width;

            tmp = (tmp - _point) * 2 * _offsetZ;

            Vector2 destination = _fixPosition + tmp;
            if (Mathf.Pow(destination.x, 2) + Mathf.Pow(destination.y, 2) >= _radius * _radius)
            {
                // 벽에 맞은 상태
                _bounceVec = -destination.normalized;
                destination = destination.normalized * (_radius - 0.1f);
                _nowWallState = eCharacterState.Bounce;
                _isWallTrigger = true;
            }
            transform.position = new Vector3(destination.x, destination.y, transform.position.z);
        }
    }
    #region [충돌 시 효과]
    // 장애물
    public IEnumerator Bounce()
	{
		_nowWallState = eCharacterState.Idle;
		_nowObstacleState = eCharacterState.Idle;
		_isBounce = true;
		EffectManager._instance.Vibrate();
		SoundManager._instance.PlayEffect(eEffectSound.TouchToCharacter);
		RotateCharacter(_bounceVec);
		_animController.Play("HardJiggle", -1, 0f);
		transform.position += _bounceVec / 10;

		yield return new WaitForSeconds(0.5f);

		_isBounce = false;

    }
    public void BrokenDead()
	{
		SoundManager._instance.PlayEffect(eEffectSound.Dead);
		_character.SetActive(false);
		_shadow.SetActive(false);
		EffectManager._instance.Vibrate();
		StartCoroutine(EffectManager._instance.PlayEffectOneShot(eParticle.Hit, transform));
		SoundManager._instance.PlayEffect(eEffectSound.Dead);
		InGameManager._instance.ProgEnd(2);
	}
	public IEnumerator Fire()
	{
		_notDoubleDie = true;
		StartCoroutine(EffectManager._instance.PlayEffectOneShot(eParticle.Fire, transform));
		SoundManager._instance.PlayEffect(eEffectSound.Fire);
		yield return new WaitForSeconds(1);
		_character.SetActive(false);
		_notDoubleDie = false;
		_shadow.SetActive(false);
		InGameManager._instance.ProgEnd(1);
	}
	public void Electric()
	{
		SoundManager._instance.PlayEffect(eEffectSound.Electric);
		InGameManager._instance.ProgEnd(2);
		_character.SetActive(false);
		_shadow.SetActive(false);
	}
	public IEnumerator EffectTurn()
	{
		yield return null;
		_nowObstacleState = eCharacterState.Idle;
	}
	// 아이템
	public IEnumerator Super(float time)
	{
		_isSuper = true;
		// 무적 시 작동할 것
		yield return new WaitForSeconds(time);
		_isSuper = false;
		_nowWallState = eCharacterState.Idle;
		_nowObstacleState = eCharacterState.Idle;
	}
	public IEnumerator Magnet(int time)
	{
		if (_isMagnet)
		{
			_isDoubleMagnet = true;
		}
		_isMagnet = true;
		_itemCollider.radius = 10;

		// 마그넷 시 작동할 것
		yield return new WaitForSeconds(time);

		if (_isDoubleMagnet)
		{
			_isDoubleMagnet = false;
		}
		else
		{
			_isMagnet = false;
			_itemCollider.radius = 0.5f;
		}
	}
	public IEnumerator Shield(int time)
	{
		if (_isShield)
		{
			_isDoubleShield = true;
		}
		_isShield = true;

		// 쉴드 시 작동할 것
		yield return new WaitForSeconds(time);

		if (_isDoubleShield)
		{
			_isDoubleShield = false;
		}
		else
		{
			_isShield = false;
		}
	}
	public void ShieldOut()
	{
		_nowWallState = eCharacterState.Idle;
		_nowObstacleState = eCharacterState.Idle;

		EffectManager._instance.EffectDisActive(eParticle.Shield);
		StartCoroutine(Bounce());
		StartCoroutine(EffectManager._instance.PlayEffectOneShot(eParticle.DeadImmune, transform));
		SoundManager._instance.PlayEffect(eEffectSound.DeadImmune);
		StartCoroutine(Super(0.5f));

		_isShield = false;
		_isDoubleShield = false;
	}
	public IEnumerator SlowTime(float time)
	{
		_speed -= time;         // 100 + time
		float baseAcc = _speed;	// 100

		_speed *= 0.7f;	//70

		while (_speed <= baseAcc)
		{
			yield return new WaitForSeconds(1); //(총 시간15초 * [2 (가속도 * 100)]) = 30 * 코루틴 시간 1
			baseAcc += _accelration;
			_speed += (baseAcc / 50);
		}
		_speed = baseAcc;
	}
	// 특성
	public void DeadImmune()
	{
		_isDeadImmune = false;
		_mainCanvas.SetDeadImmune(false);
		_nowWallState = eCharacterState.Idle;
		_nowObstacleState = eCharacterState.Idle;

		StartCoroutine(Bounce());
		StartCoroutine(EffectManager._instance.PlayEffectOneShot(eParticle.DeadImmune, transform));
		SoundManager._instance.PlayEffect(eEffectSound.DeadImmune);
		StartCoroutine(Super(0.5f));
	}
	#endregion [충돌 시 효과]

	IEnumerator SettingSpeed()
	{
		_startSpeed = 0;
		_speed = 0;

		if (BackendGameData.Instance.UserGameData.selectMode == (int)eLevelMode.Infinite)
		{
			switch ((eInfiniteLevel)BackendGameData.Instance.UserGameData.selectInfiniteLevel)
			{
				case eInfiniteLevel.NORMAL:
					_startSpeed = 50;
					_accelration = 0.25f;
					break;

				case eInfiniteLevel.FAST:
					_startSpeed = 65;
					_accelration = 0.26f;
					break;

				case eInfiniteLevel.HARD:
					_startSpeed = 70;
					_accelration = 0.27f;
					break;
			}
		}
		else
		{
			switch ((eTutorialLevel)BackendGameData.Instance.UserGameData.selectTutorialLevel)
			{
				case eTutorialLevel.STORY1:
					_startSpeed = 35;
					_accelration = 0.22f;
					break;

				case eTutorialLevel.STORY2:
					_startSpeed = 40;
					_accelration = 0.23f;
					break;

				case eTutorialLevel.STORY3:
					_startSpeed = 45;
					_accelration = 0.24f;
					break;
			}
		}
		while (_speed < _startSpeed)
		{
			if(InGameManager._instance._nowProgress == eProgress.Play)
				_speed += _startSpeed * Time.deltaTime;
			yield return null;
		}

		_speed = _startSpeed;
	}
	#endregion [ 상태 함수 ]

	#region [ 내부 함수 ]
	void CameraMove()
	{
		Vector2 _vec = Vector2.Lerp(Camera.main.transform.position, transform.position / _mulVal, 0.2f);

		if(transform.position.z - Camera.main.transform.position.z < _offsetZ)
			Camera.main.transform.position = new Vector3(_vec.x, _vec.y, -10);
		else
			Camera.main.transform.position = new Vector3(_vec.x, _vec.y, transform.position.z - _offsetZ);

		Camera.main.transform.LookAt(new Vector3(transform.position.x - _vec2.x, transform.position.y - _vec2.y, transform.position.z));
		//Camera.main.transform.LookAt(_vec2);
	}
	void _CameraMove()
	{
		Camera.main.transform.position = new Vector3(transform.position.x / _mulVal, transform.position.y / _mulVal, transform.position.z - _offsetZ);
		Camera.main.transform.LookAt(transform.position);
	}
	void MoveEffect()
	{
		var emission = _lightLine.emission;
		emission.rateOverTime = _nowSpeed;

		if (_nowSpeed >= 100)
		{
			emission = _focusLine.emission;
			emission.rateOverTime = _nowSpeed - 100;
		}
	}
	void ShadowMove()
	{
		float degree = Mathf.Sqrt((Mathf.Pow(transform.position.x, 2) + Mathf.Pow(transform.position.y, 2)));
		Vector3 scale = Vector3.one * SHADOW_SIZE * ((degree + _shadowOffset < _radius ) ? 0 : degree - _radius + _shadowOffset);
		_shadow.transform.localScale = scale;
		_shadow.transform.position = transform.position + (transform.position - Vector3.forward * transform.position.z).normalized;
		_shadow.transform.LookAt(transform.position);
	}
	public void RotateCharacter(Vector2 vec)
	{
		_rigid.angularVelocity = Vector3.zero;
		_rigid.AddTorque(new Vector2(vec.y, -vec.x) , ForceMode.Impulse);
	}
	#endregion [ 내부 함수 ]
}
