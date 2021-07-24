using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//1. 위치벡터
//2. 방향벡터
public class PlayerController : MonoBehaviour
{
    PlayerStat _stat;
    Vector3 _destPos;
    void Start()
    {

        _stat = gameObject.GetComponent<PlayerStat>();
        //방향벡터 : 거리, 방향
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;


    }
    int _mask = (1 << (int)Define.Layer.Ground | 1 << (int)Define.Layer.Monster);

    GameObject _lockTarget;
    private void OnMouseEvent(Define.MouseEvent evt)
    {
        //World좌표계 <-> Local 좌표계 <-> ViewPort 좌표계 <-> Screen 좌표계
        /*Debug.Log( Input.mousePosition);*/ //스크린 좌표
                                             //Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition)); //ViewPoint

        if(_state == PlayerState.Die)
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out RaycastHit hit, 100.0f, _mask);
        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    if (raycastHit)
                    {
                        _destPos = hit.point;
                        _state = PlayerState.Moving;
                        if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
                        {
                            _lockTarget = hit.collider.gameObject;
                        }
                        else
                        {
                            _lockTarget = null;
                        }
                    }
                }
                break;
            case Define.MouseEvent.Press:
                {
                    if(_lockTarget != null)
                    {
                        _destPos = _lockTarget.transform.position;
                    }
                    else if (raycastHit)
                    {
                        _destPos = hit.point;
                    }
                }
                break;
            default:
                break;
        }
    }

    float _yAngle = 0.0f;



    enum PlayerState
    {
        Die,
        Moving,
        Idle,
        Skill,
    }
    [SerializeField]
    PlayerState _state = PlayerState.Idle;

    public PlayerState State
    {
        get { return _state; }
        set
        {
            _state = value;
            Animator anim = gameObject.GetComponent<Animator>();
            switch (_state)
            {
                case PlayerState.Die:
                    break;
                case PlayerState.Moving:
                    break;
                case PlayerState.Idle:
                    break;
                case PlayerState.Skill:
                    break;
                default:
                    break;

            }

        }
    }
    void UpdateIdle()
    {
        //애니메이션
        Animator anim = GetComponent<Animator>();
        //게임 상태에 대한 정보를 넘겨준다
        anim.SetFloat("speed",0);
    }
    void UpdateDie()
    {

    }
    void UpdateMoving()
    {
        if(_lockTarget != null)
        {
            float distance = (_destPos - transform.position).magnitude;
            if(distance <= 1)
            {
                _state = PlayerState.Skill;
                return;
            }
        }
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.01f)
        {
            _state = PlayerState.Idle;
        }
        else
        {
            NavMeshAgent nma = gameObject.GetOrAddComponent<NavMeshAgent>();

            //nma.CalculatePath
            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            nma.Move(dir.normalized * moveDist);
            Debug.DrawRay(transform.position, dir.normalized, Color.green);
            if(Physics.Raycast(transform.position + Vector3.up, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if (Input.GetMouseButton(0) == true)
                {
                    _state = PlayerState.Moving;
                    return;
                }
                _state = PlayerState.Idle;
                return;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
        //애니메이션
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed", _stat.MoveSpeed);
    }
    void UpdateSkill()
    {
        Animator anim = gameObject.GetComponent<Animator>();
        anim.SetBool("attack", true);
    }

    void OnHitEvent()
    {
        Debug.Log("OnHitEvent");
        Animator anim = gameObject.GetComponent<Animator>();
        anim.SetBool("attack", false);
        _state = PlayerState.Idle;
    }
    void Update()
    {
        _yAngle += Time.deltaTime * 100.0f;
        //절대 회전값
        //transform.eulerAngles = new Vector3(0.0f, _yAngle, 0.0f);
        //+-delta


        //회전 : eular  :  중간값 보관 이상하게됨, gimble lock(local 방향으로 돌리고 싶을 때),
        //quaternion : (w, (x,y,z)) 형태, 벡터부분은 모두 허수 
        //중간값도 이상한 변환을 통해 흔들림 없이 보존이 된다. (slerp)
        //심지어 사원수를 사차원 벤터로도 표현 가능
        //만능?
        // transform.rotation : 반환값이 quaternion 사원수이다.
        //transform.Rotate(new Vector3(0.0f, Time.deltaTime * 100.0f, 0.0f));
        //transform.rotation = Quaternion.Euler(new Vector3(0.0f, _yAngle, 0.0f));
        //local -> world
        //TransformDirection
        //world -> local
        //InverseTransformDirection
        switch (_state)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Die:
                UpdateDie();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Skill:
                UpdateSkill();
                break;
            default:
                break;
        }


    }
}
