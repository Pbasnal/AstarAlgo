namespace GoapFramework
{
    public interface IAgentGoalProvider
    {
        IAgentState EvaluateGoal(IGoapAgent agent);
    }
}