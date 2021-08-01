using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class GoapPlanner
    {
        public List<IAgentAction> computedPath;
        private GoapData _goapData;

        public GoapPlanner(GoapData goapData)
        {
            _goapData = goapData;
        }

        public List<IAgentAction> FindActionsTo(AgentState destinationNode)
        {
            _goapData.ResetForNewOriginNode(_goapData.currentState);
            var heuristicCost = HeuristicCost(_goapData.currentState, destinationNode);
            _goapData.AddAFrontierNode(null, _goapData.currentState, null, 0, heuristicCost);

            AgentState stateToCheck;
            while (_goapData.TryGetNodeWithMinimumCost(out stateToCheck))
            {
                heuristicCost = HeuristicCost(stateToCheck, destinationNode);
                if (heuristicCost == 0) break;

                var actions = _goapData.GetEdgesOriginatingFromNode(stateToCheck);
                foreach (var action in actions)
                {
                    var newState = action.GetGeneratedState(stateToCheck);
                    TryAddFrontierNode(stateToCheck, newState, action);
                }

                _goapData.SetNodeVisited(stateToCheck);
            }

            return _goapData.GetPathTo(stateToCheck.Id);
        }

        private float TryAddFrontierNode(AgentState originNode,
            AgentState newNode, IAgentAction edge)
        {
            if (newNode == null)
            {
                return 0;
            }
            var heuristicCost = HeuristicCost(originNode, newNode);
            var nodeCost = _goapData.GetNodeCostOf(originNode) + edge.Weight;

            var nodeGotAdded = _goapData.AddAFrontierNode(
                originNode, newNode, edge, nodeCost, heuristicCost);

            //if (nodeGotAdded)
            //{
            //    _astarData.Nodes[edge.DestinationNode.Id].PreviousNode = originNode.Id;
            //}

            return heuristicCost;
        }

        public float HeuristicCost(AgentState fromNode, AgentState toNode)
        {
            var distance = 0.0f;

            foreach (var tostate in toNode.GetStateElements())
            {
                float diff;
                if (!fromNode.ContainsKey(tostate.stateName))
                {
                    diff = 1;
                }
                else
                {
                    var toNodeValue = toNode.Get(tostate.stateName);
                    var fromNodeValue = fromNode.Get(tostate.stateName);
                    diff = toNodeValue - fromNodeValue;
                }
                distance += diff * diff;
            }

            return Mathf.Sqrt(distance);
        }
    }
}

