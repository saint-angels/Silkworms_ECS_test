using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class EatSystem : ComponentSystem
{
    private EntityQuery eatersQuery;
    private EntityQuery foodQuery;
    
    protected override void OnCreate()
    {
        eatersQuery = GetEntityQuery(typeof(Eater), typeof(Translation));
        foodQuery = GetEntityQuery(typeof(Food), typeof(Translation));
    }

    protected override void OnUpdate()
    {
        var entityManager = World.Active.EntityManager;
        
        Entities.ForEach((Entity eaterEntity, ref Eater eater, ref TargetEntityFood targetFood, ref Translation translation) =>
        {
            if (World.Active.EntityManager.Exists(targetFood.foodEntity))
            {
                Translation foodTranslation = entityManager.GetComponentData<Translation>(targetFood.foodEntity);
                Food foodFood = entityManager.GetComponentData<Food>(targetFood.foodEntity);
                
                bool canEat = math.distance(translation.Value, foodTranslation.Value) < 1f;
                if (canEat)
                {
                    float eaterBiteSize = eater.eatSpeed * Time.deltaTime;
                    float availableBiteSize = math.min(foodFood.foodAmount, eaterBiteSize);
                    if (availableBiteSize > 0)
                    {
                        eater.currentFullness += availableBiteSize;
                        float newFoodAmount = foodFood.foodAmount - availableBiteSize;

                        if (newFoodAmount <= 0)
                        {
                            PostUpdateCommands.DestroyEntity(targetFood.foodEntity);
                        }
                        else
                        {
                            entityManager.SetComponentData(targetFood.foodEntity, new Food(newFoodAmount));    
                        }
                    }
                }
            }
            else
            {
                PostUpdateCommands.RemoveComponent(eaterEntity, typeof(TargetEntityFood));
            }
        });
    }
}
