using UnityEngine;
using System.Collections.Generic;
using System;

namespace MainGame
{
    public interface IAgentAction<TAgentState> 
        where TAgentState : IAgentState<TAgentState>
    {
        float Weight { get; set; }
        bool ValidateAction(TAgentState currentSate);
        TAgentState GetGeneratedState(TAgentState currentState);
        void OnStart(AGoapAgent<TAgentState> goapAgent);
        void OnUpdate(AGoapAgent<TAgentState> goapAgent);
        void OnEnter(AGoapAgent<TAgentState> goapAgent);
        void OnExit(AGoapAgent<TAgentState> goapAgent);
        bool Execute();
    }

    public abstract class AnAgentAction 
        : ScriptableObject, IAgentAction<AgentState>
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

        public virtual void OnStart(GoapAgent agent)
        {
            Weight = 1;
        }

        public abstract bool Execute();

        public void OnStart(AGoapAgent<AgentState> goapAgent)
        {
            
        }

        public void OnUpdate(AGoapAgent<AgentState> goapAgent)
        {
            
        }

        public void OnEnter(AGoapAgent<AgentState> goapAgent)
        {
            
        }

        public void OnExit(AGoapAgent<AgentState> goapAgent)
        {
            
        }
    }
}

