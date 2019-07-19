using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private EntityType entityType;
    [SerializeField] private GameObject Prefab;
    
    [SerializeField] private float spawnRadius = 3f;
    [SerializeField] private float spawnCooldown = 5f;
    [SerializeField] private int burstSize = 1;

    [SerializeField] private bool doSpawn = true;
    
    private Entity entityPrefab;
    private EntityManager entityManager;


    void Start()
    {
        entityManager = World.Active.EntityManager;
        entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(Prefab, World.Active);

        EatersConfig eatersConfig = Root.ConfigManager.EatersConfig;
        FoodConfig foodConfig = Root.ConfigManager.FoodConfig;

        switch (entityType)
        {
            case EntityType.WORM:
                
                var eaterData = new ComponentEater
                {
                    hungerSpeed = eatersConfig.hungerSpeed,
                    eatSpeed = eatersConfig.eatSpeed,
                    currentFullness =  eatersConfig.maxFullness,
                    maxFullness = eatersConfig.maxFullness
                };

                entityManager.AddComponentData(entityPrefab, eaterData);
                break;
            case EntityType.LEAF:
                var foodData = new ComponentFood
                {
                    foodAmount = foodConfig.maxFoodAmount
                };
                
                entityManager.AddComponentData(entityPrefab, foodData);    
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        

        StartCoroutine(SpawnRoutine());
    }
    
    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            if (doSpawn)
            {
                for (int i = 0; i < burstSize; i++)
                {
                    Entity instance = entityManager.Instantiate(entityPrefab);
                    Vector2 instancePosition = Random.insideUnitCircle * spawnRadius;
                    
                    float3 position = transform.TransformPoint(new float3(instancePosition.x, instancePosition.y, 0));
                    entityManager.SetComponentData(instance, new Translation {Value = position});
                }    
            }
            
            yield return new WaitForSeconds(spawnCooldown);
        }
    }


    private Mesh CreateSpriteQuadMesh() 
    {
        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];
        
        vertices[0] = new Vector3(-.5f, -.5f);
        vertices[1] = new Vector3(-.5f, +.5f);
        vertices[2] = new Vector3(+.5f, +.5f);
        vertices[3] = new Vector3(+.5f, -.5f);
        
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(0, 1);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(1, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 3;
        
        triangles[3] = 1;
        triangles[4] = 2;
        triangles[5] = 3;
        
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        return mesh;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
