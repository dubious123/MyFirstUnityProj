using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Managers : MonoBehaviour
{
    static Managers s_Instance;
    public static Managers Instance { get { Init(); return s_Instance; } }

    private DataManager _data = new DataManager();
    private InputManager _input = new InputManager();
    private PoolManager _pool = new PoolManager();
    private ResourceManager _resource = new ResourceManager();
    private SceneManagerEx _scene = new SceneManagerEx();
    private SoundManager _sound = new SoundManager();
    private UI_Manager _ui = new UI_Manager();

    public static DataManager Data { get { return Instance._data; } }
    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UI_Manager UI { get { return Instance._ui; } }
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
            s_Instance._data.Init();
            s_Instance._sound.Init();
            s_Instance._pool.Init();
        }
    }

    public static void Clear()
    {
        Input.Clear();
        Sound.Clear();
        Scene.Clear();
        UI.Clear();

        Pool.Clear();
    }
}
