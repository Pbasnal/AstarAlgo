using System;
using System.Collections.Generic;
using System.Linq;
using MainGame;
using NUnit.Framework;

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
            
            var goapData = new GoapData(actions);
            var planner = new GoapPlanner(goapData);

            goapData.SetState(AgentStateKey.CanWalk, 1);

            var destinationNode = goapData.currentState.Clone();
            destinationNode.Set(AgentStateKey.EnemyIsDead, 1);

            var actionPath = planner.FindActionsTo(destinationNode);
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
