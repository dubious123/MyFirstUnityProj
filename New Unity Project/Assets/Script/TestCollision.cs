using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollision : MonoBehaviour
{
    //1. ��Ȥ�� ��뿡��rigidBody�� �־�� �Ѵ�. (��� isKinemetic�� off)
    //2. ������ collider�� �־�� �Ѵ�.(��� isTrigger�� off)
    //3. ��뿡�� collider�� �־�� �Ѵ�.(��� isTrigger�� off)
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision @{collision.gameObject.name}");
    }
    //1. �Ѵ� collider�� �־�� �Ѵ�.
    //2. ���� �ϳ��� isTrigger: on
    //3. ���� �ϳ��� RigitBody�� �־�� �Ѵ�
    private void OnTriggerEnter(Collider other)
    {
        
        Debug.Log($"Trigger @ {other.gameObject.name} !");
    }
    void Start()
    {
        
    }

    void Update()
    {

        //World��ǥ�� <-> Local ��ǥ�� <-> ViewPort ��ǥ�� <-> Screen ��ǥ��
        /*Debug.Log( Input.mousePosition);*/ //��ũ�� ��ǥ
        Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition)); //ViewPoint
    }
}
