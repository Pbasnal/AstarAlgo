using System.Collections.Generic;
using System.Linq;
using MainGame.Sensors;
using UnityEngine;

namespace MainGame
{
    public class GoapAgent : AGoapAgent<AnAgentAction<AgentState>>
    {
        //* If we create an update method here, then
        //* then the update of parent will not be called. 
        //* Design package around that limitation
        private void Update() {
        }
    }
}