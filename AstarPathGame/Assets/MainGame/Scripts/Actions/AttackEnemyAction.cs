using UnityEngine;

namespace MainGame.Actions
{
    [CreateAssetMenu(fileName = "AttackEnemyAction", menuName = "GoapActions/AttackEnemyAction", order = 52)]
    public class AttackEnemyAction : AnAgentAction
    {
        public override void Init()
        {
            preConditions = new State();
            preConditions.Set(AgentStateKey.CanWalk
                | AgentStateKey.TargetInSight
                | AgentStateKey.TargetInRange);
                
            effects = new State();
            effects.Set(AgentStateKey.EnemyIsDead);
            
            Weight = 1;
        }
    }
}

