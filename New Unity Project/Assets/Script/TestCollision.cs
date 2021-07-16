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
        Vector3 look = transform.TransformDirection(Vector3.forward);
        
        Debug.DrawRay(transform.position + Vector3.up, look * 10, Color.red);
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position + Vector3.up, look, 10);

        foreach (RaycastHit hit in hits)
        {

            Debug.Log($"Raycast {hit.collider.gameObject.name} !");
        }
    }
}
