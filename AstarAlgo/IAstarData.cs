using System.Collections.Generic;

namespace AstarAlgo
{
    public interface IAstarData<TNode, TEdge>
        where TNode : INode, new()
        where TEdge : IWeightedEdge, new()
    {
        public TNode[] Nodes { get; }
        public TEdge[] Edges { get; }

        public double GetNodeCostOf(int id);
        public void SetNodeVisited(int id);

        public void ResetForNewOriginNode(int id);

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
