using System.Collections.Generic;
using InsightsLogger;
using System.Diagnostics;
using System;

namespace Pathfinding
{
    // Adjaceny list implementation
    public abstract class AstarPath<TNode, TEdge>
        where TNode : INode
        where TEdge : IWeightedEdge<TNode>
    {
        public List<TEdge> computedPath;
        private IAstarData<TNode, TEdge> _astarData;

        private Stopwatch _timer = new Stopwatch();

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
                if (originNode.Id == destinationNode.Id)
                {
                    break;
                }
                var edgesOriginatingFromNode = _astarData.GetEdgesOriginatingFromNode(originNode);

                foreach (var edge in edgesOriginatingFromNode)
                {
                    var tryAddFrontier = ExecutionTimer.Time(() => TryAddFrontierNode(originNode, destinationNode, edge));
                }
                _astarData.SetNodeVisited(originNode);
            }

            List<TEdge> path = null;
            var computePathTime = ExecutionTimer.Time(() =>
                path = _astarData.GetPathTo(destinationNode.Id));

            return path;
        }

        private float TryAddFrontierNode(TNode originNode, TNode destinationNode, TEdge edge)
        {
            float heuristicCost = HeuristicCost(_astarData.Nodes[edge.DestinationNode.Id], destinationNode);
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
            }

            return heuristicCost;
        }

        public abstract float HeuristicCost(TNode fromNode, TNode toNode);
    }
}
