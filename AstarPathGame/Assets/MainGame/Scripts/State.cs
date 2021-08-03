using System;
using UnityEngine;

namespace MainGame
{
    [Flags]
    public enum AgentStateKey
    {
        None = 0,
        TargetInSight = 1,
        TargetOutOfSight = 2,
        AgentInSight = 4,
        AgentOutOfSight = 8,
        TargetOutOfRange = 16,
        TargetInRange = 32,
        CanWalk = 64,
        CanNotWalk = 128,
        EnemyIsAlive = 256,
        EnemyIsDead = 512
    }

    [Serializable]
    public class State
    {
        [SerializeField]
        private AgentStateKey stateValue;
        public AgentStateKey StateValue => stateValue;

        public void Set(AgentStateKey stateKey) => stateValue |= stateKey;
        //public void UnSet(AgentStateKey stateKey) => stateValue &= ~stateKey;

        public override bool Equals(System.Object obj)
        {
            if (!(obj is State)) return false;

            State p = (State)obj;
            return StateValue == p.StateValue;
        }

        public override int GetHashCode()
        {
            return (int)StateValue;
        }
    }
}
