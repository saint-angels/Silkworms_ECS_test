using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

public class LeafEating : EntityMorphingX<Leaf, NeighboursWorm>
{
    protected override EntityType targetEntityType => EntityType.EARTH;
    protected override int Multiplier => -5;
}