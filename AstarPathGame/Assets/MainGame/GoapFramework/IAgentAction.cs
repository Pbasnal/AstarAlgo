namespace GoapFramework
{
    public interface IAgentAction
    {
        float Weight { get; set; }
        bool ValidateAction(IAgentState currentSate);
        IAgentState GetGeneratedState(IAgentState currentState);
        void OnStart(IGoapAgent goapAgent);
        void OnUpdate(IGoapAgent goapAgent);
        void OnEnter(IGoapAgent goapAgent);
        void OnExit(IGoapAgent goapAgent);
        bool Execute();
        bool CheckEffects(IAgentState state);
        bool CheckPreConditions(IAgentState state);
    }
}

