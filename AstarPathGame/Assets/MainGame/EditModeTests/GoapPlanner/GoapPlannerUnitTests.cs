using System;
using System.Collections.Generic;
using System.Linq;
using MainGame;
using NUnit.Framework;
using UnityEngine;

namespace GoapPlannerTests
{
    public class GoapPlannerUnitTests
    {
        [Test]
        public void TestGoap()
        {
            var actions = GetAllAgentActions().ToArray();
            Assert.IsTrue(actions != null && actions.Count() > 0);

            foreach (var action in actions) action.Init();
            
            var goapData = new GoapData<AgentState>();
            var planner = new GoapPlanner(goapData, actions);

            var currentState = AgentState.New();
            currentState.Set(AgentStateKey.CanWalk);

            var destinationNode = currentState.Clone();
            destinationNode.Set(AgentStateKey.EnemyIsDead);

            var actionPath = planner.FindActionsTo(currentState, destinationNode);
            Assert.IsNotNull(actionPath);
            Assert.IsTrue(actionPath.Count() == 2);

            foreach (var action in actionPath)
            {
                Debug.Log($"Action: {action.GetType().Name}");
            }
        }

        private IEnumerable<AnAgentAction> GetAllAgentActions()
        {
            return System.Reflection.Assembly.GetAssembly(typeof(AnAgentAction))
                .GetTypes()
                .Where(type => typeof(AnAgentAction).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                .Select(type => (AnAgentAction)Activator.CreateInstance(type));
        }
    }
}
