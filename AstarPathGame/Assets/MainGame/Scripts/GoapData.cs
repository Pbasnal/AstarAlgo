using System.Collections.Generic;
using Pathfinding;

namespace MainGame
{
    public class GoapNode<T> where T : IAgentState<T>
    {
        public GoapNode<T> PreviousNode { get; set; }
        public IAgentAction<T> Action { get; set; }
        public float NodeCost { get; set; }
        public float HeuristicCost { get; set; }
        public T NodeData { get; set; }
        public bool IsVisited { get; set; }

        public static GoapNode<T> New(T data, float cost, float heuristicCost, GoapNode<T> prev)
        {
            return new GoapNode<T>
            {
                NodeData = data,
                NodeCost = cost,
                HeuristicCost = heuristicCost,
                PreviousNode = prev
            };
        }
    }

    public class GoapData<T> where T: IAgentState<T>
    {
        private readonly SimpleMinHeap<GoapNode<T>> _frontierNodes;

        private Dictionary<int, GoapNode<T>> _processedNodes;

        public GoapData()
        {
            _frontierNodes = new SimpleMinHeap<GoapNode<T>>(1000);
            _processedNodes = new Dictionary<int, GoapNode<T>>();
        }

        public void Reset()
        {
            _processedNodes.Clear();
            _frontierNodes.Reset();
        }

        public bool AddAFrontierNode(GoapNode<T> newFrontierNode)
        {
            var nodePriority = newFrontierNode.NodeCost + newFrontierNode.HeuristicCost;
            _frontierNodes.Add(nodePriority, newFrontierNode);

            return true;
        }

        public bool IsNodeVisited(T nodeData)
        {
            var nodeKey = nodeData.GetHashCode();
            return _processedNodes.TryGetValue(nodeKey, out var node)
                && node.IsVisited;
        }

        public bool ShouldAddNode(T nodeData, float priority)
        {
            var nodeKey = nodeData.GetHashCode();            
            if(!_processedNodes.TryGetValue(nodeKey, out var node)) return true;

            return priority < node.NodeCost + node.HeuristicCost;
        }

        public bool TryGetNodeWithMinimumCost(out GoapNode<T> nodeToProcess)
        {
            if (_frontierNodes.IsEmpty())
            {
                nodeToProcess = null;
                return false;
            }

            nodeToProcess = _frontierNodes.Pop().Data;
            return true;
        }

        public List<IAgentAction<T>> GetPathTo(GoapNode<T> destinationNode)
        {
            var path = new List<IAgentAction<T>>();

            while (destinationNode?.Action != null)
            {
                path.Insert(0, destinationNode.Action);
                destinationNode = destinationNode.PreviousNode;
            }

            return path;
        }

        public float GetNodeCostOf(GoapNode<T> node)
        {
            return node == null ? 0 : node.NodeCost;
        }

        public void SetNodeVisited(GoapNode<T> node)
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
