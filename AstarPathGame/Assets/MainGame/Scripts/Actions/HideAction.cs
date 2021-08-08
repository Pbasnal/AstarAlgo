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
        public override bool Execute()
        {
            throw new System.NotImplementedException();
        }

        public override void Init(GoapAgent goapAgent)
        {
            preConditions = new State();
            preConditions.Set(AgentStateKey.CanWalk);

            effects = new State();
            effects.Set(AgentStateKey.AgentOutOfSight
                | AgentStateKey.TargetOutOfRange 
                | AgentStateKey.TargetOutOfSight);

            Weight = 1;
        }

        public override bool ValidateAction(GoapAgent agent)
        {
            return CheckPreconditions(agent.currentState);
        }
    }
}
