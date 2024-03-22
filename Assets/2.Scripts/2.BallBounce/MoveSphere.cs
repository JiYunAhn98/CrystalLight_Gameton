using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSphere : MonoBehaviour
{
    float MaxLeftAngle = 0.8f;
    float MaxRightAngle = -0.8f;

    [Header("Controller")]
    [SerializeField] float _moveSpeed = 20;

    [Header("Don't Touch")]
    [SerializeField] Texture2D _coloredLineTexture;

    // 참조 변수
    Rigidbody _rigid;

    // 상태 변수
    Transform _originPos;
    Vector3 _startPos;
    Vector3 _moveDir;
    Vector3 _subDir;


    void Start()
    {
        _originPos = GameObject.FindGameObjectWithTag("Player").transform.GetChild(2).transform;
        _startPos = transform.position;
        _rigid = GetComponent<Rigidbody>();
        _subDir = Vector3.forward;
        _moveDir = Vector3.zero;
    }
    private void Update()
    {
        if (_moveDir == Vector3.zero)
        {
            Debug.DrawLine(transform.position, transform.position + _subDir * 5, Color.green);
            transform.position = _originPos.position;
            // Space : 발사, Z :  오른쪽 이동, C : 왼쪽 이동
            // 반원을 기준으로 움직이도록 한다.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _moveDir = _subDir.normalized;
            }
            else if (Input.GetKey(KeyCode.Z))
            {
                if (_subDir.x < MaxLeftAngle) _subDir = Quaternion.Euler(0, 0.1f, 0) * _subDir;
            }
            else if (Input.GetKey(KeyCode.C))
            {
                if (_subDir.x > MaxRightAngle) _subDir = Quaternion.Euler(0, -0.1f, 0) * _subDir;
            }
        }
    }

    void FixedUpdate()
    {
        if (null != _rigid && _moveDir != Vector3.zero)
        {
            _rigid.velocity = _moveDir * _moveSpeed;
            Debug.Log(_rigid.velocity);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        // 충돌 지점
        Vector3 hitPos = collision.contacts[0].point;

        // 벽에 충돌시 튕기는용 입 사각, 반사각
        Vector3 incomingVec = hitPos - _startPos;
        Vector3 reflectVec = Vector3.Reflect(incomingVec, collision.contacts[0].normal);

        _moveDir = reflectVec.normalized + Physics.gravity.normalized;
        _startPos = transform.position;

#if UNITY_EDITOR
        Debug.DrawLine(_startPos, hitPos, Color.blue);
        Debug.DrawLine(hitPos, reflectVec, Color.green);
#endif

        if (collision.gameObject.tag == "enemy")
        {
            Debug.Log("Boss Hit");
        }
        if (collision.gameObject.tag == "Finish" || collision.gameObject.tag == "Player")
        {
            transform.position = _originPos.position;
            _moveDir = Vector3.zero;
        }
    }
    

    //private void OnGUI()
    //{
    //    DrawLine(transform.position, _subDir, 2);
    //}
    //private void DrawLine(Vector2 start, Vector2 end, int width)
    //{
    //    Vector2 d = end - start;
    //    float a = Mathf.Rad2Deg * Mathf.Atan(d.y / d.x);
    //    if (d.x < 0)
    //        a += 180;

    //    int width2 = (int)Mathf.Ceil(width / 2);

    //    GUIUtility.RotateAroundPivot(a, start);
    //    GUI.DrawTexture(new Rect(start.x, start.y - width2, d.magnitude, width), _coloredLineTexture);
    //    GUIUtility.RotateAroundPivot(-a, start);
    //}
}
