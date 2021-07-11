namespace AstarAlgo
{
    public interface INode
    {
        public string NodeId { get; }
        public int Id { get; set; }
    }

    public interface IEdge
    {
        public INode OriginNode { get; set; }
        public INode DestinationNode { get; set; }
    }

    public interface IWeightedEdge : IEdge
    {
        public double Weight { get; set; }
    }
}
