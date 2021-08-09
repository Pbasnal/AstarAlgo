using System;
using UnityEngine;

namespace MainGame
{
    public interface IAgentState<TAgentState>
        where TAgentState : IAgentState<TAgentState>
    {
        TAgentState Clone(Action<TAgentState> onStateChangeCallback = null);
        int AgentStateValue();
        float DistanceFrom(TAgentState state);
        TAgentState IntersectState(TAgentState state);
        void OnStateChange(TAgentState state);

        bool Contains(TAgentState state);
    }

    [CreateAssetMenu(fileName = "AgentState", menuName = "Goap/AgentState", order = 52)]
    public class AgentState : ScriptableObject, IAgentState<AgentState>
    {
        public AgentStateKey StateValue;
        private Action<AgentState> onStateChange;

        public static AgentState New(Action<AgentState> stateChangeCallback)
        {
            var newAgentState = ScriptableObject.CreateInstance<AgentState>();
            newAgentState.onStateChange += stateChangeCallback;

            return newAgentState;
        }

        public AgentState Clone(Action<AgentState> onStateChangeCallback = null)
        {
            var newState = AgentState.New(onStateChangeCallback ?? this.onStateChange);
            newState.name = this.name;
            newState.Set(StateValue);

            return newState;
        }

        public void Set(AgentStateKey stateKey)
        {
            StateValue |= stateKey;
            OnStateChange(this);
        }

        public void UnSet(AgentStateKey stateKey)
        {
            StateValue &= ~stateKey;
            OnStateChange(this);
        }

        public int AgentStateValue() => GetHashCode();

        public override bool Equals(System.Object obj)
        {
            if (!(obj is AgentState)) return false;

            var p = (AgentState)obj;
            return StateValue == p.StateValue;
        }

        public bool Contains(AgentState state)
        {
            return (StateValue & state.StateValue) == state.StateValue;
        }

        public override int GetHashCode()
        {
            return (int)StateValue;
        }

        public AgentState IntersectState(AgentState state)
        {
            var stateIntersection = Clone();
            stateIntersection.StateValue &= state.StateValue;

            return stateIntersection;
        }

        public float DistanceFrom(AgentState state)
        {
            int XOR = (int)StateValue ^ (int)state.StateValue;
            // Check for 1's in the binary form using
            // Brian Kerninghan's Algorithm
            float count = 0;
            while (XOR > 0)
            {
                XOR &= XOR - 1;
                count++;
            }
            // return the count of different bits
            return count;
        }

        public void OnStateChange(AgentState state)
        {
            onStateChange?.Invoke(state);
        }
    }
}
