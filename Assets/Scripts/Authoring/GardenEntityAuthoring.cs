using System;
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
        switch (entityType)
        {
            case EntityType.WORM:
                dstManager.AddComponentData(entity, new Worm());
                break;
            case EntityType.LEAF:
                dstManager.AddComponentData(entity, new Leaf());
                break;
        }
    }
}
