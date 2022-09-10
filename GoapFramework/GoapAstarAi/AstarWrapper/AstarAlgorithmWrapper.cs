using System;
using System.Collections.Generic;
using System.Linq;
using GoapAi;
using InsightsLogger;
using PathfindingAi.Astar;
using PathfindingAi.AstarWrapper;

namespace GoapAstarAi
{
    public class AstarAlgorithmWrapper : IFindActionPlan
    {
        private readonly AstarAlgorithm pathFinder;

        public AstarAlgorithmWrapper(
            AstarGraph astarGraph,
            ICalculateHeuristicCost factsHeuristics,
            ISimpleLogger logger)
        {
            pathFinder = new AstarAlgorithm(astarGraph, factsHeuristics, logger);
        }

        public IList<GoapAction> FindPlanForAgent(IUseGoapPlan agent)
        {
            var edges = new List<IWeightedEdge>();

            foreach (var action in agent.GetAgentActions())
            {
                edges.Add(new AstarGoapEdge(action));
            }
            var path = pathFinder.FindPathBetweenStates(
               new AstarGoapNode(agent.GetCurrentState()),
               new AstarGoapNode(agent.GetGoalStateToPlanFor()),
               edges);

            return path.Select(s => (AstarGoapEdge)s)
                    .Select(s => s.Action)
                    .ToList();
        }
    }
}