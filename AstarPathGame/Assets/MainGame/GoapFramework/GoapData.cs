using System.Collections.Generic;
using Pathfinding;

namespace GoapFramework
{
    public class GoapData
    {
        private readonly SimpleMinHeap<GoapNode> _frontierNodes;

        private Dictionary<int, GoapNode> _processedNodes;

        public GoapData()
        {
            _frontierNodes = new SimpleMinHeap<GoapNode>(1000);
            _processedNodes = new Dictionary<int, GoapNode>();
        }

        public void Reset()
        {
            _processedNodes.Clear();
            _frontierNodes.Reset();
        }

        public bool AddAFrontierNode(GoapNode newFrontierNode)
        {
            var nodePriority = newFrontierNode.NodeCost + newFrontierNode.HeuristicCost;
            _frontierNodes.Add(nodePriority, newFrontierNode);

            return true;
        }

        public bool IsNodeVisited(IAgentState nodeData)
        {
            var nodeKey = nodeData.GetHashCode();
            return _processedNodes.TryGetValue(nodeKey, out var node)
                && node.IsVisited;
        }

        public bool ShouldAddNode(IAgentState nodeData, float priority)
        {
            var nodeKey = nodeData.GetHashCode();            
            if(!_processedNodes.TryGetValue(nodeKey, out var node)) return true;

            return priority < node.NodeCost + node.HeuristicCost;
        }

        public bool TryGetNodeWithMinimumCost(out GoapNode nodeToProcess)
        {
            if (_frontierNodes.IsEmpty())
            {
                nodeToProcess = null;
                return false;
            }

            nodeToProcess = _frontierNodes.Pop().Data;
            return true;
        }

        public List<IAgentAction> GetPathTo(GoapNode destinationNode)
        {
            var path = new List<IAgentAction>();

            while (destinationNode?.Action != null)
            {
                path.Insert(0, destinationNode.Action);
                destinationNode = destinationNode.PreviousNode;
            }

            return path;
        }

        public float GetNodeCostOf(GoapNode node)
        {
            return node == null ? 0 : node.NodeCost;
        }

        public void SetNodeVisited(GoapNode node)
        {
            node.IsVisited = true;
            var nodeKey = node.NodeData.GetHashCode();
            if(_processedNodes.ContainsKey(nodeKey)) 
            {
                _processedNodes[nodeKey] = node;
                return;
            }
            
            _processedNodes.Add(nodeKey, node);
        }
    }
}
