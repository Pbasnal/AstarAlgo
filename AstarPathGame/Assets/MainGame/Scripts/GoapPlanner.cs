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
        public IAgentAction ActionForThisNode { get; set; }
        public float NodeCost { get; set; }
        public float HeuristicCost { get; set; }
        public float Priority { get; set; }
        public int PreviousNode { get; set; }

        //public int[] stateData;
        public IDictionary<AgentStateKey, int> stateData;

        private AgentState()
        {
            Id = -1;
            stateData = new Dictionary<AgentStateKey, int>();// int[StateSize];
        }

        public static AgentState New()
        {
            return new AgentState();
        }

        public void Set(AgentStateKey stateKey, int value)
        {
            stateData[stateKey] = value;
        }

        public AgentState Clone()
        {
            var newState = new AgentState();

            foreach (var pair in stateData)
            {
                newState.stateData[pair.Key] = stateData[pair.Key];
            }

            return newState;
        }
    }

    public interface IAgentAction
    {
        public float Weight { get; set; }
        //public AgentState OriginNode { get; set; }
        //public AgentState DestinationNode { get; set; }

        public bool CheckPreconditions(AgentState node);
        public AgentState GetGeneratedState(AgentState originNode);
    }

    public abstract class AnAgentAction : IAgentAction
    {
        public float Weight { get; set; }

        public IDictionary<AgentStateKey, int> preConditions { get; }
        public IDictionary<AgentStateKey, int> effects { get; }

        public AnAgentAction()
        {
            preConditions = new Dictionary<AgentStateKey, int>();
            effects = new Dictionary<AgentStateKey, int>();
        }

        public virtual bool CheckPreconditions(AgentState node)
        {
            foreach (var condition in preConditions)
            {
                if (!node.stateData.ContainsKey(condition.Key)
                    || node.stateData[condition.Key] != condition.Value)
                {
                    return false;
                }
            }

            return true;
        }
        public virtual AgentState GetGeneratedState(AgentState originNode)
        {
            var state = originNode.Clone();

            foreach (var effect in effects)
            {
                if (!state.stateData.ContainsKey(effect.Key))
                {
                    state.stateData.Add(effect.Key, effect.Value);
                }
                else
                {
                    state.stateData[effect.Key] = effect.Value;
                }
            }

            return state;
        }
    }

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
            var fromNode = _goapData.currentState;
            _goapData.ResetForNewOriginNode(fromNode);

            var heuristicCost = HeuristicCost(fromNode, destinationNode);
            _goapData.AddAFrontierNode(null, fromNode, null, 0, heuristicCost);
            while (_goapData.TryGetNodeWithMinimumCost(out fromNode))
            {
                heuristicCost = HeuristicCost(fromNode, destinationNode);
                if (heuristicCost == 0)
                {
                    destinationNode = fromNode;
                    break;
                }
                var actions = _goapData.GetEdgesOriginatingFromNode(fromNode);

                foreach (var action in actions)
                {
                    var newState = action.GetGeneratedState(fromNode);
                    TryAddFrontierNode(fromNode, newState, action);
                }
                _goapData.SetNodeVisited(fromNode);
            }

            return _goapData.GetPathTo(destinationNode.Id);
        }

        private float TryAddFrontierNode(AgentState originNode,
            AgentState newNode, IAgentAction edge)
        {
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

            foreach (var tostate in toNode.stateData)
            {
                float diff;
                if (!fromNode.stateData.ContainsKey(tostate.Key))
                {
                    diff = 1;
                }
                else
                {
                    diff = toNode.stateData[tostate.Key] - fromNode.stateData[tostate.Key];
                }
                distance += diff * diff;
            }

            return Mathf.Sqrt(distance);
        }
    }

    public enum AgentStateKey
    {
        TargetInSight = 0,
        OutOfSight,
        TargetInRange,
        CanWalk,
        EnemyIsDead
    }
}

