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
            preConditions.Set(AgentStateKey.CanWalk);

            effects = new State();
            effects.Set(AgentStateKey.AgentOutOfSight
                | AgentStateKey.TargetOutOfRange 
                | AgentStateKey.TargetOutOfSight);

            Weight = 1;
        }
    }
}
