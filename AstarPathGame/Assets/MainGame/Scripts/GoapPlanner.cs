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
                var stateValue = stateToCheck.NodeData.State.StateValue & destinationNode.State.StateValue;
                if (stateValue == destinationNode.State.StateValue) break;

                var actions = GetEdgesOriginatingFromNode(stateToCheck.NodeData);
                foreach (var action in actions)
                {
                    var newState = action.GetGeneratedState(stateToCheck.NodeData);
                    TryAddFrontierNode(stateToCheck, newState, destinationNode, action);
                }

                _goapData.SetNodeVisited(stateToCheck);
            }

            return _goapData.GetPathTo(stateToCheck);
        }

        private float TryAddFrontierNode(GoapNode<AgentState> fromNode,
            AgentState newNode, 
            AgentState destinationNode,
            IAgentAction edge)
        {
            if (newNode == null || 
                _goapData.IsNodeVisited(newNode))
            {
                return 0;
            }

            var heuristicCost = HeuristicCost(newNode, destinationNode);
            var edgeWeight = edge == null ? 0.0f : edge.Weight;
            var nodeCost = _goapData.GetNodeCostOf(fromNode) + edgeWeight;

            var newFrontierNode = GoapNode<AgentState>
                        .New(newNode, nodeCost, heuristicCost, fromNode);
            
            newFrontierNode.Action = edge;
            _goapData.AddAFrontierNode(newFrontierNode);

            return heuristicCost;
        }

        public float HeuristicCost(AgentState fromNode, AgentState toNode)
        {
            var diff = toNode.AgentStateValue() - fromNode.AgentStateValue();
            return diff * diff;
        }

        public List<IAgentAction> GetEdgesOriginatingFromNode(AgentState node)
        {
            var actions = new List<IAgentAction>();
            foreach (var action in _allActions)
            {
                if (action.CheckPreconditions(node))
                {
                    actions.Add(action);
                }
            }

            return actions;
        }
    }
}

