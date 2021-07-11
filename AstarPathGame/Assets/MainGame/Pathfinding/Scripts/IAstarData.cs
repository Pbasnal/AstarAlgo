using System.Collections.Generic;

namespace Pathfinding
{
    public interface IAstarData<TNode, TEdge>
        where TNode : INode, new()
        where TEdge : IWeightedEdge<TNode>, new()
    {
        public TNode[] Nodes { get; }
        public TEdge[] Edges { get; }

        public double GetNodeCostOf(TNode node);
        public void SetNodeVisited(TNode node);

        public void ResetForNewOriginNode(TNode node);
        
        public List<TEdge> GetPathTo(int destinationId);

        public bool AddAFrontierNode(
            TNode newFrontierNode,
            TNode fromNode,
            double edgeWeight,
            double cost);

        public bool TryGetNodeWithMinimumCost(out TNode nodeToProcess);

        public List<TEdge> GetEdgesOriginatingFromNode(TNode node);
    }
}
