using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollision : MonoBehaviour
{
    //1. ������ rigidBody�� �־�� �Ѵ�. (��� isKinemetic�� off)
    //2. ������ collider�� �־�� �Ѵ�.(��� isTrigger�� off)
    //3. ��뿡�� collider�� �־�� �Ѵ�.(��� isTrigger�� off)
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
