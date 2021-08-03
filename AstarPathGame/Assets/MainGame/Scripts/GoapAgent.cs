using System.Collections.Generic;
using System.Linq;
using MainGame;
using UnityEngine;

public class GoapAgent : MonoBehaviour
{
    public AnAgentAction[] agentActions;

    // Todo: Should be private and assigned by another system
    // that another system should be specific to selecting goals
    public AgentState goalState;
    public AgentState currentState;

    public List<AnAgentAction> actionPath;

    private GoapData<AgentState> _goapData;

    private GoapPlanner _planner;

    private void Awake()
    {
        if (agentActions == null || agentActions.Length == 0)
        {
            return;
        }

        _goapData = new GoapData<AgentState>();
        _planner = new GoapPlanner(_goapData, agentActions);

        // Todo: Remove this after testing. State should get set by sensors
        //currentState.Set(AgentStateKey.CanWalk);
    }

    private void Start()
    {
        actionPath = _planner.FindActionsTo(currentState, goalState).Select(a => (AnAgentAction)a).ToList();
    }
}
