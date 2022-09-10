using System.Collections.Generic;
using System.Linq;
using GoapAi;
using InsightsLogger;

namespace ConsoleInterface
{
    public class Agent : IUseGoapPlan
    {
        private readonly IDictionary<int, GoapGoal> agentGoals;
        private readonly List<GoapAction> agentActions;

        private readonly ISimpleLogger logger;

        private readonly State currentState;

        public Agent(ISimpleLogger logger)
        {
            this.logger = logger;


            currentState = new State("Starting State");
            agentGoals = new SortedDictionary<int, GoapGoal>();
            agentActions = new List<GoapAction>();
        }

        public State GetCurrentState() => currentState;
        public void AddAction(GoapAction action) => agentActions.Add(action);
        public void RemoveAction(GoapAction action) => agentActions.Add(action);

        public void AddGoal(int priority, GoapGoal goal)
        {
            if (agentGoals.ContainsKey(priority))
            {
                agentGoals[priority] = goal;
            }
            else
            {
                agentGoals.Add(priority, goal);
            }
        }

        public void RemoveGoal(GoapGoal goal)
        {
            int keyToRemove = -1;
            foreach (var agentGoal in agentGoals)
            {
                if (agentGoal.Value.Equals(goal))
                {
                    keyToRemove = agentGoal.Key;
                }
            }

            if (keyToRemove > -1)
            {
                agentGoals.Remove(keyToRemove);
            }
        }
        public IList<GoapAction> GetAgentActions() => agentActions;

        public State GetGoalStateToPlanFor()
        {
            var goal = agentGoals.Values
                .Where(g => g.ShouldPursueGoapGoal(currentState))
                .FirstOrDefault();

            return goal == null ? new State("Empty Goal") : goal.GoalState;
        }

        public void SetFact(string factName, bool factValue) => currentState.SetFact(factName, factValue);
    }
}