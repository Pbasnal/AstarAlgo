using System.Collections.Generic;
using InsightsLogger;
using System.Diagnostics;

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
            var timer = Stopwatch.StartNew();
            _astarData.ResetForNewOriginNode(originNode);

            var heuristicCost = HeuristicCost(_astarData.Nodes[originNode.Id], destinationNode);
            _astarData.AddAFrontierNode(originNode, originNode, 0, 0, heuristicCost);
            while (_astarData.TryGetNodeWithMinimumCost(out originNode))
            {
                var edgesOriginatingFromNode = _astarData.GetEdgesOriginatingFromNode(originNode);

                foreach (var edge in edgesOriginatingFromNode)
                {
                    var tryAddFrontier = ExecutionTimer.Time(() => TryAddFrontierNode(originNode, destinationNode, edge));
                    //RuntimeLogger.LogDebug("Pathfinding", $"TryAddFrontierNode time {tryAddFrontier.TotalMilliseconds}", heuristicCost);
                }
                _astarData.SetNodeVisited(originNode);

                if (originNode.Id == destinationNode.Id)
                {
                    // why to move anywhere else from here.
                    break;
                }
                //RuntimeLogger.LogDebug("Pathfinding", $"Loop time {timer.Elapsed.TotalMilliseconds}", timer);
                timer.Restart();
            }

            var computePathTime = ExecutionTimer.Time(
                    () => _astarData.GetPathTo(destinationNode.Id),
                    out var path);
            //RuntimeLogger.LogDebug("Pathfinding", $"ComputePath time {computePathTime.TotalMilliseconds}", path);

            return path;
        }

        private double TryAddFrontierNode(TNode originNode, TNode destinationNode, TEdge edge)
        {
            _timer.Start();
            double heuristicCost = HeuristicCost(_astarData.Nodes[edge.DestinationNode.Id], destinationNode);
            var nodeCost = _astarData.GetNodeCostOf(originNode) + edge.Weight;
            //RuntimeLogger.LogDebug("Pathfinding", $"Cost calculation time {_timer.Elapsed.TotalMilliseconds}", heuristicCost);
            _timer.Restart();
            
            var nodeGotAdded = _astarData.AddAFrontierNode(
                _astarData.Nodes[edge.DestinationNode.Id],
                originNode,
                edge.Weight,
                nodeCost, heuristicCost);
            
            //RuntimeLogger.LogDebug("Pathfinding", $"Addfrontier time {_timer.Elapsed.TotalMilliseconds}", heuristicCost);
            _timer.Restart();

            if (nodeGotAdded)
            {
                _astarData.Nodes[edge.DestinationNode.Id].HeuristicCost = heuristicCost;
                _astarData.Nodes[edge.DestinationNode.Id].NodeCost = nodeCost;
                _astarData.Nodes[edge.DestinationNode.Id].PreviousNode = originNode.Id;

                //int originId = originNode.Id;
                //int destId = _astarData.Nodes[edge.DestinationNode.Id].Id;
                //RuntimeLogger.LogDebug("Pathfinding", "AddedFrontier",
                //    ListWrapper<int>.Wrap(originId, destId));
            }

            return heuristicCost;
        }

        public abstract double HeuristicCost(TNode fromNode, TNode toNode);
    }
}
