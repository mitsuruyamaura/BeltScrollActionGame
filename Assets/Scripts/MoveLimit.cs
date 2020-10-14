using System;
using UnityEngine;

[Serializable]
public class MoveLimit
{
    [Header("横方向")]
    public HorizontalLimit horizontalLimit;
    [Header("奥行方向")]
    public DepthLimit depthLimit;

    [Serializable]
    public class HorizontalLimit {
        [Header("左制限")]
        public float left;
        [Header("右制限")]
        public float right;
    }

    [Serializable]
    public class DepthLimit {
        [Header("手前制限")]
        public float forword;
        [Header("奥制限")]
        public float back;
    }
}
