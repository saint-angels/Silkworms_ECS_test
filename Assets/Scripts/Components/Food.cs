using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct Food : ComponentWithValue
{
    public int Value
    {
        get => value;
        set => this.value = value;
    }

    public int value;
}
