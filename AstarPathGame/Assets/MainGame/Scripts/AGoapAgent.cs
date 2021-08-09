using System.Collections.Generic;
using System.Linq;
using MainGame.Sensors;
using UnityEngine;

namespace MainGame
{
    [RequireComponent(typeof(AgentMemory))]
    public abstract class AGoapAgent<TAgentState> : MonoBehaviour
        where TAgentState : IAgentState<TAgentState>
    {
        public IAgentAction<TAgentState>[] agentActions;

        protected TAgentState goalState;
        public TAgentState currentState;
        public List<IAgentAction<TAgentState>> actionPath;

        protected GoapData<TAgentState> _goapData;

        protected int _currentActionToExecute = 0;
        protected AgentMemory agentMemory;
        protected IAgentGoalProvider<TAgentState> _agentGoalProvider;
        protected GoapPlanner<TAgentState> _planner;

        protected bool _stateUpdated = false;

        protected InteractionType targetType;

        [Space]
        [Header("Debug Settings")]
        public bool drawGrid;

        private void Awake()
        {
            if (agentActions == null || agentActions.Length == 0)
            {
                throw new UnityException($"Assign actions to the agent {name}");
            }

            currentState = currentState.Clone(OnStateChange);
            _goapData = new GoapData<TAgentState>();
            _planner = new GoapPlanner<TAgentState>(_goapData, agentActions);
        }

        private void Start()
        {
            foreach (var action in agentActions)
            {
                action.OnStart(this);
            }
            agentMemory = GetComponent<AgentMemory>();

            _agentGoalProvider = GetComponent<IAgentGoalProvider<TAgentState>>();
        }

        private void Update()
        {
            if (actionPath == null || _stateUpdated)
            {
                EvaluateGoalAndActionPath();
                _stateUpdated = false;
            }

            if (_currentActionToExecute >= actionPath.Count)
            {
                _stateUpdated = true;
                actionPath.Clear();
                return;
            }

            var isActionComplete = actionPath[_currentActionToExecute].Execute();
            if (isActionComplete)
            {
                _currentActionToExecute += 1;
            }
        }

        public void OnStateChange(TAgentState state)
        {
            if (currentState.Equals(state)) return;
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

        public IInteractable GetTarget()
        {
            return Find(targetType);
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
            var newGoalState = _agentGoalProvider.EvaluateGoal(this);
            if (newGoalState == null) return;

            if (goalState == null || !newGoalState.Equals(goalState))
            {
                goalState = newGoalState.Clone();
                actionPath = _planner.FindActionsTo(currentState, goalState);
                _currentActionToExecute = 0;
                return;
            }

            var nextActionToExecute = _currentActionToExecute;
            //* validate all actions 
            for (int i = _currentActionToExecute; i < actionPath.Count; i++)
            {
                if (actionPath[i].ValidateAction(this.currentState))
                {
                    nextActionToExecute = i;
                }
            }

            if (nextActionToExecute < actionPath.Count)
            {
                _currentActionToExecute = nextActionToExecute;
            }
        }
    }
}