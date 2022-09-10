namespace PathfindingAi.GraphStructure
{
    public interface IEdge
    {
        INode OriginNode { get; }
        INode DestinationNode { get; }

        bool IsDestinationNode(INode node);
    }
}