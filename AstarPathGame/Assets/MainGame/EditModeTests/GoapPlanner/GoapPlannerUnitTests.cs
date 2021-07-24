using System;
using System.Collections.Generic;
using System.Linq;
using MainGame;
using NUnit.Framework;

namespace PathfindingTests
{
    public class GoapPlannerUnitTests
    {
        [Test]
        public void TestGoap()
        {
            var actions = GetAllAgentActions().ToArray();
            var goapData = new GoapData(actions);
            var planner = new GoapPlanner(goapData);

            goapData.SetState(AgentStateKey.CanWalk, 1);

            var destinationNode = goapData.currentState.Clone();
            destinationNode.Set(AgentStateKey.EnemyIsDead, 1);

            var actionPath = planner.FindActionsTo(destinationNode);
        }

        private IEnumerable<IAgentAction> GetAllAgentActions()
        {
            return System.Reflection.Assembly.GetAssembly(typeof(IAgentAction))
                .GetTypes()
                .Where(type => typeof(IAgentAction).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
                .Select(type => (IAgentAction)Activator.CreateInstance(type));
        }
    }
}
