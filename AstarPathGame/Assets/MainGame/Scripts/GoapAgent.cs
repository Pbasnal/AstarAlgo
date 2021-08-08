using System.Collections.Generic;
using System.Linq;
using MainGame.Sensors;
using UnityEngine;

namespace MainGame
{
    [RequireComponent(typeof(AgentMemory))]
    public class GoapAgent : MonoBehaviour
    {
        public AnAgentAction[] agentActions;

        // Todo: Should be private and assigned by another system
        // that another system should be specific to selecting goals
        public AgentState goalState;
        public AgentState currentState;

        public AnAgentAction[] actionPath;
        private int _currentActionToExecute = 0;
        private AgentMemory agentMemory;
        private GoapData<AgentState> _goapData;
        private AgentGoalController _agentGoalController;

        private GoapPlanner _planner;

        private bool _stateUpdated = false;

        [HideInInspector] public InteractionType targetType;

        [Space]
        [Header("Debug Settings")]
        public bool drawGrid;

        private void Awake()
        {
            agentMemory = GetComponent<AgentMemory>();
            if (agentActions == null || agentActions.Length == 0)
            {
                return;
            }

            currentState = currentState.Clone();
            _goapData = new GoapData<AgentState>();
            _planner = new GoapPlanner(_goapData, agentActions);

            foreach (var action in agentActions)
            {
                action.Init(this);
            }
        }

        private void Start()
        {
            _agentGoalController = GetComponent<AgentGoalController>();
            //actionPath = _planner.FindActionsTo(currentState, goalState).Select(a => (AnAgentAction)a).ToList();
        }

        private void Update()
        {
            if (actionPath == null || _stateUpdated)
            {
                EvaluateGoalAndActionPath();
                _stateUpdated = false;
            }

            if (_currentActionToExecute >= actionPath.Length)
            {
                _stateUpdated = true;
                goalState = null;
                return;
            }

            var isActionComplete = actionPath[_currentActionToExecute].Execute();
            if (isActionComplete)
            {
                _currentActionToExecute += 1;
            }

        }

        public void SetState(AgentStateKey state)
        {
            currentState.Set(state);
            _stateUpdated = true;
        }

        public void UnSetState(AgentStateKey state)
        {
            if ((currentState.State.StateValue & state) == 0) return;

            currentState.UnSet(state);
            _stateUpdated = true;
        }

        public void UpdateMemory(IInteractable interactable)
        {
            if (!agentMemory.AddInteractable(interactable)) return;
            _stateUpdated = true;
        }
        public void RemoveFromMemory(IInteractable interactable)
        {
            agentMemory.Remove(interactable);
            _stateUpdated = true;
        }

        public void SetTargetType(InteractionType targetType)
        {
            this.targetType = targetType;
        }

        public IInteractable Find(InteractionType interactionType)
        {
            var allInteractablesOfType = agentMemory.GetAll(interactionType);
            if (allInteractablesOfType == null || allInteractablesOfType.Count == 0)
            {
                return null;
            }
            return allInteractablesOfType.FirstOrDefault();
        }

        private void EvaluateGoalAndActionPath()
        {
            var newGoalState = _agentGoalController.EvaluateGoal(this);
            if (newGoalState == null) return;

            if (goalState == null || newGoalState.State.StateValue != goalState.State.StateValue)
            {
                goalState = newGoalState.Clone();
                actionPath = _planner.FindActionsTo(currentState, goalState).Select(a => (AnAgentAction)a).ToArray();
                _currentActionToExecute = 0;
                return;
            }

            var nextActionToExecute = _currentActionToExecute;
            //* validate all actions 
            for (int i = _currentActionToExecute; i < actionPath.Length; i++)
            {
                if (actionPath[i].ValidateAction(this))
                {
                    nextActionToExecute = i;
                }
            }

            if (nextActionToExecute < actionPath.Length)
            {
                _currentActionToExecute = nextActionToExecute;
            }
        }
    }
}