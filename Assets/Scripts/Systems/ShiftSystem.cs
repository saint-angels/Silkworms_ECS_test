using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static Unity.Mathematics.math;

public class ShiftSystem : ComponentSystem
{
    private EntityManager entityManager;
    
    private EntityCommandBufferSystem m_Barrier;

    protected override void OnCreate()
    {
        base.OnCreate();
        
        entityManager = World.Active.EntityManager;
        
        
        m_Barrier = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();

        
    }

    public void OnDirectionPressed(Vector2 direction)
    {
        EntityCommandBuffer commandBuffer = m_Barrier.CreateCommandBuffer();
        
        
        Entities.WithNone<Shifting>().ForEach((Entity entity, ref Translation translation) =>
        {
            PostUpdateCommands.SetComponent(entity, new Shifting
            {
                direction = direction,
                distanceLeft = 5f,
                speed = 10f
            });
        });
    }

    protected override void OnUpdate()
    {
        if (Root.PlayerInput.directionPressed.HasValue)
        {
            Entities.WithNone<Shifting>().ForEach((Entity entity) =>
            {
                PostUpdateCommands.AddComponent(entity, new Shifting
                {
                    direction = Root.PlayerInput.directionPressed.Value,
                    distanceLeft = 10f,
                    speed = 10f
                });
            });

            Root.PlayerInput.directionPressed = null;
        }
    }
}

public class ShiftSystem2 : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity entity, ref Shifting shifting, ref Translation translation) =>
        {
            float2 shiftAmount = shifting.direction * shifting.speed * Time.deltaTime;
            float3 newTranslation = new float3(translation.Value.x + shiftAmount.x, translation.Value.y + shiftAmount.y, 0);
            translation.Value = newTranslation;
            
            float distanceShifted =  math.length(shiftAmount);
            float distanceLeft = math.max(0, shifting.distanceLeft - distanceShifted);
            if (distanceLeft <= 0)
            {
                PostUpdateCommands.RemoveComponent<Shifting>(entity);
            }
            else
            {
                shifting.distanceLeft = distanceLeft;
            }
        });
    }
}