using UnityEngine;

namespace MainGame.Actions
{
    [CreateAssetMenu(fileName = "AttackEnemyAction", menuName = "GoapActions/AttackEnemyAction", order = 52)]
    public class AttackEnemyAction : AnActionWithAgentState
    {
        public override bool Execute()
        {
            throw new System.NotImplementedException();
        }
        
        protected override AgentStateKey ApplyEffects(AgentStateKey currentState)
        {
            return currentState | AgentStateKey.EnemyIsDead;
        }

        protected override AgentStateKey ApplyPreConditions(AgentStateKey currentState)
        {
            return currentState | AgentStateKey.CanWalk
                | AgentStateKey.TargetInSight
                | AgentStateKey.TargetInRange;
        }
    }
}

