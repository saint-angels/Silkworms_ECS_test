using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Networking;
using static Unity.Mathematics.math;

public class ShiftSystem : ComponentSystem
{
    private EntityManager entityManager;
    
    protected override void OnCreate()
    {
        base.OnCreate();
        
        entityManager = World.Active.EntityManager;
    }

    protected override void OnUpdate()
    {
        if (Root.PlayerInput.directionPressed.HasValue)
        {
            float2 direction = Root.PlayerInput.directionPressed.Value;
            
            Entities.WithNone<Shift>().ForEach((Entity entity, ref Translation translation) =>
            {
                float3 endPosition = new float3(translation.Value.x + direction.x, translation.Value.y + direction.y, 0);

                PostUpdateCommands.AddComponent(entity, new Shift
                {
                    duration = .3f,
                    startPosition = translation.Value,
                    endPosition = endPosition,
                    progress = 0
                });
            });

            Root.PlayerInput.directionPressed = null;
        }
    }
}

/// <summary>
/// WHY THE HELL I NEED TO SPLIT SYSTEM INTO 2??
/// </summary>
public class ShiftSystem2 : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity entity, ref Shift shift, ref Translation translation) =>
        {
            shift.progress += Time.deltaTime / shift.duration;

            float progressTween = Easing.Circular.Out(shift.progress);
            
            float3 currentTranslation = math.lerp(shift.startPosition, shift.endPosition, progressTween);

            translation.Value = currentTranslation;
            
            if (shift.progress >= 1)
            {
                PostUpdateCommands.RemoveComponent<Shift>(entity);
            }
        });
    }
}