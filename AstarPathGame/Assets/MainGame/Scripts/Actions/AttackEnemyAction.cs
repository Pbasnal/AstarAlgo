namespace MainGame.Actions
{
    public class AttackEnemyAction : AnAgentAction
    {
        public AttackEnemyAction()
        {
            preConditions.Add(AgentStateKey.CanWalk, 1);
            preConditions.Add(AgentStateKey.TargetInSight, 1);
            preConditions.Add(AgentStateKey.TargetInRange, 1);
            preConditions.Add(AgentStateKey.OutOfSight, 0);

            effects.Add(AgentStateKey.EnemyIsDead, 1);            

            Weight = 1;
        }
    }
}

