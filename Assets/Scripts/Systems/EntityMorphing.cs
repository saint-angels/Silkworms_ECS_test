using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public abstract class EntityMorphing<T1> : JobComponentSystem
    where T1 : struct, IComponentData
{
//    [BurstCompile]
    struct EntityMorphingJob : IJobForEachWithEntity<Translation, T1>
    {
        [ReadOnly] public Random random;
        [ReadOnly] public float morphChance;
        
        [WriteOnly] public EntityCommandBuffer.Concurrent CommandBuffer;
        public SpawnerGardenEntity spawner;
        [ReadOnly] public EntityType targetEntityType;

        public void Execute(Entity entity, int index, ref Translation translation, [ReadOnly] ref T1 originComponent)
        {
            if (random.NextFloat() < morphChance)
            {
                CommandBuffer.DestroyEntity(index, entity);

                Entity targetPrefab = spawner.GetEntityForType(targetEntityType);
                Entity newEntity = CommandBuffer.Instantiate(index, targetPrefab);
                CommandBuffer.SetComponent(index, newEntity, new Translation { Value = translation.Value });
            }
        }
    }

    protected abstract EntityType targetEntityType { get; }
    protected abstract float MorphChance { get; }
    
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
        
        var jobHandle = new EntityMorphingJob
        {
            CommandBuffer = commandBuffer,
            spawner = spawner,
            random = new Random((uint)(UnityEngine.Random.value * 100 + 1)),
            targetEntityType = targetEntityType,
            morphChance = MorphChance
        }.Schedule(this, inputDependencies);
        
        m_Barrier.AddJobHandleForProducer(jobHandle);
        
        spawnerArray.Dispose();
        
        return jobHandle;
    }
}