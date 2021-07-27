using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//1. À§Ä¡º¤ÅÍ
//2. ¹æÇâº¤ÅÍ
public class PlayerController : BaseController
{
    int _mask = (1 << (int)Define.Layer.Ground | 1 << (int)Define.Layer.Monster);

    PlayerStat _stat;

    bool _stopSkill = false;
    private void OnMouseEvent(Define.MouseEvent evt)
    {
        //WorldÁÂÇ¥°è <-> Local ÁÂÇ¥°è <-> ViewPort ÁÂÇ¥°è <-> Screen ÁÂÇ¥°è
        /*Debug.Log( Input.mousePosition);*/ //½ºÅ©¸° ÁÂÇ¥
                                             //Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition)); //ViewPoint

        switch (State)
        {
            case Define.State.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Skill:
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
                        State = Define.State.Moving;
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
    void OnHitEvent()
    {
        if (_lockTarget != null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);
        }
        if (_stopSkill)
        {
            State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Skill;
        }

    }
    public override void Init()
    {
        worldObjectType = Define.WorldObject.Player;
        _stat = gameObject.GetComponent<PlayerStat>();
        //¹æÇâº¤ÅÍ : °Å¸®, ¹æÇâ
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
        {
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
        }
    }
    protected override void UpdateMoving()
    {
        if(_lockTarget != null)
        {
            _destPos = _lockTarget.transform.position;
            float distance = (_destPos - transform.position).magnitude;
            if(distance <= 1)
            {
                State = Define.State.Skill;
                return;
            }
        }
        Vector3 dir = _destPos - transform.position;
        dir.y = 0;
        if (dir.magnitude < 0.01f)
        {
            State = Define.State.Idle;
        }
        else
        {
            Debug.DrawRay(transform.position, dir.normalized, Color.green);
            if(Physics.Raycast(transform.position + Vector3.up, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if (Input.GetMouseButton(0) == true)
                {
                    State = Define.State.Idle;
                }
                return;
            }
            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }
        //¾Ö´Ï¸ÞÀÌ¼Ç
    }
    protected override void UpdateSkill()
    {
        if(_lockTarget != null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }






}
