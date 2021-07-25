using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//1. ��ġ����
//2. ���⺤��
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
        //World��ǥ�� <-> Local ��ǥ�� <-> ViewPort ��ǥ�� <-> Screen ��ǥ��
        /*Debug.Log( Input.mousePosition);*/ //��ũ�� ��ǥ
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
        //���⺤�� : �Ÿ�, ����
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    void UpdateIdle()
    {
        //�ִϸ��̼�
        Animator anim = GetComponent<Animator>();
        //���� ���¿� ���� ������ �Ѱ��ش�
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
        //�ִϸ��̼�
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
        //���� ȸ����
        //transform.eulerAngles = new Vector3(0.0f, _yAngle, 0.0f);
        //+-delta


        //ȸ�� : eular  :  �߰��� ���� �̻��ϰԵ�, gimble lock(local �������� ������ ���� ��),
        //quaternion : (w, (x,y,z)) ����, ���ͺκ��� ��� ��� 
        //�߰����� �̻��� ��ȯ�� ���� ��鸲 ���� ������ �ȴ�. (slerp)
        //������ ������� ������ ���ͷε� ǥ�� ����
        //����?
        // transform.rotation : ��ȯ���� quaternion ������̴�.
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
