using System.Collections.Generic;

namespace GoapFramework
{
    public interface IGoapAgent
    {
        void SetState(IAgentState state);
        void UnSetState(IAgentState state);
        void OnStateChange(IAgentState state);
        void OnActionPathUpdate(List<IAgentAction> actionPath);
        IAgentState GetCurrentState();
        IAgentAction[] GetAllActions();

        IAgentGoalProvider GetGoalProvider();
        // IInteractable GetTarget();
        // void UpdateMemory(IInteractable interactable);
        // void RemoveFromMemory(IInteractable interactable);
        // void SetTargetType(InteractionType targetType);
        // IInteractable Find(InteractionType interactionType);
    }
}