using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using InsightsLogger;

namespace Pathfinding
{
    public interface INodeWithPriority : IMinHeapNode, INode
    {
        public bool IsVisited { get; set; }
    }

    public class AstarDataWithHeap<TNode, TEdge> : IAstarData<TNode, TEdge>
        where TNode : INodeWithPriority, new()
        where TEdge : IWeightedEdge<TNode>, new()
    {
        public TNode[] Nodes { get; }
        public TEdge[] Edges { get; }

        private SimpleMinHeap<TNode> _frontierNodes;
        private float[] _nodeCost;
        private int[] _path;
        private float[] _pathCost;
        private bool[] _visitedNodes;

        private Dictionary<int, List<TEdge>> _nodeEdges;
        private static readonly List<TEdge> _emptyEdgeList = new List<TEdge>();

        public AstarDataWithHeap(TNode[] nodes, TEdge[] edges)
        {
            Edges = edges;
            Nodes = nodes;

            for (int i = 0; i < Nodes.Length; i++)
            {
                Nodes[i].Id = i;
            }

            _visitedNodes = new bool[nodes.Length];
            _path = new int[nodes.Length];
            _nodeCost = new float[nodes.Length];
            _pathCost = new float[nodes.Length];

            _frontierNodes = new SimpleMinHeap<TNode>(nodes.Length);

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

        public void ResetForNewOriginNode(TNode node)
        {
            int id = node.Id;
            for (int i = 0; i < _path.Length; i++)
            {
                _path[i] = -1;
                _nodeCost[i] = float.MaxValue;
                _visitedNodes[i] = false;
            }
            _nodeCost[id] = 0;
            _frontierNodes._size = 0;
        }

        public bool AddAFrontierNode(
            TNode newFrontierNode,
            TNode fromNode,
            float edgeWeight,
            float costToNode, float heuristicCost)
        {
            if (costToNode > _nodeCost[newFrontierNode.Id]
                || _visitedNodes[newFrontierNode.Id])
            {
                return false;
            }

            newFrontierNode.PreviousNode = fromNode.Id;

            _frontierNodes.Add(costToNode + heuristicCost, newFrontierNode);

            _path[newFrontierNode.Id] = fromNode.Id;
            _nodeCost[newFrontierNode.Id] = costToNode;
            _pathCost[newFrontierNode.Id] = edgeWeight;
            
            return true;
        }

        public bool TryGetNodeWithMinimumCost(out TNode nodeToProcess)
        {
            if (_frontierNodes.IsEmpty())
            {
                nodeToProcess = default(TNode);
                return false;
            }

            nodeToProcess = _frontierNodes.Pop().Data;
            return true;
        }

        public List<TEdge> GetEdgesOriginatingFromNode(TNode node)
        {
            if (_nodeEdges.TryGetValue(node.Id, out var edges))
            {
                return edges;
            }

            return _emptyEdgeList;
        }

        public List<TEdge> GetPathTo(int destinationId)
        {
            var path = new List<TEdge>();

            var startingNodeId = destinationId;
            var nextNodeInPathId = _path[destinationId];

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

        public float GetNodeCostOf(TNode node) => _nodeCost[node.Id];

        public void SetNodeVisited(TNode node) => _visitedNodes[node.Id] = true;
    }
}
