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

        public override bool Execute()
        {
            bool interacted = false;
            var colliders = Physics.OverlapSphere(_agent.transform.position, 5, consumableLayers);
            for (int i = 0; i < colliders.Length; i++)
            {
                if(!colliders[i].TryGetComponent<IInteractable>(out var interactable)) continue;
                interactable.Interact(_agent.gameObject);

                if(!colliders[i].gameObject.activeSelf)
                {
                    _agent.RemoveFromMemory(interactable);
                }

                interacted = true;
            }

            Debug.Log($"Eat action called - Interacted = {interacted}");
            if(interacted)
            {
                _agent.SetState(AgentStateKey.IsNotHungry);
                _agent.UnSetState(AgentStateKey.TargetInRange | AgentStateKey.TargetInSight);
            }

            return interacted;
        }

        public override void Init(GoapAgent goapAgent)
        {
            preConditions = new State();
            preConditions.Set(AgentStateKey.CanWalk);
            preConditions.Set(AgentStateKey.TargetInRange);
            preConditions.Set(AgentStateKey.TargetInSight);

            effects = new State();
            effects.Set(AgentStateKey.IsNotHungry);

            Weight = 1;

            _agent = goapAgent;
            _agentMemory = goapAgent.GetComponent<AgentMemory>();
            if (_agentMemory == null)
            {
                var up = new UnityException("Add agent memory to use in patrol action"); ;
                throw up;
            }
        }

        public override bool ValidateAction(GoapAgent agent)
        {
            return CheckPreconditions(agent.currentState);
        }
    }
}

