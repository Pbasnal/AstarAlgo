using System.Collections.Generic;
using System.Linq;
using MainGame.Sensors;
using UnityEngine;
using GoapFramework;
namespace MainGame
{
    public abstract class AGoapAgent<TAgentAction> : MonoBehaviour, IGoapAgent
        where TAgentAction : IAgentAction
    {
        // Properties to be set via inspector
        public TAgentAction[] agentActions;

        protected IAgentState goalState;
        public IAgentState currentState;
        public List<TAgentAction> actionPath;

        protected GoapData _goapData;

        protected int _currentActionToExecute = 0;
        protected AgentMemory agentMemory;
        protected IAgentGoalProvider _agentGoalProvider;
        protected GoapPlanner _planner;
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
            //_mirrorOfAgentActions = new TAgentAction[agentActions.Length];
            var state = ScriptableObject.CreateInstance<AgentState>();
            state.Set(AgentStateKey.CanWalk);//currentState.Clone(OnStateChange);
            currentState = state;
        }

        private void Start()
        {
            for (int i = 0; i < agentActions.Length; i++)
            {
                agentActions[i].OnStart(this);
            }
            _goapData = new GoapData();
            _planner = new GoapPlanner(_goapData, agentActions.Select(a => (IAgentAction) a).ToArray());
            agentMemory = GetComponent<AgentMemory>();
            _agentGoalProvider = GetComponent<IAgentGoalProvider>();
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
        public void SetState(IAgentState state)
        {
            currentState.AddState(state);
        }

        public void UnSetState(IAgentState state)
        {
            currentState.RemoveState(state);
        }

        public IAgentState GetCurrentState()
        {
            return currentState;
        }

        public TAgentAction[] GetAllActions()
        {
            return agentActions;
        }

        public void OnStateChange(IAgentState state)
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
                actionPath = _planner.FindActionsTo(currentState, goalState).Select(a => (TAgentAction)a).ToList();
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