using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollision : MonoBehaviour
{
    //1. 나혹은 상대에게rigidBody가 있어야 한다. (대신 isKinemetic은 off)
    //2. 나에게 collider가 있어야 한다.(대신 isTrigger는 off)
    //3. 상대에게 collider가 있어야 한다.(대신 isTrigger는 off)
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collision @{collision.gameObject.name}");
    }
    //1. 둘다 collider가 있어야 한다.
    //2. 둘중 하나는 isTrigger: on
    //3. 둘중 하나는 RigitBody가 있어야 한다
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
