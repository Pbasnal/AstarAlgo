using System;
using System.Collections.Generic;
using System.Linq;

namespace AstarAlgo
{
    public interface INode
    {
        public string NodeId { get; }
        protected internal int Id { get; set; }
    }

    public interface IEdge
    {
        public INode OriginNode { get; set; }
        public INode DestinationNode { get; set; }
    }

    public interface IWeightedEdge : IEdge
    {
        public double Weight { get; set; }
    }

    // Adjaceny list implementation
    public abstract class AstarPath<TNode, TEdge>
        where TNode : INode, new()
        where TEdge : IWeightedEdge, new()
    {
        public TNode[] Nodes;
        public TEdge[] Edges;

        private List<TNode> _frontierNodes;
        private double[] _nodeCost;
        private int[] _path;
        private double[] _pathCost;
        private bool[] _visitedNodes;

        private Dictionary<int, List<TEdge>> _nodeEdges;

        private static readonly List<TEdge> _emptyEdgeList = new List<TEdge>();

        public AstarPath(List<TNode> nodes, List<TEdge> edges)
        {
            Edges = edges.ToArray();
            Nodes = nodes.Select(n => (TNode)n).ToArray();

            for (int i = 0; i < Nodes.Length; i++)
            {
                Nodes[i].Id = i;
            }

            _visitedNodes = new bool[nodes.Count];
            _path = new int[nodes.Count];
            _nodeCost = new double[nodes.Count];
            _pathCost = new double[nodes.Count];

            _frontierNodes = new List<TNode>();

            _nodeEdges = new Dictionary<int, List<TEdge>>();
            foreach (var edge in Edges)
            {
                var originNode = edge.OriginNode;
                if (!_nodeEdges.ContainsKey(edge.OriginNode.Id))
                {
                    _nodeEdges.Add(originNode.Id, new List<TEdge>());
                }

                _nodeEdges[originNode.Id].Add(edge);
            }
        }

        public List<TEdge> FindPathBetween(int originNodeIndex, int destinationNodeIndex)
        {
            var path = new List<TEdge>();
            var nodeToProcess = (TNode)Nodes[originNodeIndex];
            var destinationNode = (TNode)Nodes[destinationNodeIndex];

            ClearVisitedNodes();
            ResetNodeCosts();
            ResetPath();
            _nodeCost[nodeToProcess.Id] = 0; // Set starting node cost as 0

            AddToFrontierNodes(nodeToProcess, nodeToProcess, 0);
            while (TryGetNodeWithMinimumCost(out nodeToProcess))
            {
                if (nodeToProcess.Id == destinationNode.Id)
                {
                    // why to move anywhere else from here.
                    continue;
                }
                var edgesOriginatingFromNode = GetEdgesOriginatingFromNode(nodeToProcess);
                foreach (var edge in edgesOriginatingFromNode)
                {
                    var newFrontierNode = Nodes[edge.DestinationNode.Id];
                    var costToNode = (_nodeCost[nodeToProcess.Id] + edge.Weight) +
                        HeuristicCost(newFrontierNode, destinationNode);

                    if (AddToFrontierNodes(newFrontierNode, nodeToProcess, costToNode))
                    {
                        _pathCost[newFrontierNode.Id] = edge.Weight;
                    }
                }
                _visitedNodes[nodeToProcess.Id] = true;
            }

            var startingNodeId = destinationNode.Id;
            var nextNodeInPathId = _path[destinationNode.Id];

            while (startingNodeId != nextNodeInPathId && nextNodeInPathId != -1)
            {
                var edgeInPath = new TEdge();
                edgeInPath.OriginNode = Nodes[nextNodeInPathId];
                edgeInPath.DestinationNode = Nodes[startingNodeId];
                edgeInPath.Weight = _pathCost[startingNodeId];

                path.Insert(0, edgeInPath);

                startingNodeId = nextNodeInPathId;
                nextNodeInPathId = _path[Nodes[nextNodeInPathId].Id];
            }

            return path;
        }

        private void ResetPath()
        {
            for (int i = 0; i < _path.Length; i++)
            {
                _path[i] = -1;
            }
        }

        private void ResetNodeCosts()
        {
            for (int i = 0; i < _nodeCost.Length; i++)
            {
                _nodeCost[i] = double.MaxValue;
            }
        }

        private void ClearVisitedNodes()
        {
            for (int i = 0; i < _visitedNodes.Length; i++)
            {
                _visitedNodes[i] = false;
            }
        }

        private bool TryGetNodeWithMinimumCost(out TNode nodeToProcess)
        {
            if (_frontierNodes.Count <= 0)
            {
                nodeToProcess = default(TNode);
                return false;

            }

            var minimumCost = double.MaxValue;
            var minimumCostNodeId = -1;

            for (int i = 0; i < _frontierNodes.Count; i++)
            {
                if (_nodeCost[_frontierNodes[i].Id] < minimumCost)
                {
                    minimumCost = _nodeCost[_frontierNodes[i].Id];
                    minimumCostNodeId = i;
                }
            }

            nodeToProcess = _frontierNodes[minimumCostNodeId];
            _frontierNodes.RemoveAt(minimumCostNodeId);
            return true;
        }

        private bool AddToFrontierNodes(TNode newFrontierNode, TNode fromNode, double cost)
        {
            if (cost > _nodeCost[newFrontierNode.Id]
                || _visitedNodes[newFrontierNode.Id])
            {
                return false;
            }

            _frontierNodes.Add(newFrontierNode);
            _path[newFrontierNode.Id] = fromNode.Id;
            _nodeCost[newFrontierNode.Id] = cost;

            return true;
        }

        private List<TEdge> GetEdgesOriginatingFromNode(TNode node)
        {
            if (_nodeEdges.TryGetValue(node.Id, out var edges))
            {
                return edges;
            }

            return _emptyEdgeList;
        }

        public abstract double HeuristicCost(TNode fromNode, TNode toNode);
    }
}
