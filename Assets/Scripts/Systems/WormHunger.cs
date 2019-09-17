using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class WormHunger : JobSystemDelayed
{
    [BurstCompile]
    struct WormHungerJob : IJobForEach<Worm>
    {
        [ReadOnly] public int hungerAmount;
        
        public void Execute(ref Worm worm)
        {
            worm.Value = max(worm.Value - hungerAmount, 0);
        }
    }

    protected override JobHandle DelayedUpdate(JobHandle inputDependencies)
    {
        var job = new WormHungerJob
        {
            hungerAmount = 20
        };

        return job.Schedule(this, inputDependencies);
    }
}