using UnityEngine;
using System.Collections.Generic;
using System;

namespace MainGame
{
    public interface IAgentAction
    {
        float Weight { get; set; }
        bool CheckPreconditions(AgentState node);
         AgentState GetGeneratedState(AgentState originNode);
    }

    public abstract class AnAgentAction : ScriptableObject, IAgentAction
    {
        public float Weight { get; set; }

        public State preConditions;
        public State effects;
        public virtual bool CheckPreconditions(AgentState currentState)
        {
            var maskedState = currentState.State.StateValue & preConditions.StateValue;
            if (maskedState != preConditions.StateValue) return false;

            //* No need to perfrom this action if all the effects are already
            //* present in the state.
            maskedState = currentState.State.StateValue & effects.StateValue;
            if (maskedState == effects.StateValue) return false;

            return true;
        }

        public virtual AgentState GetGeneratedState(AgentState originNode)
        {
            var agentState = originNode.Clone();
            agentState.Set(effects.StateValue);
            return agentState;
        }

        public abstract void Init(GoapAgent goapAgent);

        public abstract bool Execute();

        public abstract bool ValidateAction(GoapAgent agent);
    }
}

