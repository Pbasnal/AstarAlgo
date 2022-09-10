using System;
using PathfindingAi.GraphStructure;

namespace PathfindingAi.Astar
{
    public interface IAstarPathNode : INode, IComparable<IAstarPathNode>
    {
        public IAstarPathNode PreviousNode { get; set; }
        public IWeightedEdge EdgeFromPreviousNode {get; set;}
        public float CostOfPathToNode { get; set; }
        public float HeuristicCost { get; set; }
        public bool IsVisited { get; set; }
    }
}