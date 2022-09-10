using PathfindingAi.GraphStructure;

namespace PathfindingAi.Astar
{
    public interface ICalculateHeuristicCost
    {
        float GetHeuristicCostBetween(IAstarPathNode fromNode, IAstarPathNode toNode);
    }
}