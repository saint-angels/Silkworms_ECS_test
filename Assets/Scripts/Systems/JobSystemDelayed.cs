﻿using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public abstract class JobSystemDelayed : JobComponentSystem
{
    float currentCooldown;
    
    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        if (currentCooldown <= 0)
        {
            currentCooldown = Root.SimulationTick;
            return DelayedUpdate(inputDependencies);
        }
        else
        {
            currentCooldown -= UnityEngine.Time.deltaTime;
            return inputDependencies;
        }
    }

    protected abstract JobHandle DelayedUpdate(JobHandle inputDependencies);
}