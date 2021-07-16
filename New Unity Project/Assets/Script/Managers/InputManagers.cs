using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class InputManagers
{
    //��ǥ���� Listener ���
    //key�� ������ �ʿ��� �ֵ鿡�� event�� �Ѹ��ž�
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
