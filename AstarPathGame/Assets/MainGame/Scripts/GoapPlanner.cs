using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace MainGame
{
    public class AgentState : INodeWithPriority
    {
        public static int StateSize = Enum.GetValues(typeof(AgentStateKey)).Length;

        public string NodeId => string.Empty;

        public bool IsVisited { get; set; }
        public int Id { get; set; }
        public AgentAction ActionForThisNode { get; set; }
        public float NodeCost { get; set; }
        public float HeuristicCost { get; set; }
        public float Priority { get; set; }
        public int PreviousNode { get; set; }

        public int[] stateData;

        private AgentState()
        {
            Id = -1;
            stateData = new int[StateSize];
        }

        public static AgentState New()
        {
            return new AgentState();
        }

        public void Set(AgentStateKey stateKey, int value)
        {
            stateData[(int)stateKey] = value;
        }

        public AgentState Clone()
        {
            var newState = new AgentState();

            for (int i = 0; i < StateSize; i++)
            {
                newState.stateData[i] = stateData[i];
            }

            return newState;
        }
    }

    public class AgentAction : IWeightedEdge<AgentState>
    {
        public float Weight { get; set; }

        public AgentState OriginNode { get; set; }
        public AgentState DestinationNode { get; set; }

        public AgentAction(AgentState originNode, AgentState destinationNode, float cost)
        {
            OriginNode = originNode;
            DestinationNode = destinationNode;

            Weight = cost;
        }

        internal bool CheckPreconditions(AgentState node)
        {
            throw new NotImplementedException();
        }
    }

    public class GoapPlanner
    {
        public List<AgentAction> computedPath;
        private GoapData _goapData;

        public GoapPlanner(GoapData goapData)
        {
            _goapData = goapData;
        }

        public AgentState FindPathBetweenNodes(AgentState originNode,
            AgentState destinationNode)
        {
            _goapData.ResetForNewOriginNode(originNode);

            var heuristicCost = HeuristicCost(_goapData.Nodes[originNode.Id],
                destinationNode);
            _goapData.AddAFrontierNode(originNode, null, 0, heuristicCost);
            while (_goapData.TryGetNodeWithMinimumCost(out originNode))
            {
                if (originNode.Id == destinationNode.Id)
                {
                    break;
                }
                var actions = _goapData.GetEdgesOriginatingFromNode(originNode);

                foreach (var edge in actions)
                {
                    TryAddFrontierNode(originNode, destinationNode, edge);
                }
                _goapData.SetNodeVisited(originNode);
            }

            //var path = _goapData.GetPathTo(destinationNode.Id);

            return destinationNode;
        }

        private float TryAddFrontierNode(AgentState originNode, 
            AgentState destinationNode, AgentAction edge)
        {
            float heuristicCost = HeuristicCost(originNode, destinationNode);
            var nodeCost = _goapData.GetNodeCostOf(originNode) + edge.Weight;

            var nodeGotAdded = _goapData.AddAFrontierNode(
                destinationNode, edge, nodeCost, heuristicCost);

            //if (nodeGotAdded)
            //{
            //    _astarData.Nodes[edge.DestinationNode.Id].PreviousNode = originNode.Id;
            //}

            return heuristicCost;
        }

        public float HeuristicCost(AgentState fromNode, AgentState toNode)
        {
            var distance = 0.0f;

            for (int i = 0; i < AgentState.StateSize; i++)
            {
                var diff = toNode.stateData[i] - fromNode.stateData[i];
                distance += diff * diff;
            }

            return Mathf.Sqrt(distance);
        }
    }

    public enum AgentStateKey
    {
        TargetInSight = 0
    }
}

