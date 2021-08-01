using UnityEngine;

namespace MainGame.Actions
{
    [CreateAssetMenu(fileName = "ReachTargetAction", menuName = "GoapActions/ReachTargetAction", order = 52)]
    public class ReachTargetAction : AnAgentAction
    {
        public override void Init()
        {
            preConditions = new State();
            preConditions.Set(AgentStateKey.CanWalk, 1);

            effects = new State();
            effects.Set(AgentStateKey.AgentOutOfSight, 0);
            effects.Set(AgentStateKey.TargetInSight, 1);
            effects.Set(AgentStateKey.TargetInRange, 1);

            Weight = 1;
        }
    }
}

