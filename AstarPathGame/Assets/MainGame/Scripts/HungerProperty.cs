using UnityEngine;

namespace MainGame
{
    public class HungerProperty : MonoBehaviour
    {
        public int maxHunger = 10;
        public float hungerGrowRate = 0.5f;
        public int hungerThresholdToBecomeHungry = 4;
        public float currentHunger = 0;

        private GoapAgent _agent;

        private void Start()
        {
            _agent = GetComponent<GoapAgent>();
        }

        private void Update()
        {
            currentHunger += Time.deltaTime * hungerGrowRate;

            if (currentHunger > hungerThresholdToBecomeHungry)
            {
                var currentState = (AgentState) _agent.GetCurrentState();
                currentState.StateValue |= AgentStateKey.IsNotHungry;
            }

            if (currentHunger >= maxHunger)
            {
                Debug.Log("Too hungry! Dead");
                gameObject.SetActive(false);
            }
        }

        public bool Eat(int food)
        {
            if (currentHunger == 0) return false;

            currentHunger = Mathf.Clamp(currentHunger - food, 0, maxHunger);
            return true;
        }
    }
}