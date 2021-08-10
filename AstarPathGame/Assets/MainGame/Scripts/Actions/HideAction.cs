using UnityEngine;

namespace MainGame.Actions
{
    [
        CreateAssetMenu(
            fileName = "HideAction",
            menuName = "GoapActions/HideAction",
            order = 52)
    ]
    public class HideAction : AnActionWithAgentState
    {
        public override bool Execute()
        {
            throw new System.NotImplementedException();
        }

        protected override AgentStateKey ApplyEffects(AgentStateKey currentState)
        {
            return currentState 
                | AgentStateKey.AgentOutOfSight
                | AgentStateKey.TargetOutOfSight
                | AgentStateKey.TargetOutOfRange;
        }

        protected override AgentStateKey ApplyPreConditions(AgentStateKey currentState)
        {
            return currentState | AgentStateKey.CanWalk;
        }
    }
}
