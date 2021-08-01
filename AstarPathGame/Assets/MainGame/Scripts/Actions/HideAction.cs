using UnityEngine;

namespace MainGame.Actions
{
    [
        CreateAssetMenu(
            fileName = "HideAction",
            menuName = "GoapActions/HideAction",
            order = 52)
    ]
    public class HideAction : AnAgentAction
    {
        public override void Init()
        {
            preConditions = new State();
            preConditions.Set(AgentStateKey.CanWalk, 1);

            effects = new State();
            effects.Set(AgentStateKey.AgentOutOfSight, 1);
            effects.Set(AgentStateKey.TargetInRange, 0);

            Weight = 1;
        }
    }
}
