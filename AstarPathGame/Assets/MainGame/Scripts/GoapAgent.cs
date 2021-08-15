using System.Collections.Generic;
using System.Linq;
using MainGame.Sensors;
using UnityEngine;
using GoapFramework;
namespace MainGame
{
    [RequireComponent(typeof(GoapPlannerBehaviour))]
    [RequireComponent(typeof(AgentGoalProvider))]
    public class GoapAgent : MonoBehaviour, IGoapAgent
    {
        // Properties to be set via inspector
        public AnActionWithAgentState[] agentActions;
        protected IAgentGoalProvider _agentGoalProvider;
        public IAgentState currentState;
        public List<AnActionWithAgentState> actionPath;

        protected int _currentActionToExecute = 0;
        protected AgentMemory agentMemory;
        protected GoapPlannerBehaviour _planner;
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

            var state = ScriptableObject.CreateInstance<AgentState>();
            //TODO: Hack to set starting state
            state.Set(AgentStateKey.CanWalk);
            currentState = state;
        }

        private void Start()
        {
            for (int i = 0; i < agentActions.Length; i++)
            {
                agentActions[i].OnStart(this);
            }

            _planner = GetComponent<GoapPlannerBehaviour>();
            agentMemory = GetComponent<AgentMemory>();
        }

        private void Update()
        {
            if (_currentActionToExecute >= actionPath.Count)
            {
                //* In case agent has executed all the actions but
                //* hasn't recieved a new goal, this will cause it
                //* to do nothing and just stand there.
                //TODO: need to plan for this scenario.
                actionPath.Clear();
                return;
            }

            var isActionComplete = actionPath[_currentActionToExecute].Execute();
            if (isActionComplete)
            {
                _currentActionToExecute += 1;
            }
        }

        public void OnActionPathUpdate(List<IAgentAction> actionPath)
        {
            this.actionPath = actionPath
                                .Select(a => (AnActionWithAgentState)a)
                                .ToList();
            _currentActionToExecute = 0;
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

        public IAgentAction[] GetAllActions()
        {
            return agentActions;
        }

        public void OnStateChange(IAgentState state)
        {
            if (currentState.Equals(state)) return;
        }

        public void UpdateMemory(IInteractable interactable)
        {
            if (!agentMemory.AddInteractable(interactable)) return;
        }

        public void RemoveFromMemory(IInteractable interactable)
        {
            agentMemory.Remove(interactable);
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

        IAgentAction[] IGoapAgent.GetAllActions()
        {
            return this.agentActions.Select(a => (IAgentAction)a).ToArray();
        }

        public IAgentGoalProvider GetGoalProvider()
        {
            return _agentGoalProvider;
        }
    }
}