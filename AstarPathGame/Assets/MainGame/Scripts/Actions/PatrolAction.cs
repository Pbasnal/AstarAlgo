using UnityEngine;
using MainGame;

namespace MainGame.Actions
{
    [CreateAssetMenu(fileName = "PatrolAction", menuName = "GoapActions/PatrolAction", order = 52)]
    public class PatrolAction : AnAgentAction
    {
        private PatrolBehaviour _patrolBehaviour;

        public override bool Execute()
        {
            _patrolBehaviour.Behave();

            return false;
        }

        protected override AgentStateKey ApplyEffects(AgentStateKey currentState)
        {
            return currentState 
                | AgentStateKey.AreaExplored
                | AgentStateKey.TargetInSight
                | AgentStateKey.TargetInRange;
        }

        protected override AgentStateKey ApplyPreConditions(AgentStateKey currentState)
        {
            return currentState | AgentStateKey.CanWalk;
        }

        public override void Init(GoapAgent goapAgent)
        {
            base.Init(goapAgent);
            
            _patrolBehaviour = goapAgent.GetComponent<PatrolBehaviour>();
            if(_patrolBehaviour == null)
            {
                var up = new UnityException("Add patrol behaviour to use in patrol action");;
                throw up;
            }
        }
    }
}

