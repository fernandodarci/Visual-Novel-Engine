using System;
using UnityEngine;

[Serializable]
public class GSC_PositionSize
{
    public Vector2 Position;
    public Vector2 Size;

    public GSC_PositionSize(Vector2 position, Vector2 size)
    {
        Position = position;
        Size = size;
    }
}


