using UnityEngine;

namespace MainGame
{
    [
        CreateAssetMenu(
            fileName = "AgentState",
            menuName = "Goap/AgentState",
            order = 52)
    ]
    public class AgentState : ScriptableObject
    {
        [SerializeField]
        private State state;
        public State State => state;

        public static AgentState New()
        {
            var state = ScriptableObject.CreateInstance<AgentState>();
            state.state = new State();

            return state;
        }

        public AgentState Clone()
        {
            var newState = AgentState.New();
            newState.State.Set(State.StateValue);

            return newState;
        }

        public void Set(AgentStateKey stateKey) => State.Set(stateKey);

        public int AgentStateValue() => State.GetHashCode();
    }
}
