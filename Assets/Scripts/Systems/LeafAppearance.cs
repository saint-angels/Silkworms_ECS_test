using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class LeafAppearance : JobComponentSystem
{
    // This declares a new kind of job, which is a unit of work to do.
    // The job is declared as an IJobForEach<Translation, Rotation>,
    // meaning it will process all entities in the world that have both
    // Translation and Rotation components. Change it to process the component
    // types you want.
    //
    // The job is also tagged with the BurstCompile attribute, which means
    // that the Burst compiler will optimize it for the best performance.
    
    //Burst doesn't support command buffer for now
//    [BurstCompile]
    struct LeafAppearanceJob : IJobForEachWithEntity<Translation, Earth>
    {
        [WriteOnly]
        public EntityCommandBuffer.Concurrent CommandBuffer;

        [ReadOnly] 
        public Random random;

        public SpawnerGardenEntity spawner;
        
        public void Execute(Entity entity, int index, ref Translation translation, [ReadOnly] ref Earth c1)
        {
            if (random.NextFloat() < .01f)
            {
                CommandBuffer.DestroyEntity(index, entity);

                var newLeafEntity = CommandBuffer.Instantiate(index, spawner.prefabLeaf);
                CommandBuffer.SetComponent(index, newLeafEntity, new Translation { Value = translation.Value });
            }
        }
    }
    
    private EntityCommandBufferSystem m_Barrier;
    private EntityQuery spawnerQuery;

    protected override void OnCreate()
    {
        m_Barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        spawnerQuery = GetEntityQuery(typeof(SpawnerGardenEntity));
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        EntityCommandBuffer.Concurrent commandBuffer = m_Barrier.CreateCommandBuffer().ToConcurrent();

        NativeArray<SpawnerGardenEntity> spawnerArray = spawnerQuery.ToComponentDataArray<SpawnerGardenEntity>(Allocator.TempJob);
        var spawner = spawnerArray[0];
        
        var jobHandle = new LeafAppearanceJob
        {
            CommandBuffer = commandBuffer,
            random =new Random((uint)(UnityEngine.Random.value * 100 + 1)),
            spawner = spawner
        }.Schedule(this, inputDependencies);
        
        m_Barrier.AddJobHandleForProducer(jobHandle);
   
        
        spawnerArray.Dispose();
        // Now that the job is set up, schedule it to be run. 
        return jobHandle;
    }
}