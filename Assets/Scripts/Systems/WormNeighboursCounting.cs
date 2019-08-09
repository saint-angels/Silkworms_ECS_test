using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static Unity.Mathematics.math;
using Random = Unity.Mathematics.Random;

public class WormNeighboursCounting : JobComponentSystem
{
    // This declares a new kind of job, which is a unit of work to do.
    // The job is declared as an IJobForEach<Translation, Rotation>,
    // meaning it will process all entities in the world that have both
    // Translation and Rotation components. Change it to process the component
    // types you want.
    //
    // The job is also tagged with the BurstCompile attribute, which means
    // that the Burst compiler will optimize it for the best performance.
    
    [BurstCompile]
    struct WormNeighbourCountingJob : IJobForEach<Translation, NeighboursWorm>
    {
        [DeallocateOnJobCompletion] [ReadOnly] public NativeArray<Translation> wormPositions;

        public void Execute([ReadOnly] ref Translation translation, ref NeighboursWorm neighboursWorm)
        {
            int neighbourWormsCount = 0;
            for (int i = 0; i < wormPositions.Length; i++)
            {
                float distanceToWorm = math.distancesq(translation.Value, wormPositions[i].Value);
                if (distanceToWorm <= 1)
                {
                    neighbourWormsCount++;
                }
            }
            neighboursWorm.Value = neighbourWormsCount;
        }
    }
    
    private EntityQuery wormsQuery;

    protected override void OnCreate()
    {
        base.OnCreate();
        wormsQuery = GetEntityQuery(typeof(Worm), typeof(Translation));
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        NativeArray<Translation> wormTranslations = wormsQuery.ToComponentDataArray<Translation>(Allocator.TempJob);

        var job = new WormNeighbourCountingJob
        {
            wormPositions = wormTranslations
        };
            
        var jobHandle = job.Schedule(this, inputDependencies);
        return jobHandle;
    }
}