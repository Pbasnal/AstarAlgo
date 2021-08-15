using System;
using UnityEngine;
using BasnalGames.GoapFramework;

namespace MainGame
{

    [CreateAssetMenu(fileName = "AgentState", menuName = "Goap/AgentState", order = 52)]
    public class AgentState : ScriptableObject, IAgentState, IAgentStateFactory<AgentState>
    {
        public AgentStateKey StateValue;
        private Action<IAgentState> onStateChange;

        public static AgentState NewState(Action<IAgentState> stateChangeCallback)
        {
            var newAgentState = ScriptableObject.CreateInstance<AgentState>();
            if (stateChangeCallback != null)
            {
                newAgentState.onStateChange += stateChangeCallback;
            }
            return newAgentState;
        }

        public AgentState New(Action<IAgentState> stateChangeCallback)
        {
            return NewState(stateChangeCallback);
        }

        public IAgentState Clone(Action<IAgentState> onStateChangeCallback = null)
        {
            var newState = New(onStateChangeCallback ?? this.onStateChange);
            newState.name = this.name;
            newState.AddState(this);

            return newState;
        }

        public int AgentStateValue() => GetHashCode();

        public override bool Equals(System.Object obj)
        {
            if (!(obj is AgentState)) return false;

            var p = (AgentState)obj;
            return StateValue == p.StateValue;
        }

        public bool Contains(IAgentState inputState)
        {
            var state = inputState as AgentState;
            if (state == null) return false;

            return (StateValue & state.StateValue) == state.StateValue;
        }

        public override int GetHashCode()
        {
            return (int)StateValue;
        }

        public IAgentState IntersectState(IAgentState inputState)
        {
            var state = inputState as AgentState;
            if (state == null) return null;

            var stateIntersection = Clone() as AgentState;
            stateIntersection.StateValue &= state.StateValue;

            return stateIntersection;
        }

        public void AddState(IAgentState stateInfoToUpdateWith)
        {
            var state = stateInfoToUpdateWith as AgentState;
            if (state == null) return;

            Set(state.StateValue);
        }

        public void RemoveState(IAgentState stateInfoToUpdateWith)
        {
            var state = stateInfoToUpdateWith as AgentState;
            if (state == null) return;
            UnSet(state.StateValue);
        }

        public void Set(AgentStateKey stateToAdd)
        {
            StateValue |= stateToAdd;
            OnStateChange(this);
        }

        public void UnSet(AgentStateKey stateToRemove)
        {
            StateValue &= ~stateToRemove;
            OnStateChange(this);
        }

        public float DistanceFrom(IAgentState inputState)
        {
            var state = inputState as AgentState;
            if (state == null) return float.MaxValue;

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

        public void OnStateChange(IAgentState state)
        {
            onStateChange?.Invoke(state);
        }
    }
}
