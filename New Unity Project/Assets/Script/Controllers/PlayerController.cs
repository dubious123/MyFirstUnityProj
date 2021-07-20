using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1. ��ġ����
//2. ���⺤��
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.0f;
    Vector3 _destPos;
    void Start()
    {
        //���⺤�� : �Ÿ�, ����
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;

        //Temp
        UI_Button ui = Managers.UI.ShowPopupUI<UI_Button>();
        Managers.UI.ClosePopupUI(ui);
    }

    private void OnMouseClicked(Define.MouseEvent evt)
    {
        //World��ǥ�� <-> Local ��ǥ�� <-> ViewPort ��ǥ�� <-> Screen ��ǥ��
        /*Debug.Log( Input.mousePosition);*/ //��ũ�� ��ǥ
                                             //Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition)); //ViewPoint

        if(_state == PlayerState.Die)
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);
        if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, LayerMask.GetMask("Wall")))
        {
            _destPos = hit.point;
            _state= PlayerState.Moving;
        }
        
        //Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        //Vector3 dir = (mousePos - Camera.main.transform.position).normalized;
        //RaycastHit hit;
        //Debug.DrawRay(Camera.main.transform.position, dir*100.0f, Color.red, 1.0f);
        //if (Physics.Raycast(Camera.main.transform.position, dir, out hit, 100.0f))
        //{
        //    Debug.Log($"Raycast Camera @ {hit.collider.gameObject.name}");
        //}
        Debug.Log("OnMouseClicked !");
    }

    float _yAngle = 0.0f;



    enum PlayerState
    {
        Die,
        Moving,
        Idle,
    }
    PlayerState _state = PlayerState.Idle;
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
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.000001f)
        {
            _state = PlayerState.Idle;
        }
        else
        {
            transform.position += dir.normalized * Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
        //�ִϸ��̼�
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed",_speed);
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
            default:
                break;
        }


    }
}
