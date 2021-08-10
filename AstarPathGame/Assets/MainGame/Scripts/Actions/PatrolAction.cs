using UnityEngine;
using MainGame;
using GoapFramework;

namespace MainGame.Actions
{
    [CreateAssetMenu(fileName = "PatrolAction", menuName = "GoapActions/PatrolAction", order = 52)]
    public class PatrolAction : AnActionWithAgentState
    {
        private PatrolBehaviour _patrolBehaviour;

        public override bool Execute()
        {
            _patrolBehaviour.Behave();

            return false;
        }

        public override void OnStart(IGoapAgent goapAgent)
        {
            base.OnStart(goapAgent);
            
            var agent = goapAgent as GoapAgent;
            _patrolBehaviour = agent.GetComponent<PatrolBehaviour>();
            if(_patrolBehaviour == null)
            {
                var up = new UnityException("Add patrol behaviour to use in patrol action");;
                throw up;
            }
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
    }
}

