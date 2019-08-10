using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class LeafBirth : EntityMorphing<Earth>
{
    protected override EntityType targetEntityType => EntityType.LEAF;
    protected override float MorphChance => .01f;

}