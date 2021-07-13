using System.Collections.Generic;
using InsightsLogger;

namespace Pathfinding
{
    // Adjaceny list implementation
    public abstract class AstarPath<TNode, TEdge>
        where TNode : INode
        where TEdge : IWeightedEdge<TNode>
    {
        public List<TEdge> computedPath;
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

            var heuristicCost = HeuristicCost(_astarData.Nodes[originNode.Id], destinationNode);
            _astarData.AddAFrontierNode(originNode, originNode, 0, 0, heuristicCost);
            while (_astarData.TryGetNodeWithMinimumCost(out originNode))
            {
                var edgesOriginatingFromNode = _astarData.GetEdgesOriginatingFromNode(originNode);

                computedPath = _astarData.GetPathTo(originNode.Id);

                foreach (var edge in edgesOriginatingFromNode)
                {
                    heuristicCost = HeuristicCost(_astarData.Nodes[edge.DestinationNode.Id], destinationNode);
                    var nodeCost = _astarData.GetNodeCostOf(originNode) + edge.Weight;

                    var nodeGotAdded = _astarData.AddAFrontierNode(
                        _astarData.Nodes[edge.DestinationNode.Id],
                        originNode,
                        edge.Weight,
                        nodeCost, heuristicCost);

                    if (nodeGotAdded)
                    {
                        _astarData.Nodes[edge.DestinationNode.Id].HeuristicCost = heuristicCost;
                        _astarData.Nodes[edge.DestinationNode.Id].NodeCost = nodeCost;
                        _astarData.Nodes[edge.DestinationNode.Id].PreviousNode = originNode.Id;

                        int originId = originNode.Id;
                        int destId = _astarData.Nodes[edge.DestinationNode.Id].Id;
                        RuntimeLogger.LogDebug("Pathfinding", "AddedFrontier", 
                            ListWrapper<int>.Wrap(originId, destId));
                    }
                }
                _astarData.SetNodeVisited(originNode);
                
                if (originNode.Id == destinationNode.Id)
                {
                    // why to move anywhere else from here.
                    break;
                }
            }

            return _astarData.GetPathTo(destinationNode.Id);
        }

        public abstract double HeuristicCost(TNode fromNode, TNode toNode);
    }
}
