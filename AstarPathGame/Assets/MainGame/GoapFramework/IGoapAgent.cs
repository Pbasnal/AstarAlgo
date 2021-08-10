namespace GoapFramework
{
    public interface IGoapAgent//<TAgentState>
        //where TAgentState : IAgentState
    {
        void SetState(IAgentState state);
        void UnSetState(IAgentState state);
        void OnStateChange(IAgentState state);
        void UpdateMemory(IInteractable interactable);
        void RemoveFromMemory(IInteractable interactable);
        void SetTargetType(InteractionType targetType);
        IAgentState GetCurrentState();
        IInteractable GetTarget();
        IInteractable Find(InteractionType interactionType);
    }
}