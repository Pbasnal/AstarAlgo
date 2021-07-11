using System.Collections.Generic;

namespace Pathfinding
{
    // Adjaceny list implementation
    public abstract class AstarPath<TNode, TEdge>
        where TNode : INode, new()
        where TEdge : IWeightedEdge<TNode>, new()
    {
        private IAstarData<TNode, TEdge> _astarData;

        public AstarPath(IAstarData<TNode, TEdge> astarData)
        {
            _astarData = astarData;
        }

        public List<TEdge> FindPathBetween(int originNodeIndex, int destinationNodeIndex)
        {
            var originNode = _astarData.Nodes[originNodeIndex];
            var destinationNode = _astarData.Nodes[destinationNodeIndex];

            return FindPathBetweenNodes(originNode, destinationNode);
        }

        public List<TEdge> FindPathBetweenNodes(TNode originNode, TNode destinationNode)
        {
            _astarData.ResetForNewOriginNode(originNode);

            _astarData.AddAFrontierNode(originNode, originNode, 0, 0);
            while (_astarData.TryGetNodeWithMinimumCost(out originNode))
            {
                if (originNode.Id == destinationNode.Id)
                {
                    // why to move anywhere else from here.
                    continue;
                }
                var edgesOriginatingFromNode = _astarData.GetEdgesOriginatingFromNode(originNode);
                foreach (var edge in edgesOriginatingFromNode)
                {
                    var newFrontierNode = _astarData.Nodes[edge.DestinationNode.Id];
                    var costToNode = (_astarData.GetNodeCostOf(originNode) + edge.Weight) +
                        HeuristicCost(newFrontierNode, destinationNode);

                    _astarData.AddAFrontierNode(
                        newFrontierNode,
                        originNode,
                        edge.Weight,
                        costToNode);
                }
                _astarData.SetNodeVisited(originNode);
            }

            return _astarData.GetPathTo(destinationNode.Id);
        }

        public abstract double HeuristicCost(TNode fromNode, TNode toNode);
    }
}
