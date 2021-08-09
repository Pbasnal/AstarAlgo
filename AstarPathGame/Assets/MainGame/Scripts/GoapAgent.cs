using System.Collections.Generic;
using System.Linq;
using MainGame.Sensors;
using UnityEngine;

namespace MainGame
{
    public class GoapAgent : AGoapAgent<AgentState>
    {
        public void SetState(AgentStateKey state)
        {
            currentState.Set(state);
        }

        public void UnSetState(AgentStateKey state)
        {
            if ((currentState.StateValue & state) == 0) return;
            currentState.UnSet(state);
        }
    }
}