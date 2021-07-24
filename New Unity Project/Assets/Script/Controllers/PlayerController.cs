using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//1. ��ġ����
//2. ���⺤��
public class PlayerController : MonoBehaviour
{
    PlayerStat _stat;
    Vector3 _destPos;
    void Start()
    {

        _stat = gameObject.GetComponent<PlayerStat>();
        //���⺤�� : �Ÿ�, ����
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;


    }
    int _mask = (1 << (int)Define.Layer.Ground | 1 << (int)Define.Layer.Monster);

    GameObject _lockTarget;
    private void OnMouseEvent(Define.MouseEvent evt)
    {
        //World��ǥ�� <-> Local ��ǥ�� <-> ViewPort ��ǥ�� <-> Screen ��ǥ��
        /*Debug.Log( Input.mousePosition);*/ //��ũ�� ��ǥ
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
        //�ִϸ��̼�
        Animator anim = GetComponent<Animator>();
        //���� ���¿� ���� ������ �Ѱ��ش�
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
        //�ִϸ��̼�
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
