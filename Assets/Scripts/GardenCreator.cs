using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEditor.U2D;
using UnityEngine;
using Random = System.Random;

public class GardenCreator : MonoBehaviour
{
    
    
    
    [SerializeField] private int width = 150;
    [SerializeField] private int height = 150;

    [SerializeField] private float cellScale = 1f;

    [SerializeField] private GameObject[] gameobjectsPrefabs;

    private EntityManager entityManager;

    
    
    private void Start()
    {
        entityManager = World.Active.EntityManager;

        Entity[] entitiesPrefabs = new Entity[gameobjectsPrefabs.Length];
        for (int i = 0; i < entitiesPrefabs.Length; i++)
        {
            entitiesPrefabs[i] = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameobjectsPrefabs[i], World.Active);
        }

        //Generate grid
        float3 startPoint = new float3(- (width / 2f) * cellScale, - (height / 2f) * cellScale, 0);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float3 position = new float3(x * cellScale + startPoint.x, y * cellScale + startPoint.y, 0);
                var randomPrefab = entitiesPrefabs[UnityEngine.Random.Range(0, entitiesPrefabs.Length)];
                
                var instance = entityManager.Instantiate(randomPrefab);
                entityManager.SetComponentData(instance, new Translation {Value = position});
            }
        }
    }
}
