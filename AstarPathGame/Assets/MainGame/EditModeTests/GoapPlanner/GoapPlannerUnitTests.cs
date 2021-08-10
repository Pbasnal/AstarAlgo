using System;
using System.Collections.Generic;
using System.Linq;
using MainGame;
using NUnit.Framework;
using UnityEngine;
using GoapFramework;

namespace GoapPlannerTests
{
    public class GoapPlannerUnitTests
    {
        [Test]
        public void TestGoap()
        {
            var actions = GetAllAgentActions().ToArray();
            Assert.IsTrue(actions != null && actions.Count() > 0);

            var gameObject = new GameObject();
            gameObject.AddComponent<PatrolBehaviour>();
            gameObject.AddComponent<GoapAgent>();
            var goapAgent = gameObject.GetComponent<GoapAgent>();

            var goapData = new GoapData();
            var planner = new GoapPlanner(goapData, actions);

            var currentState = ScriptableObject.CreateInstance<AgentState>();
            currentState.Set(AgentStateKey.CanWalk);

            var destinationNode = (AgentState)currentState.Clone();
            destinationNode.Set(AgentStateKey.EnemyIsDead);

            var actionPath = planner.FindActionsTo(currentState, destinationNode);
            Assert.IsNotNull(actionPath);
            Assert.IsTrue(actionPath.Count() == 2);

            foreach (var action in actionPath)
            {
                Debug.Log($"Action: {action.GetType().Name}");
            }
        }

        private IEnumerable<AnActionWithAgentState> GetAllAgentActions()
        {
            return System.Reflection.Assembly.GetAssembly(typeof(AnActionWithAgentState))
                .GetTypes()
                .Where(type => typeof(AnActionWithAgentState).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                .Select(type => (AnActionWithAgentState)Activator.CreateInstance(type));
        }
    }
}
