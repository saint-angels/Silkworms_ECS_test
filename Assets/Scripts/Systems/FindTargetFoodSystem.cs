using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static Unity.Mathematics.math;
using float3 = Unity.Mathematics.float3;

public class FindTargetFoodSystem : JobComponentSystem
{
    private struct EntityWithPosition {
        public Entity entity;
        public float3 position;
    }
    
    
    [RequireComponentTag(typeof(Eater))]
    [ExcludeComponent(typeof(FoodTarget))]
    [BurstCompile]
    struct FindTargetFoodSystemJob : IJobForEachWithEntity<Translation>
    {
        [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<EntityWithPosition> foodPositions;
        public NativeArray<Entity> closestFoodEntitiesResult; //Result closest food for all entities
        
        public void Execute(Entity entity, int index, [ReadOnly] ref Translation translation)
        {
            float3 eaterPosition = translation.Value;
            Entity closestFoodEntity = Entity.Null;
            float closestFoodDistance = float.MaxValue;

            for (int i = 0; i < foodPositions.Length; i++)
            {
                EntityWithPosition foodEntityPosition = foodPositions[i];

                float foodDistance = math.distance(eaterPosition, foodEntityPosition.position);
                bool foodCloser = foodDistance < closestFoodDistance;
                if (foodCloser)
                {
                    closestFoodEntity = foodEntityPosition.entity;
                    closestFoodDistance = foodDistance;
                }
            }

            closestFoodEntitiesResult[index] = closestFoodEntity;

        }
    }
    
    [RequireComponentTag(typeof(Eater))]
    [ExcludeComponent(typeof(FoodTarget))]
    private struct AddFoodTargetComponentJob : IJobForEachWithEntity<Translation>
    {
        [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<Entity> closestFoodEntityArray;
        public EntityCommandBuffer.Concurrent entityCommandBuffer;

        public void Execute(Entity entity, int index, ref Translation translation)
        {
            if (closestFoodEntityArray[index] != Entity.Null)
            {
                entityCommandBuffer.AddComponent(index, entity, new FoodTarget { targetFoodEntity = closestFoodEntityArray[index] });
            }
        }
    }
    
    private EndSimulationEntityCommandBufferSystem commandBufferSystem;
    private EntityQuery foodQuery;
    private EntityQuery eaterQuery;
    
    
    protected override void OnCreate()
    {
        base.OnCreate();

        commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

        foodQuery = GetEntityQuery(typeof(Food), ComponentType.ReadOnly<Translation>());
        
        eaterQuery = GetEntityQuery(typeof(Eater), ComponentType.Exclude<FoodTarget>());
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        var job = new FindTargetFoodSystemJob();

        //Do they really need tempJob? Isn't Temp enough for 1 frame calc?
        var foodEntityArray = foodQuery.ToEntityArray(Allocator.TempJob);
        var foodTranslationArray = foodQuery.ToComponentDataArray<Translation>(Allocator.TempJob);
        
        var foodPositions = new NativeArray<EntityWithPosition>(foodEntityArray.Length, Allocator.TempJob);

        for (int i = 0; i < foodPositions.Length; i++) {
            foodPositions[i] = new EntityWithPosition {
                entity = foodEntityArray[i],
                position = foodTranslationArray[i].Value,
            };
        }
        
        foodEntityArray.Dispose();
        foodTranslationArray.Dispose();
        
        NativeArray<Entity> closestFoodEntitiesResult = new NativeArray<Entity>(eaterQuery.CalculateLength(), Allocator.TempJob);
        

        FindTargetFoodSystemJob findTargetFoodSystemJob = new FindTargetFoodSystemJob
        {
                foodPositions = foodPositions,
                closestFoodEntitiesResult = closestFoodEntitiesResult
        };

        JobHandle jobHandle = findTargetFoodSystemJob.Schedule(this, inputDependencies);
            
        AddFoodTargetComponentJob addComponentJob = new AddFoodTargetComponentJob {
            closestFoodEntityArray = closestFoodEntitiesResult,
            entityCommandBuffer = commandBufferSystem.CreateCommandBuffer().ToConcurrent(),
        };
        jobHandle = addComponentJob.Schedule(this, jobHandle);
        
//        foodEntityTranslationArray.Dispose();
        
        commandBufferSystem.AddJobHandleForProducer(jobHandle);

        return jobHandle;
    }
}