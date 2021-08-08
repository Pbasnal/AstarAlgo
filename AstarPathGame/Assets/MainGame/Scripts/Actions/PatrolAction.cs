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

        public override void Init(GoapAgent goapAgent)
        {
            preConditions = new State();
            preConditions.Set(AgentStateKey.CanWalk);

            effects = new State();
            effects.Set(AgentStateKey.AreaExplored);
            effects.Set(AgentStateKey.TargetInSight);

            Weight = 1;

            _patrolBehaviour = goapAgent.GetComponent<PatrolBehaviour>();
            if(_patrolBehaviour == null)
            {
                var up = new UnityException("Add patrol behaviour to use in patrol action");;
                throw up;
            }
        }

        public override bool ValidateAction(GoapAgent agent)
        {
            return CheckPreconditions(agent.currentState);
        }
    }
}

