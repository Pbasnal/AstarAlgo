using UnityEngine;
using System.Collections.Generic;

namespace MainGame
{
    public interface IAgentAction
    {
        public float Weight { get; set; }
        public bool CheckPreconditions(AgentState node);
        public AgentState GetGeneratedState(AgentState originNode);
    }

    public abstract class AnAgentAction : ScriptableObject, IAgentAction
    {
        public float Weight { get; set; }

        public State preConditions;
        public State effects;
        public virtual bool CheckPreconditions(AgentState node)
        {
            var maskedState = node.State.StateValue & preConditions.StateValue;
            if (maskedState != preConditions.StateValue) return false;

            //* No need to perfrom this action if all the effects are already
            //* present in the state.
            maskedState = node.State.StateValue & effects.StateValue;
            if (maskedState == effects.StateValue) return false;

            return true;
        }
        public virtual AgentState GetGeneratedState(AgentState originNode)
        {
            var state = originNode.Clone();
            state.Set(effects.StateValue);
            return state;
        }

        public abstract void Init();
    }
}

