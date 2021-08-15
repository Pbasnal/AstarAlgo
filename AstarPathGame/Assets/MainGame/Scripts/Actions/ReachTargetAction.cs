using UnityEngine;
using BasnalGames.GoapFramework;

namespace MainGame.Actions
{
    [CreateAssetMenu(fileName = "ReachTargetAction", menuName = "GoapActions/ReachTargetAction", order = 52)]
    public class ReachTargetAction : AnActionWithAgentState
    {
        private GoapAgent agent;
        private PatrolBehaviour patrolBehaviour;
        private MoveToTarget moveToTarget;

        public override bool Execute()
        {
            var target = agent.GetTarget();
            if (target == null)
            {
                patrolBehaviour.Behave();
                return false;
            }

            moveToTarget.Move(target.Position);
            if (Vector3.Distance(target.Position, agent.transform.position) <= 3f)
            {
                var currentState = (AgentState)agent.GetCurrentState();
                currentState.StateValue = ApplyEffects(currentState.StateValue);
                return true;
            }

            return false;
        }

        public override void OnStart(IGoapAgent goapAgent)
        {
            base.OnStart(goapAgent);

            agent = goapAgent as GoapAgent;
            patrolBehaviour = agent.GetComponent<PatrolBehaviour>();
            moveToTarget = agent.GetComponent<MoveToTarget>();
        }

        public override bool ValidateAction(IAgentState currentState)
        {
            if (!base.ValidateAction(currentState)) return false;

            return agent.GetTarget() != null;
        }

        protected override AgentStateKey ApplyEffects(AgentStateKey currentState)
        {
            return currentState | AgentStateKey.TargetInSight
                | AgentStateKey.TargetInRange;
        }

        protected override AgentStateKey ApplyPreConditions(AgentStateKey currentState)
        {
            return currentState | AgentStateKey.CanWalk;
        }
    }
}

