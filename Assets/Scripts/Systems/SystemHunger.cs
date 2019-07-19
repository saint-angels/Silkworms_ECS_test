using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static Unity.Mathematics.math;

public class SystemHunger : JobComponentSystem
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
    struct SystemHungerJob : IJobForEachWithEntity<ComponentEater>
    {
        // Add fields here that your job needs to do its work.
        // For example,
        public float deltaTime;

        [WriteOnly]
        public EntityCommandBuffer.Concurrent CommandBuffer;

        public void Execute(Entity entity, int jobIndex, ref ComponentEater eater)
        {
            eater.currentFullness = eater.currentFullness - deltaTime * eater.hungerSpeed;    
            
            if (eater.currentFullness < 0.0f)
            {
                CommandBuffer.DestroyEntity(jobIndex, entity);
            }

            // Implement the work to perform for each entity here.
            // You should only access data that is local or that is a
            // field on this job. Note that the 'rotation' parameter is
            // marked as [ReadOnly], which means it cannot be modified,
            // but allows this job to run in parallel with other jobs
            // that want to read Rotation component data.
            // For example,
            //     translation.Value += mul(rotation.Value, new float3(0, 0, 1)) * deltaTime;
        }
    }
    
    EntityCommandBufferSystem m_Barrier;

    protected override void OnCreate()
    {
        m_Barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        EntityCommandBuffer.Concurrent commandBuffer = m_Barrier.CreateCommandBuffer().ToConcurrent();

        JobHandle job = new SystemHungerJob
        {
            deltaTime = Time.deltaTime,
            CommandBuffer = commandBuffer
        }.Schedule(this, inputDependencies);
        
        m_Barrier.AddJobHandleForProducer(job);
        
        return job;
    }
}