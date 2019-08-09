﻿using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[RequiresEntityConversion]

public class GardenEntityAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public EntityType entityType;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new NeighboursWorm());
        
        switch (entityType)
        {
            case EntityType.EARTH:
                dstManager.AddComponentData(entity, new Earth());
                break;
            case EntityType.WORM:
                dstManager.AddComponentData(entity, new Worm());
                break;
            case EntityType.LEAF:
                dstManager.AddComponentData(entity, new Leaf());
                break;
        }
    }
}