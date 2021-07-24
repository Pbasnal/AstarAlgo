namespace MainGame.Actions
{
    public class ReachTargetAction : AnAgentAction
    {
        public ReachTargetAction()
        {
            preConditions.Add(AgentStateKey.CanWalk, 1);

            effects.Add(AgentStateKey.OutOfSight, 0);
            effects.Add(AgentStateKey.TargetInSight, 1);
            effects.Add(AgentStateKey.TargetInRange, 1);

            Weight = 1;
        }
    }
}

