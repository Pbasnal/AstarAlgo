using UnityEngine;

namespace MainGame.Actions
{
    [CreateAssetMenu(fileName = "ReachTargetAction", menuName = "GoapActions/ReachTargetAction", order = 52)]
    public class ReachTargetAction : AnAgentAction
    {
        private GoapAgent agent;
        private PatrolBehaviour patrolBehaviour;
        private MoveToTarget moveToTarget;

        public override bool Execute()
        {
            var target = agent.Find(agent.targetType);

            if (target == null)
            {
                patrolBehaviour.Behave();
                return false;
            }

            moveToTarget.Move(target.Position);
            if (Vector3.Distance(target.Position, agent.transform.position) <= 3f)
            {
                agent.SetState(AgentStateKey.TargetInSight | AgentStateKey.TargetInRange);
                return true;
            }

            return false;
        }

        public override void Init(GoapAgent goapAgent)
        {
            preConditions = new State();
            preConditions.Set(AgentStateKey.CanWalk);

            effects = new State();
            effects.Set(AgentStateKey.TargetInSight
                | AgentStateKey.TargetInRange);

            Weight = 1;

            agent = goapAgent;
            patrolBehaviour = agent.GetComponent<PatrolBehaviour>();
            moveToTarget = agent.GetComponent<MoveToTarget>();
        }

        public override bool ValidateAction(GoapAgent agent)
        {
            if (!CheckPreconditions(agent.currentState)) return false;

            return agent.Find(agent.targetType) != null;
        }
    }
}

