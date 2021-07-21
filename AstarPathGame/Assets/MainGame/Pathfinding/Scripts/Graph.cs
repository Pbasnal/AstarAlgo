namespace Pathfinding
{
    public interface INode
    {
        public string NodeId { get; }
        public int Id { get; set; }

        public int PreviousNode { get; set; }
        public float NodeCost { get; set; }
        public float HeuristicCost { get; set; }
    }

    public interface IEdge<T> 
        where T : INode
    {
        T OriginNode { get; set; }
        T DestinationNode { get; set; }
    }

    public interface IWeightedEdge<T> : IEdge<T> 
        where T : INode
    {
        new T OriginNode { get; set; }
        public float Weight { get; set; }
    }
}
