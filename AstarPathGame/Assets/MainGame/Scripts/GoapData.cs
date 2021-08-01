using System.Collections.Generic;
using Pathfinding;

namespace MainGame
{
    public class GoapData
    {
        public AgentState currentState;

        public Dictionary<int, AgentState> Nodes { get; }
        public IAgentAction[] Actions { get; }

        public AgentState[] FrontierNodes => _frontierNodes._elements;

        private SimpleMinHeap<AgentState> _frontierNodes;

        private Dictionary<int, List<IAgentAction>> _nodeEdges;
        private static readonly List<IAgentAction> _emptyEdgeList = new List<IAgentAction>();

        public GoapData(IAgentAction[] agentActions)
        {
            Actions = agentActions;
            currentState = AgentState.New();
            Nodes = new Dictionary<int, AgentState>();
            _frontierNodes = new SimpleMinHeap<AgentState>(1000);
        }

        public void SetState(AgentStateKey stateKey, int stateValue)
        {
            currentState.Set(stateKey, stateValue);
        }

        public void ResetForNewOriginNode(AgentState node)
        {
            Nodes.Clear();
            //Nodes.Add(node.Id, node);
        }

        public bool AddAFrontierNode(
            AgentState fromNode,
            AgentState newFrontierNode,
            IAgentAction withAction,
            float costToNode, float heuristicCost)
        {
            // assign id if it's a new node
            if (newFrontierNode.Id == -1)
            {
                newFrontierNode.Id = Nodes.Count;
                Nodes.Add(newFrontierNode.Id, newFrontierNode);
            }
            else if (costToNode > Nodes[newFrontierNode.Id].NodeCost
                || Nodes[newFrontierNode.Id].IsVisited)
            {
                return false;
            }

            Nodes[newFrontierNode.Id].Priority =
                costToNode + heuristicCost;
            Nodes[newFrontierNode.Id].ActionForThisNode = withAction;
            Nodes[newFrontierNode.Id].HeuristicCost = heuristicCost;
            Nodes[newFrontierNode.Id].NodeCost = costToNode;
            Nodes[newFrontierNode.Id].PreviousNode = fromNode == null ? 0 : fromNode.Id;

            _frontierNodes.Add(newFrontierNode);
            return true;
        }

        public bool TryGetNodeWithMinimumCost(out AgentState nodeToProcess)
        {
            if (_frontierNodes.IsEmpty())
            {
                nodeToProcess = null;
                return false;
            }

            nodeToProcess = _frontierNodes.Pop();
            return true;
        }

        public List<IAgentAction> GetEdgesOriginatingFromNode(AgentState node)
        {
            List<IAgentAction> actions = new List<IAgentAction>();
            foreach (var action in Actions)
            {
                if (action.CheckPreconditions(node))
                {
                    actions.Add(action);
                }
            }

            return actions;
        }

        public List<IAgentAction> GetPathTo(int destinationId)
        {
            var path = new List<IAgentAction>();

            var startingNodeId = destinationId;
            var nextNodeInPathId = Nodes[destinationId].PreviousNode;
            path.Insert(0, Nodes[destinationId].ActionForThisNode);

            while (startingNodeId != nextNodeInPathId
                && nextNodeInPathId != -1
                && nextNodeInPathId != 0)
            {
                var edgeInPath = Nodes[nextNodeInPathId].ActionForThisNode;
                path.Insert(0, edgeInPath);

                startingNodeId = nextNodeInPathId;
                nextNodeInPathId = Nodes[nextNodeInPathId].PreviousNode;
            }

            return path;
        }

        public float GetNodeCostOf(AgentState node) => Nodes[node.Id].NodeCost;

        public void SetNodeVisited(AgentState node) => Nodes[node.Id].IsVisited = true;
    }
}
