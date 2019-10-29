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
    struct RandomSelection
    {
        public Entity entityPrefab;
        public float probability;

        public RandomSelection(Entity entityPrefab, float probability)
        {
            this.entityPrefab = entityPrefab;
            this.probability = probability;
        }
    }
    
    
    [SerializeField] private int width = 150;
    [SerializeField] private int height = 150;

    [SerializeField] private float cellScale = 1f;

    [SerializeField] private GameObject earthPrefab;
    [SerializeField] private GameObject leafPrefab;
    [SerializeField] private GameObject wormPrefab;

    private EntityManager entityManager;

    private void Start()
    {
        entityManager = World.Active.EntityManager;
        
        Entity earthEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(earthPrefab, World.Active);
        Entity leafEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(leafPrefab, World.Active);
        Entity antEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(wormPrefab, World.Active);

        RandomSelection[] entityPrefabs = new RandomSelection[]
        {
            new RandomSelection(earthEntityPrefab, .3f), 
            new RandomSelection(leafEntityPrefab, .7f), 
//            new RandomSelection(antEntityPrefab, .1f) 
        };
        
        float3 startPoint = new float3(- (width / 2f) * cellScale, - (height / 2f) * cellScale, 0);
        //Create ants
        int antHomeRaidus = 5;

        int homeCenterX = UnityEngine.Random.Range(0, width);
        int homeCenterY = UnityEngine.Random.Range(0, height);

        for (int x = homeCenterX - antHomeRaidus; x < homeCenterX + antHomeRaidus; x++)
        {
            for (int y = homeCenterY - antHomeRaidus; y < homeCenterY + antHomeRaidus; y++)
            {
                if (Mathf.Sqrt(Mathf.Pow(x - homeCenterX, 2) + Mathf.Pow(y - homeCenterY, 2)) < antHomeRaidus)
                {
                    float3 position = new float3(x * cellScale + startPoint.x, y * cellScale + startPoint.y, 0);

                    Entity antEntityinstance = entityManager.Instantiate(antEntityPrefab);
                    entityManager.SetComponentData(antEntityinstance, new GridPosition { Value = new int2(x, y)});
                    entityManager.SetComponentData(antEntityinstance, new Translation {Value = position});
                }
            }
        }
        

        //Generate grid
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float3 position = new float3(x * cellScale + startPoint.x, y * cellScale + startPoint.y, 0);
                Entity randomPrefab = GetRandomValue(entityPrefabs);
                
                Entity instance = entityManager.Instantiate(randomPrefab);
                entityManager.SetComponentData(instance, new GridPosition { Value = new int2(x, y)});
                entityManager.SetComponentData(instance, new Translation {Value = position});
            }
        }
    }
    
    Entity GetRandomValue(params RandomSelection[] selections) {
        float rand = UnityEngine.Random.value;
        float currentProb = 0;
        foreach (var selection in selections) {
            currentProb += selection.probability;
            if (rand <= currentProb)
            {
                return selection.entityPrefab;
            }
        }
 
        //will happen if the input's probabilities sums to less than 1
        //throw error here if that's appropriate
        return Entity.Null;
    }
}
