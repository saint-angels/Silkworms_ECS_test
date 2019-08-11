using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountingNeighbourLeaves : NeighbourCounting<NeighboursLeaf, Leaf>
{
    protected override float RequiredDistanceSq => requiredDistanceSq;

    private const float requiredDistanceSq = 1.5f * 1.5f;
}
