using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1. 위치벡터
//2. 방향벡터
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.0f;
    Vector3 _destPos;
    void Start()
    {
        //방향벡터 : 거리, 방향
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;

        //Temp
        UI_Button ui = Managers.UI.ShowPopupUI<UI_Button>();
        Managers.UI.ClosePopupUI(ui);
    }

    private void OnMouseClicked(Define.MouseEvent evt)
    {
        //World좌표계 <-> Local 좌표계 <-> ViewPort 좌표계 <-> Screen 좌표계
        /*Debug.Log( Input.mousePosition);*/ //스크린 좌표
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
        //애니메이션
        Animator anim = GetComponent<Animator>();
        anim.SetFloat("speed",_speed);
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
            default:
                break;
        }


    }
}
