using UnityEngine;
using MainGame;

namespace MainGame.Actions
{
    [CreateAssetMenu(fileName = "EatAction", menuName = "GoapActions/EatAction", order = 52)]
    public class EatAction : AnAgentAction
    {
        public LayerMask consumableLayers;
        private AgentMemory _agentMemory;
        private GoapAgent _agent;

        public override void OnStart(GoapAgent goapAgent)
        {
            Weight = 1;

            _agent = goapAgent;
            _agentMemory = goapAgent.GetComponent<AgentMemory>();
            if (_agentMemory == null)
            {
                var up = new UnityException("Add agent memory to use in patrol action"); ;
                throw up;
            }
        }

        public override bool Execute()
        {
            bool interacted = false;
            var colliders = Physics.OverlapSphere(_agent.transform.position, 5, consumableLayers);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (!colliders[i].TryGetComponent<IInteractable>(out var interactable)) continue;
                interactable.Interact(_agent.gameObject);

                if (!colliders[i].gameObject.activeSelf)
                {
                    _agent.RemoveFromMemory(interactable);
                }

                interacted = true;
            }

            Debug.Log($"Eat action called - Interacted = {interacted}");
            if (interacted)
            {
                _agent.SetState(AgentStateKey.IsNotHungry);
                _agent.UnSetState(AgentStateKey.TargetInRange | AgentStateKey.TargetInSight);
            }

            return colliders.Length == 0 || interacted;
        }

        protected override AgentStateKey ApplyEffects(AgentStateKey currentState)
        {
            return currentState | AgentStateKey.IsNotHungry;
        }

        protected override AgentStateKey ApplyPreConditions(AgentStateKey currentState)
        {
            return currentState | AgentStateKey.CanWalk
                | AgentStateKey.TargetInRange
                | AgentStateKey.TargetInSight;
        }
    }
}

