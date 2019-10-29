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
        dstManager.AddComponentData(entity, new NeighboursWorm());
        dstManager.AddComponentData(entity, new NeighboursFood());
        dstManager.AddComponent<GridPosition>(entity);
        
        switch (entityType)
        {
            case EntityType.EARTH:
                dstManager.AddComponentData(entity, new Earth());
                break;
            case EntityType.WORM:
                dstManager.AddComponentData(entity, new Worm{ Value = 100});
                break;
            case EntityType.FOOD:
                dstManager.AddComponentData(entity, new Food {Value = 100});
                break;
        }
    }
}
