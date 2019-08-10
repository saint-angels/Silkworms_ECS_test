﻿using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class LeafDeath : EntityMorphing<Leaf, NeighboursWorm>
{
    protected override EntityType targetEntityType => EntityType.EARTH;
    protected override float MorphChance => .5f;
    protected override int ConditionMoreThan => 1;
}