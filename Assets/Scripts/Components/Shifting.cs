using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct Shifting : IComponentData
{
    public float speed;
    public float distanceLeft;
    public float2 direction;
}
