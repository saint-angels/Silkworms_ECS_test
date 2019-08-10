using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

[UpdateBefore(typeof(WormDeath))]
public class WormBirth : EntityMorphing<Earth, NeighboursWorm>
{
    protected override EntityType targetEntityType => EntityType.WORM;
    protected override float MorphChance => .2f;
    protected override int ConditionMoreThan => 3;
}

public class WormBirthFromEarth : EntityMorphing<Earth>
{
    protected override EntityType targetEntityType => EntityType.WORM;
    protected override float MorphChance => .01f;
}