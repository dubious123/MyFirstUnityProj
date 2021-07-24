using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Layer
    {
        Monster = 6,
        Ground = 7,
        Block = 8,

    }
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
        PointerDown,
        PointerUp,
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
