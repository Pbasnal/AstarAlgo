using UnityEngine;
using System.Collections.Generic;
using System;

namespace MainGame
{
    public interface IAgentAction
    {
        float Weight { get; set; }
        bool ValidateAction(AgentState sate);
        AgentState GetGeneratedState(AgentState originNode);
    }

    public abstract class AnAgentAction : ScriptableObject, IAgentAction
    {
        public float Weight { get; set; }

        public AgentState preConditions;
        public AgentState effects;
        public virtual bool ValidateAction(AgentState currentState)
        {
            var currentStateValue = currentState.StateValue;
            var stateWithPreConditions = ApplyPreConditions(currentStateValue);
            if (stateWithPreConditions != currentStateValue) return false;

            //* No need to perfrom this action if all the effects are already
            //* present in the state.
            var stateWithEffects = ApplyEffects(currentStateValue);
            if (stateWithEffects == currentStateValue) return false;

            return true;
        }

        public virtual AgentState GetGeneratedState(AgentState originNode)
        {
            var agentState = originNode.Clone();
            agentState.StateValue = ApplyEffects(agentState.StateValue);
            return agentState;
        }

        protected abstract AgentStateKey ApplyPreConditions(AgentStateKey currentState);

        protected abstract AgentStateKey ApplyEffects(AgentStateKey currentState);

        public virtual void Init(GoapAgent agent)
        {
            Weight = 1;
        }

        public abstract bool Execute();
    }
}

