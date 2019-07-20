using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct Shift : IComponentData
{
    public float3 startPosition;
    public float3 endPosition;
    
    public float duration;
    public float progress;
}
