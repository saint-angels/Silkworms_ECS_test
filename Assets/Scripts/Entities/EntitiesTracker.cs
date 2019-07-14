using System;
using System.Collections;
using System.Collections.Generic;
using Components;
using Entities;
using UnityEngine;
using UnityEngine.UIElements;

public enum EntityType
{
    WORM,
    LEAF
}

public class EntitiesTracker : MonoBehaviour
{
    [System.Serializable]
    struct EntityPrefabSetting
    {
        public EntityType type;
        public EntityBase entityPrefab;
    }

    [SerializeField] private EntityPrefabSetting[] prefabSettings = null;
    
    
    private Dictionary<EntityType, List<EntityBase>> spawnedEntities = new Dictionary<EntityType, List<EntityBase>>();

    private List<CompFood> allFood = new List<CompFood>();


    public List<CompFood> GetFood()
    {
        return allFood;
    }

    public EntityBase Spawn(EntityType entityType)
    {
        EntityBase entityPrefab = null;
        
        for (int i = 0; i < prefabSettings.Length; i++)
        {
            if (prefabSettings[i].type == entityType)
            {
                entityPrefab = prefabSettings[i].entityPrefab;
                break;
            }
        }

        if (entityPrefab == null)
        {
            Debug.LogError($"Can't find prefab for entity {entityType}");
            return null;
        }

        var newEntity = ObjectPool.Spawn(entityPrefab, Vector3.zero, Quaternion.identity);
        newEntity.Init();
        
        Root.UIManager.SetHUDForEntity(newEntity);

        if (spawnedEntities.ContainsKey(entityType) == false)
        {
            spawnedEntities.Add(entityType, new List<EntityBase>());
        }
        spawnedEntities[entityType].Add(newEntity);

        newEntity.OnDeath += EntityOnOnDeath;


        switch (entityType)
        {
            case EntityType.WORM:
                break;
            case EntityType.LEAF:
                CompFood foodPiece = newEntity.GetComponent<CompFood>();
                allFood.Add(foodPiece);
                foodPiece.OnFinished += FoodOnFinished;
                
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(entityType), entityType, null);
        }
        
        return newEntity;
    }

    private void FoodOnFinished(CompFood foodPiece)
    {
        foodPiece.OnFinished -= FoodOnFinished;
        
        allFood.Remove(foodPiece);
    }

    private void EntityOnOnDeath(EntityBase dyingEntity)
    {
        dyingEntity.OnDeath -= EntityOnOnDeath;
        spawnedEntities[dyingEntity.type].Remove(dyingEntity);
    }
}
