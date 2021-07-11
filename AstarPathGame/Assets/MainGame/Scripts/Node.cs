using Pathfinding;
using UnityEngine;

namespace MainGame
{
    public struct Node : INodeWithPriority
    {
        public bool isWalkable;
        public Vector3 worldPosition;

        public int Priority { get; set; }
        public int Id { get; set; }

        public string NodeId { get; set; }

        public Node(bool isWalkable, Vector3 worldPosition)
        {
            NodeId = string.Empty;
            this.isWalkable = isWalkable;
            this.worldPosition = worldPosition;
            Priority = 0;
            Id = 0;
        }
    }

    public struct WeightedEdge : IWeightedEdge<Node>
    {
        public double Weight { get; set; }

        public Node OriginNode { get; set; }
        public Node DestinationNode { get; set; }

        public WeightedEdge(ref Node originNode, ref Node destinationNode, double cost)
        {
            OriginNode = originNode;
            DestinationNode = destinationNode;

            Weight = cost;
        }
    }
}