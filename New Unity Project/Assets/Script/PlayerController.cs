using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1. ��ġ����
//2. ���⺤��
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.0f;
    void Start()
    {
        //���⺤�� : �Ÿ�, ����


    }
    float _yAngle = 0.0f;

    private void OnKeyboard()
    {
        if (Input.GetKey(KeyCode.W))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.1f);
            transform.position += Vector3.forward * Time.deltaTime * _speed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.back);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.1f);
            transform.position += Vector3.back * Time.deltaTime * _speed;

        }
        if (Input.GetKey(KeyCode.A))
        {

            //transform.rotation = Quaternion.LookRotation(Vector3.left);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.1f);
            transform.position += Vector3.left * Time.deltaTime * _speed;
        }
        if (Input.GetKey(KeyCode.D))
        {
            //transform.rotation = Quaternion.LookRotation(Vector3.right);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.1f);
            transform.position += Vector3.right * Time.deltaTime * _speed;
        }
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
        Managers.Input.KeyAction -= OnKeyboard;
        Managers.Input.KeyAction += OnKeyboard;
    }
}
