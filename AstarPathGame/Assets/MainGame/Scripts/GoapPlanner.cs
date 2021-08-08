using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class GoapPlanner
    {
        private IAgentAction[] _allActions;
        public List<IAgentAction> computedPath;
        private GoapData<AgentState> _goapData;

        public GoapPlanner(GoapData<AgentState> goapData, IAgentAction[] allActions)
        {
            _allActions = allActions;
            _goapData = goapData;
        }

        public List<IAgentAction> FindActionsTo(AgentState currentState, AgentState destinationNode)
        {
            _goapData.Reset();

            TryAddFrontierNode(null, currentState, destinationNode, null);

            GoapNode<AgentState> stateToCheck;
            while (_goapData.TryGetNodeWithMinimumCost(out stateToCheck))
            {
                var stateValue = stateToCheck.NodeData.StateValue & destinationNode.StateValue;
                if (stateValue == destinationNode.StateValue) break;

                var actions = GetEdgesOriginatingFromNode(stateToCheck.NodeData);
                foreach (var action in actions)
                {
                    var newState = action.GetGeneratedState(stateToCheck.NodeData);
                    TryAddFrontierNode(stateToCheck, newState, destinationNode, action);
                }
                try
                {
                    _goapData.SetNodeVisited(stateToCheck);
                }
                catch (System.Exception up)
                {
                    Debug.Log($"Caught {up}");
                    throw up;
                }
            }

            return _goapData.GetPathTo(stateToCheck);
        }

        private float TryAddFrontierNode(GoapNode<AgentState> fromNode,
            AgentState newNode,
            AgentState destinationNode,
            IAgentAction edge)
        {
            var heuristicCost = HeuristicCost(newNode, destinationNode);
            var edgeWeight = edge == null ? 0.0f : edge.Weight;
            var nodeCost = _goapData.GetNodeCostOf(fromNode) + edgeWeight;

            if (!_goapData.ShouldAddNode(newNode, nodeCost + heuristicCost)) return 0;

            var newFrontierNode = GoapNode<AgentState>
                        .New(newNode, nodeCost, heuristicCost, fromNode);

            newFrontierNode.Action = edge;
            _goapData.AddAFrontierNode(newFrontierNode);

            return heuristicCost;
        }

        public float HeuristicCost(AgentState fromNode, AgentState toNode)
        {
            int XOR = toNode.AgentStateValue() ^ fromNode.AgentStateValue();
            // Check for 1's in the binary form using
            // Brian Kerninghan's Algorithm
            int count = 0;
            while (XOR > 0)
            {
                XOR &= (XOR - 1);
                count++;
            }
            // return the count of different bits
            return count;
        }

        public List<IAgentAction> GetEdgesOriginatingFromNode(AgentState node)
        {
            var actions = new List<IAgentAction>();
            foreach (var action in _allActions)
            {
                if (action.ValidateAction(node))
                {
                    actions.Add(action);
                }
            }

            return actions;
        }
    }
}

