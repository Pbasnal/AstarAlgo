namespace GoapAi
{
    public class GoapGoal
    {
        public string GoalName { get; private set; }
        public State PreConditions { get; private set; }
        public State GoalState { get; private set; }

        public GoapGoal(string goalName)
        {
            this.GoalName = goalName;
            PreConditions = new State($"{goalName}-conditions");
            GoalState = new State($"{goalName}-state");
        }

        public void SetAPreconditionFact(string factName, bool factValue) => PreConditions.SetFact(factName, factValue);
        public void SetAGoalFact(string factName, bool factValue) => GoalState.SetFact(factName, factValue);
        public bool ShouldPursueGoapGoal(State currentState) => PreConditions.IsSameAs(currentState);
    }
}