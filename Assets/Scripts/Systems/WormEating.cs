﻿using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using static Unity.Mathematics.math;

//[UpdateBefore(typeof(FoodEating))]
//public class WormEating : EntityMorphingX<Worm, NeighboursFood>
//{
//    protected override EntityType targetEntityType => EntityType.EARTH;
//    protected override int Multiplier => 3;
//    
//}