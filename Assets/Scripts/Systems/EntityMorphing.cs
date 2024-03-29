﻿using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public abstract class EntityMorphing<T1> : JobSystemDelayed
    where T1 : struct, IComponentData
{
//    [BurstCompile]
    struct EntityMorphingJob : IJobForEachWithEntity<Translation, GridPosition, T1>
    {
        [ReadOnly] public Random random;
        [ReadOnly] public float morphChance;
        
        [WriteOnly] public EntityCommandBuffer.Concurrent CommandBuffer;
        public SpawnerGardenEntity spawner;
        [ReadOnly] public EntityType targetEntityType;

        public void Execute(Entity entity, int index, [ReadOnly] ref Translation translation, [ReadOnly] ref GridPosition gridPosition, [ReadOnly] ref T1 originComponent)
        {
            if (random.NextFloat() < morphChance)
            {
                CommandBuffer.DestroyEntity(index, entity);

                Entity targetPrefab = spawner.GetEntityForType(targetEntityType);
                Entity newEntity = CommandBuffer.Instantiate(index, targetPrefab);
                CommandBuffer.SetComponent(index, newEntity, new Translation { Value = translation.Value });
                CommandBuffer.SetComponent(index, newEntity, new GridPosition { Value = gridPosition.Value });
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

    protected override JobHandle DelayedUpdate(JobHandle inputDependencies)
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

public abstract class EntityMorphing<T1, T2> : JobSystemDelayed
    where T1 : struct, IComponentData
    where T2 : struct, ComponentWithValue
{
//    [BurstCompile]
    struct EntityMorphingJob : IJobForEachWithEntity<Translation, GridPosition, T1, T2>
    {
        [ReadOnly] public Random random;
        [ReadOnly] public float morphChance;
        [ReadOnly] public int conditionMoreThan;
        [ReadOnly] public int conditionLessThan;
        
        [WriteOnly] public EntityCommandBuffer.Concurrent CommandBuffer;
        public SpawnerGardenEntity spawner;
        [ReadOnly] public EntityType targetEntityType;

        public void Execute(Entity entity, int index, [ReadOnly] ref Translation translation, [ReadOnly] ref GridPosition gridPosition, [ReadOnly] ref T1 originComponent, [ReadOnly] ref T2 counterComponent)
        {
            bool canMorph = conditionMoreThan <= counterComponent.Value && counterComponent.Value <= conditionLessThan
                            && random.NextFloat() < morphChance;
            if (canMorph)
            {
                CommandBuffer.DestroyEntity(index, entity);

                Entity targetPrefab = spawner.GetEntityForType(targetEntityType);
                Entity newEntity = CommandBuffer.Instantiate(index, targetPrefab);
                CommandBuffer.SetComponent(index, newEntity, new Translation { Value = translation.Value });
                CommandBuffer.SetComponent(index, newEntity, new GridPosition { Value = gridPosition.Value });
            }
        }
    }

    protected abstract EntityType targetEntityType { get; }
    protected abstract float MorphChance { get; }
    protected virtual int ConditionMoreThan => int.MinValue;
    protected virtual int ConditionLessThan => int.MaxValue;
    
    private EntityCommandBufferSystem m_Barrier;
    private EntityQuery spawnerQuery;
    
    protected override void OnCreate()
    {
        m_Barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        spawnerQuery = GetEntityQuery(typeof(SpawnerGardenEntity));
    }

    protected override JobHandle DelayedUpdate(JobHandle inputDependencies)
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
            morphChance = MorphChance,
            conditionMoreThan = ConditionMoreThan,
            conditionLessThan = ConditionLessThan
        }.Schedule(this, inputDependencies);
        
        m_Barrier.AddJobHandleForProducer(jobHandle);
        
        spawnerArray.Dispose();
        
        return jobHandle;
    }
}

public abstract class EntityMorphingX<T1, T2> : JobSystemDelayed
    where T1 : struct, ComponentWithValue
    where T2 : struct, ComponentWithValue
{
    struct EntityMorphingJob : IJobForEachWithEntity<Translation, GridPosition, T1, T2>
    {
        [ReadOnly] public int multiplier;

        [WriteOnly] public EntityCommandBuffer.Concurrent CommandBuffer;
        public SpawnerGardenEntity spawner;
        [ReadOnly] public EntityType targetEntityType;

        public void Execute(Entity entity, int index, [ReadOnly] ref Translation translation, [ReadOnly] ref GridPosition gridPosition, ref T1 originComponent, [ReadOnly] ref T2 counterComponent)
        {
            originComponent.Value = originComponent.Value + counterComponent.Value * multiplier;

            bool shouldMorph = originComponent.Value <= 0;
            if (shouldMorph)
            {
                CommandBuffer.DestroyEntity(index, entity);

                Entity targetPrefab = spawner.GetEntityForType(targetEntityType);
                Entity newEntity = CommandBuffer.Instantiate(index, targetPrefab);
                CommandBuffer.SetComponent(index, newEntity, new Translation { Value = translation.Value });
                CommandBuffer.SetComponent(index, newEntity, new GridPosition { Value = gridPosition.Value });
            }
        }
    }

    protected abstract EntityType targetEntityType { get; }
    protected abstract int Multiplier { get; }

    private EntityCommandBufferSystem m_Barrier;
    private EntityQuery spawnerQuery;
    
    protected override void OnCreate()
    {
        m_Barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        spawnerQuery = GetEntityQuery(typeof(SpawnerGardenEntity));
    }

    protected override JobHandle DelayedUpdate(JobHandle inputDependencies)
    {
        EntityCommandBuffer.Concurrent commandBuffer = m_Barrier.CreateCommandBuffer().ToConcurrent();

        NativeArray<SpawnerGardenEntity> spawnerArray = spawnerQuery.ToComponentDataArray<SpawnerGardenEntity>(Allocator.TempJob);
        var spawner = spawnerArray[0];
        
        var jobHandle = new EntityMorphingJob
        {
            CommandBuffer = commandBuffer,
            spawner = spawner,
            targetEntityType = targetEntityType,
            multiplier = Multiplier
        }.Schedule(this, inputDependencies);
        
        m_Barrier.AddJobHandleForProducer(jobHandle);
        
        spawnerArray.Dispose();
        
        return jobHandle;
    }
}