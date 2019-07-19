using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class SystemEating_Legacy : ComponentSystem
{
    private EntityQuery eatersQuery;
    private EntityQuery foodQuery;
    
    protected override void OnCreate()
    {
        eatersQuery = GetEntityQuery(typeof(ComponentEater), typeof(Translation));
        foodQuery = GetEntityQuery(typeof(ComponentFood), typeof(Translation));
    }

    protected override void OnUpdate()
    {
        NativeArray<Entity> foodEntities = foodQuery.ToEntityArray(Allocator.TempJob);
        NativeArray<Entity> eaterEntities = eatersQuery.ToEntityArray(Allocator.TempJob);

        NativeArray<ComponentEater> eaterComponents = eatersQuery.ToComponentDataArray<ComponentEater>(Allocator.TempJob);
        NativeArray<ComponentFood> foodComponents = foodQuery.ToComponentDataArray<ComponentFood>(Allocator.TempJob);

        NativeArray<Translation> eaterPositions = eatersQuery.ToComponentDataArray<Translation>(Allocator.TempJob);
        NativeArray<Translation> foodPositions = foodQuery.ToComponentDataArray<Translation>(Allocator.TempJob);

        
        EntityManager entityManager = World.Active.EntityManager;

        
        
        for (int i = 0; i < eaterComponents.Length; i++)
        {
            bool isEating = false;
            for (int j = 0; j < foodComponents.Length; j++)
            {
                bool closeEnough = math.distance(eaterPositions[i].Value, foodPositions[j].Value) <= 1f;

                if (closeEnough)
                {
                    float eaterBiteSize = eaterComponents[i].eatSpeed * Time.deltaTime;
                    float availableBiteSize = math.min(foodComponents[j].foodAmount, eaterBiteSize);

                    if (availableBiteSize > 0)
                    {
                        float newFullness = eaterComponents[i].currentFullness + availableBiteSize;
                        
                        //Make eater fuller!                        
                        entityManager.SetComponentData(eaterEntities[i], new ComponentEater
                        {
                            hungerSpeed = eaterComponents[i].hungerSpeed,
                            currentFullness = newFullness,
                            maxFullness = eaterComponents[i].hungerSpeed,
                            eatSpeed = eaterComponents[i].eatSpeed,
                            isEating = true
                        });

                        //Subtract food amount from food
                        float newFoodAmount = foodComponents[j].foodAmount - availableBiteSize;
                        
                        entityManager.SetComponentData(foodEntities[j], new ComponentFood
                        {
                            foodAmount = newFoodAmount 
                        });


                        bool foodEaten = Mathf.Approximately(newFoodAmount, 0f);
                        if (foodEaten)
                        {
                            PostUpdateCommands.DestroyEntity(foodEntities[j]);
                        }

                        isEating = true;
                        break;
                    }
                }
            }

            if (isEating == false)
            {
                entityManager.SetComponentData(eaterEntities[i], new ComponentEater
                {
                    hungerSpeed = eaterComponents[i].hungerSpeed,
                    currentFullness = eaterComponents[i].currentFullness,
                    maxFullness = eaterComponents[i].hungerSpeed,
                    eatSpeed = eaterComponents[i].eatSpeed,
                    isEating = false
                });
            }
        }
        
        eaterEntities.Dispose();
        foodEntities.Dispose();
        
        foodPositions.Dispose();
        eaterPositions.Dispose();

        eaterComponents.Dispose();
        foodComponents.Dispose();
    }
}
