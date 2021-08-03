using UnityEngine;

namespace MainGame.Actions
{
    [CreateAssetMenu(fileName = "ReachTargetAction", menuName = "GoapActions/ReachTargetAction", order = 52)]
    public class ReachTargetAction : AnAgentAction
    {
        public override void Init()
        {
            preConditions = new State();
            preConditions.Set(AgentStateKey.CanWalk);

            effects = new State();
            effects.Set(AgentStateKey.AgentOutOfSight
                | AgentStateKey.TargetInSight
                | AgentStateKey.TargetInRange);

            Weight = 1;
        }
    }
}

