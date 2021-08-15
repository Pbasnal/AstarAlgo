using UnityEngine;
using BasnalGames.GoapFramework;

namespace MainGame
{
    // implementation | not framework
    [RequireComponent(typeof(GoapAgent))]
    public class AgentGoalProvider : MonoBehaviour, IAgentGoalProvider
    {
        private GoapAgent agent;
        public AgentState Patrol;
        public AgentState EatFood;

        private void Start()
        {
            agent = GetComponent<GoapAgent>();
        }

        public IAgentState EvaluateGoal()
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