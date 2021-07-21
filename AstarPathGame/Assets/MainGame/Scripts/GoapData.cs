using System.Collections.Generic;
using Pathfinding;

namespace MainGame
{
    public class GoapData// : IAstarData<AgentState, AgentAction>
    {
        public Dictionary<int, AgentState> Nodes { get; }
        public AgentAction[] Edges { get; }

        public AgentState[] FrontierNodes => _frontierNodes._elements;

        private SimpleMinHeap<AgentState> _frontierNodes;
        private float[] _nodeCost;
        //private float[] _pathCost;
        private bool[] _visitedNodes;

        private Dictionary<int, List<AgentAction>> _nodeEdges;
        private static readonly List<AgentAction> _emptyEdgeList = new List<AgentAction>();

        public void ResetForNewOriginNode(AgentState node)
        { }

        public bool AddAFrontierNode(
            AgentState newFrontierNode,
            AgentAction withAction,
            float costToNode, float heuristicCost)
        {
            // assign id if it's a new node
            if (newFrontierNode.Id == -1)
            {
                newFrontierNode.Id = Nodes.Count;
                Nodes.Add(newFrontierNode.Id, newFrontierNode);
            }
            if (costToNode > Nodes[newFrontierNode.Id].NodeCost
                || Nodes[newFrontierNode.Id].IsVisited)
            {
                return false;
            }

            Nodes[newFrontierNode.Id].Priority = 
                costToNode + heuristicCost;
            Nodes[newFrontierNode.Id].ActionForThisNode = withAction;
            Nodes[newFrontierNode.Id].HeuristicCost = heuristicCost;
            Nodes[newFrontierNode.Id].NodeCost = costToNode;

            _frontierNodes.Add(newFrontierNode);
            return true;
        }

        public bool TryGetNodeWithMinimumCost(out AgentState nodeToProcess)
        {
            if (_frontierNodes.IsEmpty())
            {
                nodeToProcess = default(AgentState);
                return false;
            }

            nodeToProcess = _frontierNodes.Pop();
            return true;
        }

        public List<AgentAction> GetEdgesOriginatingFromNode(AgentState node)
        {
            List<AgentAction> actions = new List<AgentAction>();
            foreach (var action in Edges)
            {
                if (action.CheckPreconditions(node))
                {
                    actions.Add(action);
                }
            }

            return actions;
        }

        public List<AgentAction> GetPathTo(int destinationId)
        {
            var path = new List<AgentAction>();

            var startingNodeId = destinationId;

            //while (startingNodeId != nextNodeInPathId && nextNodeInPathId != -1)
            {
                //var edgeInPath = new AgentAction();
                //edgeInPath.OriginNode = Nodes[nextNodeInPathId];
                //edgeInPath.DestinationNode = Nodes[startingNodeId];

                //path.Insert(0, edgeInPath);

                //startingNodeId = nextNodeInPathId;
                //nextNodeInPathId = _path[Nodes[nextNodeInPathId].Id];
            }

            return path;
        }

        public float GetNodeCostOf(AgentState node) => Nodes[node.Id].NodeCost;

        public void SetNodeVisited(AgentState node) => Nodes[node.Id].IsVisited = true;
    }
}
