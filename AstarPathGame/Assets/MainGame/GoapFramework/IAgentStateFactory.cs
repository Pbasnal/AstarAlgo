using System;

namespace GoapFramework
{
    public interface IAgentStateFactory<TAgentState>
        where TAgentState : IAgentState
    {
        TAgentState New(Action<IAgentState> stateChangeCallback);
    }
}
