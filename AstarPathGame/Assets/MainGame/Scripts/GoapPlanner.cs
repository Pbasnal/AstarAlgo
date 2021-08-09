using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class GoapPlanner<TAgentState> 
        where TAgentState: IAgentState<TAgentState>
    {
        private IAgentAction<TAgentState>[] _allActions;
        public List<IAgentAction<TAgentState>> computedPath;
        private GoapData<TAgentState> _goapData;

        public GoapPlanner(GoapData<TAgentState> goapData, 
            IAgentAction<TAgentState>[] allActions)
        {
            _allActions = allActions;
            _goapData = goapData;
        }

        public List<IAgentAction<TAgentState>> FindActionsTo(TAgentState currentState, 
            TAgentState destinationNode)
        {
            _goapData.Reset();
            TryAddFrontierNode(null, currentState, destinationNode, null);

            GoapNode<TAgentState> stateToCheck;
            while (_goapData.TryGetNodeWithMinimumCost(out stateToCheck))
            {
                if (stateToCheck.NodeData.Contains(destinationNode)) break;

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

        private float TryAddFrontierNode(GoapNode<TAgentState> fromNode,
            TAgentState newNode,
            TAgentState destinationNode,
            IAgentAction<TAgentState> edge)
        {
            var heuristicCost = newNode.DistanceFrom(destinationNode);
            var edgeWeight = edge == null ? 0.0f : edge.Weight;
            var nodeCost = _goapData.GetNodeCostOf(fromNode) + edgeWeight;

            if (!_goapData.ShouldAddNode(newNode, nodeCost + heuristicCost)) return 0;

            var newFrontierNode = GoapNode<TAgentState>
                        .New(newNode, nodeCost, heuristicCost, fromNode);

            newFrontierNode.Action = edge;
            _goapData.AddAFrontierNode(newFrontierNode);

            return heuristicCost;
        }

        public List<IAgentAction<TAgentState>> GetEdgesOriginatingFromNode(TAgentState node)
        {
            var actions = new List<IAgentAction<TAgentState>>();
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

