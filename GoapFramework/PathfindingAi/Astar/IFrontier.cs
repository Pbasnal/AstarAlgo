namespace PathfindingAi.Astar
{
    public interface IFrontier
    {
        void AddNodeToFrontier(IAstarPathNode node);
        IAstarPathNode GetClosestNode();
        bool IsEmpty();
        void Reset();
    }
}