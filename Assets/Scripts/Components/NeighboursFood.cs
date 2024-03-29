﻿using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct NeighboursFood : ComponentWithValue
{
    public int Value
    {
        get => value;
        set => this.value = value;
    }

    private int value;
}
