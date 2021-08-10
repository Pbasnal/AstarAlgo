using UnityEngine;
using GoapFramework;
namespace MainGame
{
    // implementation | not framework
    public class AgentGoalProvider : MonoBehaviour, IAgentGoalProvider
    {
        public AgentState Patrol;
        public AgentState EatFood;

        public IAgentState EvaluateGoal(IGoapAgent agent)
        {
            var currentState = agent.GetCurrentState();
            if (!currentState.Contains(EatFood))
            {
                agent.SetTargetType(InteractionType.Consumable);
                return EatFood;

            }
            return Patrol;
        }
    }
}