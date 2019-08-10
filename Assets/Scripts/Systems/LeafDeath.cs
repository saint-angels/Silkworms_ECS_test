using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class LeafDeath : JobComponentSystem
{
    // This declares a new kind of job, which is a unit of work to do.
    // The job is declared as an IJobForEach<Translation, Rotation>,
    // meaning it will process all entities in the world that have both
    // Translation and Rotation components. Change it to process the component
    // types you want.


    [RequireComponentTag(typeof(Leaf))]
    struct LeafDeathJob : IJobForEachWithEntity<NeighboursWorm, Translation>
    {
        [WriteOnly]
        public EntityCommandBuffer.Concurrent CommandBuffer;
        
        [ReadOnly]
        public float deathChance;
        
        public SpawnerGardenEntity spawner;
        
        public void Execute(Entity entity, int index, [ReadOnly] ref NeighboursWorm wormNeighbours, [ReadOnly] ref Translation translation)
        {
            if (3 <= wormNeighbours.Value)
            {
                CommandBuffer.DestroyEntity(index, entity);
                var newEarthEntity = CommandBuffer.Instantiate(index, spawner.prefabEarth);
                CommandBuffer.SetComponent(index, newEarthEntity, new Translation { Value = translation.Value });
            }
        }
    }
    //

    // The job is also tagged with the BurstCompile attribute, which means

    // that the Burst compiler will optimize it for the best performance.
    
    private EntityCommandBufferSystem m_Barrier;
    private EntityQuery spawnerQuery;
    
    protected override void OnCreate()
    {
        m_Barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        spawnerQuery = GetEntityQuery(typeof(SpawnerGardenEntity));
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        return inputDependencies;
        
        EntityCommandBuffer.Concurrent commandBuffer = m_Barrier.CreateCommandBuffer().ToConcurrent();

        NativeArray<SpawnerGardenEntity> spawnerArray = spawnerQuery.ToComponentDataArray<SpawnerGardenEntity>(Allocator.TempJob);
        var spawner = spawnerArray[0];
        
        var jobHandle = new LeafDeathJob
        {
            CommandBuffer = commandBuffer,
            spawner = spawner
        }.Schedule(this, inputDependencies);
        
        m_Barrier.AddJobHandleForProducer(jobHandle);
   
        
        spawnerArray.Dispose();
        // Now that the job is set up, schedule it to be run. 
        return jobHandle;
    }
}