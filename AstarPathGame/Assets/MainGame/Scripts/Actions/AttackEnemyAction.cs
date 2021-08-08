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

        public override void Init(GoapAgent goapAgent)
        {
            preConditions = new State();
            preConditions.Set(AgentStateKey.CanWalk
                | AgentStateKey.TargetInSight
                | AgentStateKey.TargetInRange);
                
            effects = new State();
            effects.Set(AgentStateKey.EnemyIsDead);
            
            Weight = 1;
        }

        public override bool ValidateAction(GoapAgent agent)
        {
            return CheckPreconditions(agent.currentState);
        }
    }
}

