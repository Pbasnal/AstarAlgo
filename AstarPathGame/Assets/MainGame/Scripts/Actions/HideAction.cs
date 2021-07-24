namespace MainGame.Actions
{
    public class HideAction : AnAgentAction
    {
        public HideAction()
        {
            preConditions.Add(AgentStateKey.CanWalk, 1);

            effects.Add(AgentStateKey.OutOfSight, 1);
            effects.Add(AgentStateKey.TargetInRange, 0);

            Weight = 1;
        }
    }
}

