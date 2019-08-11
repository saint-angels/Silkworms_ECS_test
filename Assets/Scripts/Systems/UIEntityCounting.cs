using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class UIEntityCounting : ComponentSystem
{
    protected override void OnUpdate()
    {
        var queryWorms = GetEntityQuery(typeof(Worm));
        int wormsCount = queryWorms.CalculateEntityCount();
        Root.UIManager.UpdateEntityCounter(EntityType.WORM, wormsCount);
    }
}