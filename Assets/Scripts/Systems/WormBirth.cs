using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

//[UpdateBefore(typeof(WormEating))]
public class WormBirth : EntityMorphing<Earth, NeighboursWorm>
{
    protected override EntityType targetEntityType => EntityType.WORM;
    protected override float MorphChance => .7f;
    protected override int ConditionMoreThan => 2;
    protected override int ConditionLessThan => 3;
}

public class WormDeathStarvation : EntityMorphing<Worm, NeighboursWorm>
{
    protected override EntityType targetEntityType => EntityType.EARTH;
    protected override float MorphChance => 1f;
    protected override int ConditionMoreThan => 0;
    protected override int ConditionLessThan => 1;
}

public class WormDeathOverCrowding : EntityMorphing<Worm, NeighboursWorm>
{
    protected override EntityType targetEntityType => EntityType.EARTH;
    protected override float MorphChance => 1f;
    protected override int ConditionMoreThan => 4;
}


//public class WormBirthFromEarth : EntityMorphing<Earth>
//{
//    protected override EntityType targetEntityType => EntityType.WORM;
//    protected override float MorphChance => .01f;
//}