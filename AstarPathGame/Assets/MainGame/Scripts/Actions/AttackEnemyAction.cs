using UnityEngine;

namespace MainGame.Actions
{
    [CreateAssetMenu(fileName = "AttackEnemyAction", menuName = "GoapActions/AttackEnemyAction", order = 52)]
    public class AttackEnemyAction : AnAgentAction
    {
        public override void Init()
        {
            preConditions = new State();
            preConditions.Set(AgentStateKey.CanWalk, 1);
            preConditions.Set(AgentStateKey.TargetInSight, 1);
            preConditions.Set(AgentStateKey.TargetInRange, 1);
            preConditions.Set(AgentStateKey.AgentOutOfSight, 0);

            effects = new State();
            effects.Set(AgentStateKey.EnemyIsDead, 1);
            
            Weight = 1;
        }
    }
}

