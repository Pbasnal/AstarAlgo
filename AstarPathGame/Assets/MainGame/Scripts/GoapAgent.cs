using System.Collections.Generic;
using System.Linq;
using MainGame;
using UnityEngine;

public class GoapAgent : MonoBehaviour
{
    public  AnAgentAction[] agentActions;

    // Todo: Should be private and assigned by another system
    // that another system should be specific to selecting goals
    public AgentState goalState;

    public List<AnAgentAction> actionPath;

    private GoapData _goapData;

    private GoapPlanner _planner;

    private void Awake()
    {
        if (agentActions == null || agentActions.Length == 0)
        {
            return;
        }
        
        _goapData = new GoapData(agentActions);
        _planner = new GoapPlanner(_goapData);

        // Todo: Remove this after testing. State should get set by sensors
        _goapData.SetState(AgentStateKey.CanWalk, 1);
    }

    private void Start() {
        actionPath = _planner.FindActionsTo(goalState).Select(a => (AnAgentAction)a).ToList();
    }
}
