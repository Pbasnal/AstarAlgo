using GoapAi;
using InsightsLogger;
using PathfindingAi.Astar;

namespace GoapAstarAi
{
    public class GoapPlannerFactory
    {
        public static GoapPlanner GetAGoapPlanner(ISimpleLogger logger)
        {
            var frontier = new SortedSetFrontier(logger);
            var astarGraph = new AstarGraph(frontier, logger);

            var factsHeuristics = new FactsHeuristicCalculator();
            var pathFinder = new AstarAlgorithmWrapper(astarGraph, factsHeuristics, logger);
            return new GoapPlanner(pathFinder, logger);
        }        
    }
}