using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Managers : MonoBehaviour
{
    static Managers s_Instance;
    public static Managers Instance { get { Init(); return s_Instance; } }

    private InputManagers _input = new InputManagers();
    private ResourceManagers _resourceManagers = new ResourceManagers();
    public static InputManagers Input { get { return Instance._input; } }
    public static ResourceManagers Resource { get { return Instance._resourceManagers; } }
    void Start()
    {
        Init();
    }
    void Update()
    {
        _input.OnUpdate();
    }
    static void Init()
    {
        if(s_Instance == null)
        {

            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject("@Managers");
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            s_Instance = go.GetComponent<Managers>();
        }
    }
}
