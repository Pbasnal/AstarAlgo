using System.Collections;
using System.Collections.Generic;
using GoapFramework;
using MainGame;
using UnityEngine;

namespace MainGame
{
    [RequireComponent(typeof(AgentGoalProvider))]
    public class GoapPlannerBehaviour : MonoBehaviour
    {
        public GoapAgent goapAgent;
        protected GoapPlanner _planner;
        protected IAgentGoalProvider _agentGoalProvider;

        protected int _currentActionToExecute = 0;

        void Start()
        {
            _agentGoalProvider = GetComponent<IAgentGoalProvider>();

            if (goapAgent == null)
            {
                Debug.Log("Add an implementation of GoapAgent.");
            }
            _planner = new GoapPlanner(goapAgent);
        }

        // Update is called once per frame
        void Update()
        {
            _planner.EvaluateGoalAndActionPath();
        }
    }
}