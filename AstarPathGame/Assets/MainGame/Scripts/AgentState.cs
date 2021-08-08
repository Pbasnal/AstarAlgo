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
        public AgentStateKey StateValue;
        //public AgentStateKey StateValue => stateValue;

        public static AgentState New()
        {
            return ScriptableObject.CreateInstance<AgentState>();
        }

        public AgentState Clone()
        {
            var newState = AgentState.New();
            newState.name = this.name;
            newState.Set(StateValue);

            return newState;
        }

        public void Set(AgentStateKey stateKey) => StateValue |= stateKey;

        public void UnSet(AgentStateKey stateKey) => StateValue &= ~stateKey;

        public int AgentStateValue() => GetHashCode();

        public override bool Equals(System.Object obj)
        {
            if (!(obj is AgentState)) return false;

            var p = (AgentState)obj;
            return StateValue == p.StateValue;
        }

        public override int GetHashCode()
        {
            return (int)StateValue;
        }
    }
}
