﻿using Pathfinding;
using UnityEngine;

namespace MainGame
{
    public class AstarPathfinder : AstarPath<Node, WeightedEdge>
    {
        public AstarPathfinder(IAstarData<Node, WeightedEdge> astarData)
            : base(astarData)
        {
        }

        public override double HeuristicCost(Node fromNode, Node toNode)
        {
            return Vector3.Distance(toNode.worldPosition, fromNode.worldPosition);
        }
    }
}
