using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    private EntityManager entityManager;

    private EatersConfig eatersConfig;
    private FoodConfig foodConfig;
    private EntitiesAssets entitiesAssets;
    

    public void Init()
    {
        entityManager = World.Active.EntityManager;
        
        eatersConfig = Root.ConfigManager.EatersConfig;
        foodConfig = Root.ConfigManager.FoodConfig;
        entitiesAssets = Root.ConfigManager.EntitiesAssets;
        
    }

    public Entity SpawnEntity(EntityType entityType, Vector2 position2D)
    {
        float3 position = new float3(position2D.x, position2D.y, 0);
        
        Entity newEntity = entityManager.CreateEntity(
            typeof(Translation),
            typeof(LocalToWorld),
            typeof(RenderMesh),
            typeof(Scale)
        );
        
        entityManager.SetComponentData(newEntity, new Translation{ Value = position});
        entityManager.SetComponentData(newEntity, new Scale { Value = 1f });


        switch (entityType)
        {
            case EntityType.WORM:
                entityManager.AddComponentData(newEntity, new Eater
                {
                    hungerSpeed = eatersConfig.hungerSpeed,
                    eatSpeed = eatersConfig.eatSpeed,
                    currentFullness =  eatersConfig.maxFullness,
                    maxFullness = eatersConfig.maxFullness
                });
                SetEntityRenderData(newEntity, entitiesAssets.wormMaterial);

                break;
            case EntityType.LEAF:
                entityManager.AddComponentData(newEntity, new Food
                {
                    foodAmount = foodConfig.maxFoodAmount
                });    
                
                SetEntityRenderData(newEntity, entitiesAssets.leafMaterial);
                break;
        }

        return newEntity;
    }
    
    private void SetEntityRenderData(Entity entity, Material material) {
        entityManager.SetSharedComponentData<RenderMesh>(entity,
            new RenderMesh {
                material = material,
                mesh = entitiesAssets.quadMesh,
            }
        );
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
}
