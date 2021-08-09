using UnityEngine;

namespace MainGame
{
    public interface IAgentGoalProvider<TAgentState>
        where TAgentState: IAgentState<TAgentState>
    {
        TAgentState EvaluateGoal(AGoapAgent<TAgentState> agent);
    }

    public class AgentGoalProvider : MonoBehaviour, IAgentGoalProvider<AgentState>
    {
        public AgentState Patrol;
        public AgentState EatFood;

        public AgentState EvaluateGoal(AGoapAgent<AgentState> agent)
        {
            var currentState = agent.currentState;
            if (!currentState.Contains(EatFood))
            {
                agent.SetTargetType(InteractionType.Consumable);
                return EatFood;

            }
            return Patrol;
        }
    }
}