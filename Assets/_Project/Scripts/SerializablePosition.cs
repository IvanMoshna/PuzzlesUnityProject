using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializablePosition
{
    public float x;
    public float y;

    public SerializablePosition(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}
