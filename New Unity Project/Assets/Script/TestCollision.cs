using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollision : MonoBehaviour
{
    //1. 나에게 rigidBody가 있어야 한다. (대신 isKinemetic은 off)
    //2. 나에게 collider가 있어야 한다.(대신 isTrigger는 off)
    //3. 상대에게 collider가 있어야 한다.(대신 isTrigger는 off)
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision !");
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger !");
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
