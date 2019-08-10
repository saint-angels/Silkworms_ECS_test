using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

[UpdateBefore(typeof(LeafDeath))]
public class WormDeath : EntityMorphing<Worm, NeighboursLeaf>
{
    protected override EntityType targetEntityType => EntityType.EARTH;
    protected override float MorphChance => .9f;
    protected override int ConditionLessThan => 0;
}