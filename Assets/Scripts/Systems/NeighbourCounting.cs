using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static Unity.Mathematics.math;
using Random = Unity.Mathematics.Random;

public abstract class NeighbourCounting<T1, T2> : JobComponentSystem
    where T1 : struct, ComponentWithValue
    where T2 : struct, IComponentData
{
    [BurstCompile]
    struct NeighbourCountingJob : IJobForEach<Translation, T1>
    {
        [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<Translation> targetComponentPositions;
        [ReadOnly] public float requiredDistanceSq; 
        
        public void Execute([ReadOnly] ref Translation translation, ref T1 neighboursCounter)
        {
            int neighbourCount = 0;
            for (int i = 0; i < targetComponentPositions.Length; i++)
            {
                float distanceToTarget = math.distancesq(translation.Value, targetComponentPositions[i].Value);
                if (distanceToTarget <= requiredDistanceSq)
                {
                    neighbourCount++;
                }
            }
            
            neighboursCounter.Value = neighbourCount;
        }
    }
    protected abstract float RequiredDistanceSq { get; }

    private EntityQuery targetNeighbourQuery;

    protected override void OnCreate()
    {
        base.OnCreate();
        targetNeighbourQuery = GetEntityQuery(typeof(T2), typeof(Translation));
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        NativeArray<Translation> targetComponetPositions = targetNeighbourQuery.ToComponentDataArray<Translation>(Allocator.TempJob);

        var job = new NeighbourCountingJob
        {
            targetComponentPositions = targetComponetPositions,
            requiredDistanceSq = RequiredDistanceSq
        };
            
        var jobHandle = job.Schedule(this, inputDependencies);
        return jobHandle;
    }
}