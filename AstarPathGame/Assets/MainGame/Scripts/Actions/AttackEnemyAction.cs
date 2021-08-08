using UnityEngine;

namespace MainGame.Actions
{
    [CreateAssetMenu(fileName = "AttackEnemyAction", menuName = "GoapActions/AttackEnemyAction", order = 52)]
    public class AttackEnemyAction : AnAgentAction
    {
        public override bool Execute()
        {
            throw new System.NotImplementedException();
        }

        protected override AgentStateKey ApplyEffects(AgentStateKey currentState)
        {
            var state = currentState;
            state |= AgentStateKey.EnemyIsDead;

            return state;
        }

        protected override AgentStateKey ApplyPreConditions(AgentStateKey currentState)
        {
            var state = currentState;

            state |= AgentStateKey.CanWalk
                | AgentStateKey.TargetInSight
                | AgentStateKey.TargetInRange;
            
            return state;
        }
    }
}

