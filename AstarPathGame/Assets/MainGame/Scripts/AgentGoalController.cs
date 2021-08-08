using UnityEngine;

namespace MainGame
{
    public class AgentGoalController : MonoBehaviour
    {
        public AgentState Patrol;
        public AgentState EatFood;

        public AgentState EvaluateGoal(GoapAgent agent)
        {
            var currentState = agent.currentState;
            if ((currentState.State.StateValue & AgentStateKey.IsNotHungry) == 0)
            {
                agent.SetTargetType(InteractionType.Consumable);
                return EatFood;

            }
            return Patrol;
        }
    }
}