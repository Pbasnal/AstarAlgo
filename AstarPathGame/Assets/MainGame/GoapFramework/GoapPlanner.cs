﻿using System.Collections.Generic;

namespace GoapFramework
{
    public class GoapPlanner
    {
        private IAgentAction[] _allActions;
        public List<IAgentAction> computedPath;
        private GoapData _goapData;

        public GoapPlanner(GoapData goapData, IAgentAction[] allActions)
        {
            _allActions = allActions;
            _goapData = goapData;
        }

        public List<IAgentAction> FindActionsTo(IAgentState currentState,
            IAgentState destinationNode)
        {
            _goapData.Reset();
            TryAddFrontierNode(null, currentState, destinationNode, null);

            GoapNode stateToCheck;
            while (_goapData.TryGetNodeWithMinimumCost(out stateToCheck))
            {
                if (stateToCheck.NodeData.Contains(destinationNode)) break;

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

        private float TryAddFrontierNode(GoapNode fromNode,
            IAgentState newNode,
            IAgentState destinationNode,
            IAgentAction edge)
        {
            var heuristicCost = newNode.DistanceFrom(destinationNode);
            var edgeWeight = edge == null ? 0.0f : edge.Weight;
            var nodeCost = _goapData.GetNodeCostOf(fromNode) + edgeWeight;

            if (!_goapData.ShouldAddNode(newNode, nodeCost + heuristicCost)) return 0;

            var newFrontierNode = GoapNode.New(newNode, nodeCost, heuristicCost, fromNode);

            newFrontierNode.Action = edge;
            _goapData.AddAFrontierNode(newFrontierNode);

            return heuristicCost;
        }

        public List<IAgentAction> GetEdgesOriginatingFromNode(IAgentState node)
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
