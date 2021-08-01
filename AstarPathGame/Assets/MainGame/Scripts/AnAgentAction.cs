using UnityEngine;
using System.Collections.Generic;

namespace MainGame
{
    public interface IAgentAction
    {
        public float Weight { get; set; }
        //public AgentState OriginNode { get; set; }
        //public AgentState DestinationNode { get; set; }

        public bool CheckPreconditions(AgentState node);
        public AgentState GetGeneratedState(AgentState originNode);
    }

    public abstract class AnAgentAction : ScriptableObject, IAgentAction
    {
        public float Weight { get; set; }

        public State preConditions;
        public State effects;
        //public IDictionary<AgentStateKey, int> preConditions = new Dictionary<AgentStateKey, int>();
        //public IDictionary<AgentStateKey, int> effects = new Dictionary<AgentStateKey, int>();

        public virtual bool CheckPreconditions(AgentState node)
        {
            foreach (var condition in preConditions.GetStateElements())
            {
                //* Node doesn't contain the required state element
                var nodeDoesntContainStateElement = !node.ContainsKey(condition.stateName);
                if (nodeDoesntContainStateElement || node.Get(condition.stateName) != condition.value)
                {
                    return false;
                }
            }

            //* No need to perfrom this action if all the effects are already
            //* present in the state.
            foreach (var condition in effects.GetStateElements())
            {
                if (!node.ContainsKey(condition.stateName) || node.Get(condition.stateName) != condition.value)
                {
                    return true;
                }
            }

            return false;
        }
        public virtual AgentState GetGeneratedState(AgentState originNode)
        {
            var state = originNode.Clone();

            var stateGotUpdated = false;
            foreach (var effect in effects.GetStateElements())
            {
                stateGotUpdated |= state.Set(effect.stateName, effect.value);
            }

            if (stateGotUpdated)
            {
                state.NodeCost = originNode.NodeCost + 1;
                return state;
            }
            return null;
        }

        public abstract void Init();
    }
}

