using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{

    public enum Scene
    {
        Unknown,
        Login,
        Loby,
        Game
    }
    public enum Sound
    {
        Bgm,
        Effect,
        MaxCount
    }
    public enum MouseEvent
    {
        Press,
        Click,
    }
    public enum CameraMode
    {
        QuaterView,
    }
    public enum UIEvent
    {
        Click,
        Drag,

    }
}
