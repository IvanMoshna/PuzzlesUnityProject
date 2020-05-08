using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableVector 
{
    public float x;
    public float y;

    public SerializableVector(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}
