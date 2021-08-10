using System;

namespace GoapFramework
{
    public interface IAgentState
    {
        IAgentState Clone(Action<IAgentState> onStateChangeCallback = null);
        int AgentStateValue();
        float DistanceFrom(IAgentState state);
        IAgentState IntersectState(IAgentState state);
        void OnStateChange(IAgentState state);
        void AddState(IAgentState stateInfoToUpdateWith);
        void RemoveState(IAgentState stateInfoToUpdateWith);
        bool Contains(IAgentState state);
    }
}
