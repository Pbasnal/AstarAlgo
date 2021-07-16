using Pathfinding;
using UnityEngine;

namespace MainGame
{
    public class Node : INodeWithPriority
    {
        public bool isWalkable;
        public Vector3 worldPosition;

        public double Priority { get; set; }
        public int Id { get; set; }

        public string NodeId { get; set; }
        public int PreviousNode { get; set; }
        public double NodeCost { get; set; }
        public double HeuristicCost { get; set; }

        public Node()
        { }

        public Node(bool isWalkable, Vector3 worldPosition)
        {
            NodeId = string.Empty;
            this.isWalkable = isWalkable;
            this.worldPosition = worldPosition;
            Priority = 0;
            Id = 0;

            PreviousNode = 0;
            NodeCost = 0;
            HeuristicCost = 0;
        }
    }

    public class WeightedEdge : IWeightedEdge<Node>
    {
        public double Weight { get; set; }

        public Node OriginNode { get; set; }
        public Node DestinationNode { get; set; }

        public WeightedEdge()
        { }

        public WeightedEdge(Node originNode, Node destinationNode, double cost)
        {
            OriginNode = originNode;
            DestinationNode = destinationNode;

            Weight = cost;
        }
    }
}