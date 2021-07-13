using System.Collections.Generic;

namespace Pathfinding
{
    public interface IAstarData<TNode, TEdge>
        where TNode : INode
        where TEdge : IWeightedEdge<TNode>
    {
        public TNode[] Nodes { get; }
        public TEdge[] Edges { get; }

        public TNode[] FrontierNodes { get; }
        public double GetNodeCostOf(ref TNode node);
        public void SetNodeVisited(ref TNode node);

        public void ResetForNewOriginNode(ref TNode node);
        
        public List<TEdge> GetPathTo(int destinationId);

        public bool AddAFrontierNode(
            ref TNode newFrontierNode,
            ref TNode fromNode,
            double edgeWeight,
            double costToNode, double heuristicCost);

        public bool TryGetNodeWithMinimumCost(out TNode nodeToProcess);

        public List<TEdge> GetEdgesOriginatingFromNode(ref TNode node);
    }
}
