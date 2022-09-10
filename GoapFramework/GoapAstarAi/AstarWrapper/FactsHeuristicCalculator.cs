using PathfindingAi.Astar;
using PathfindingAi.AstarWrapper;
using PathfindingAi.GraphStructure;

namespace GoapAstarAi
{
    public class FactsHeuristicCalculator : ICalculateHeuristicCost
    {
        public float GetHeuristicCostBetween(IAstarPathNode fromNode, IAstarPathNode toNode)
        {
            var fromState = (AstarGoapNode)fromNode;
            var toState = (AstarGoapNode)toNode;

            return toState.DistanceFrom(fromState);
        }
    }
}