using UnityEngine;
using System.Collections.Generic;
using System;
using BasnalGames.GoapFramework;

namespace MainGame
{

    public abstract class AnAgentAction<TAgentState>
        : ScriptableObject, IAgentAction
        where TAgentState : IAgentState
    {
        public float Weight { get; set; }

        public virtual bool ValidateAction(IAgentState currentState)
        {
            //* No need to perfrom this action if all the effects are already
            //* present in the state.
            return CheckPreConditions(currentState)
                && !CheckEffects(currentState);
        }

        public abstract IAgentState GetGeneratedState(IAgentState originNode);

        public virtual void OnStart(IGoapAgent goapAgent)
        {
            Weight = 1;
        }
        
        public void OnUpdate(IGoapAgent goapAgent)
        { }

        public void OnEnter(IGoapAgent goapAgent)
        { }

        public void OnExit(IGoapAgent goapAgent)
        { }

        public abstract bool CheckEffects(IAgentState state);

        public abstract bool CheckPreConditions(IAgentState state);

        public abstract bool Execute();
    }


    public abstract class AnActionWithAgentState : AnAgentAction<AgentState>
    {
        public override IAgentState GetGeneratedState(IAgentState state)
        {
            var newState = state.Clone() as AgentState;
            newState.StateValue = ApplyEffects(newState.StateValue);

            return newState;
        }

        public override bool CheckEffects(IAgentState state)
        {
            var currentState = state as AgentState;

            // current state should change on applying effects
            return currentState.StateValue == ApplyEffects(currentState.StateValue);
        }

        public override bool CheckPreConditions(IAgentState state)
        {
            var currentState = state as AgentState;

            // current state should stay same on applying preconditions
            return currentState.StateValue == ApplyPreConditions(currentState.StateValue);
        }

        protected abstract AgentStateKey ApplyEffects(AgentStateKey currentState);

        protected abstract AgentStateKey ApplyPreConditions(AgentStateKey currentState);
    }
}

