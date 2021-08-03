using System.Collections.Generic;

namespace Pathfinding
{
    public interface IAstarData<TNode, TEdge>
        where TNode : INode
        where TEdge : IWeightedEdge<TNode>
    {
        public TNode[] Nodes { get; }
        public TEdge[] Edges { get; }

        public float GetNodeCostOf(TNode node);
        public void SetNodeVisited(TNode node);

        public void ResetForNewOriginNode(TNode node);
        
        public List<TEdge> GetPathTo(int destinationId);

        public bool AddAFrontierNode(
            TNode newFrontierNode,
            TNode fromNode,
            float edgeWeight,
            float costToNode, float heuristicCost);

        public bool TryGetNodeWithMinimumCost(out TNode nodeToProcess);

        public List<TEdge> GetEdgesOriginatingFromNode(TNode node);
    }
}
