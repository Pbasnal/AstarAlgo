using PathfindingAi.GraphStructure;

namespace PathfindingAi.Astar
{
    public interface IWeightedEdge : IEdge
    {
        float Cost { get; }
    }
}