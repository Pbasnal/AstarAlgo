namespace PathfindingAi.GraphStructure
{
    public interface INode
    {
        string StateName { get; }
        string Id { get; set; }
    }
}