using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class InputManagers
{
    //대표적인 Listener 방식
    //key가 눌리면 필요한 애들에게 event로 뿌릴거야
    public Action KeyAction = null;

    public void OnUpdate()
    {
        if (Input.anyKey == false)
        {
            return;
        }
        if(KeyAction != null)
        {
            KeyAction.Invoke();
        }
    }
}
