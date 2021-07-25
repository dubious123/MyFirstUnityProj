using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//1. 위치벡터
//2. 방향벡터
public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
        Skill,
    }
    int _mask = (1 << (int)Define.Layer.Ground | 1 << (int)Define.Layer.Monster);

    PlayerStat _stat;
    Vector3 _destPos;
    [SerializeField]
    PlayerState _state = PlayerState.Idle;
    GameObject _lockTarget;


    bool _stopSkill = false;
    private void OnMouseEvent(Define.MouseEvent evt)
    {
        //World좌표계 <-> Local 좌표계 <-> ViewPort 좌표계 <-> Screen 좌표계
        /*Debug.Log( Input.mousePosition);*/ //스크린 좌표
                                             //Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition)); //ViewPoint

        switch (State)
        {
            case PlayerState.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case PlayerState.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case PlayerState.Skill:
                {
                    if(evt == Define.MouseEvent.PointerUp)
                    {
                        _stopSkill = true;
                    }
                }
                break;
            default:
                break;

        }

    }

    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out RaycastHit hit, 100.0f, _mask);
        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    if (raycastHit)
                    {
                        _destPos = hit.point;
                        State = PlayerState.Moving;
                        _stopSkill = false;
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
                    if (_lockTarget == null && raycastHit)
                    {
                        _destPos = hit.point;
                    }
                }
                break;
            case Define.MouseEvent.PointerUp:
                _stopSkill = true;
                break;
            default:
                break;
        }
    }





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
                    anim.CrossFade("RUN", 0.1f);
                    break;
                case PlayerState.Idle:
                    anim.CrossFade("WAIT", 0.1f);
                    break;
                case PlayerState.Skill:
                    anim.CrossFade("ATTACK", 0.1f, -1, 0);
                    break;
                default:
                    break;

            }

        }
    }
    void Start()
    {

        _stat = gameObject.GetComponent<PlayerStat>();
        //방향벡터 : 거리, 방향
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    void UpdateIdle()
    {
        //애니메이션
        Animator anim = GetComponent<Animator>();
        //게임 상태에 대한 정보를 넘겨준다
    }
    void UpdateDie()
    {

    }
    void UpdateMoving()
    {
        if(_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            if(distance <= 1)
            {
                State = PlayerState.Skill;
                return;
            }
        }
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.01f)
        {
            State = PlayerState.Idle;
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
                    State = PlayerState.Idle;
                }
                return;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
        //애니메이션
    }
    void UpdateSkill()
    {
        if(_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    void OnHitEvent()
    {
        if(_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            Stat myStat = gameObject.GetComponent<PlayerStat>();
            int damage = Mathf.Max(0, myStat.Attack - targetStat.Defense);
            Debug.Log(damage);
            targetStat.Hp -= damage;
        }
        if (_stopSkill)
        {
            State = PlayerState.Idle;
        }
        else
        {
            State = PlayerState.Skill;
        }

    }
    void Update()
    {
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
        switch (State)
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
