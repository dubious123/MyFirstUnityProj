using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    PlayerStat _stat;
    [SerializeField]
    protected Vector3 _destPos;
    [SerializeField]
    protected Define.State _state = Define.State.Idle;
    [SerializeField]
    protected GameObject _lockTarget;

    public Define.WorldObject worldObjectType { get; protected set; } = Define.WorldObject.Unknown;
    public virtual Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;
            Animator anim = gameObject.GetComponent<Animator>();
            switch (_state)
            {
                case Define.State.Die:
                    break;
                case Define.State.Moving:
                    anim.CrossFade("RUN", 0.1f);
                    break;
                case Define.State.Idle:
                    anim.CrossFade("WAIT", 0.1f);
                    break;
                case Define.State.Skill:
                    anim.CrossFade("ATTACK", 0.1f, -1, 0);
                    break;
                default:
                    break;

            }

        }
    }
    private void Start()
    {
        Init();
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
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Moving:
                UpdateMoving();
                break;
            case Define.State.Skill:
                UpdateSkill();
                break;
            default:
                break;
        }


    }
    public abstract void Init();
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateDie() { }
    protected virtual void UpdateMoving() { }
    protected virtual void UpdateSkill() { }
}
