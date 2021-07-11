using AstarAlgo;

namespace AstarUnitTest
{
    public class DirectedEdge : IWeightedEdge
    {
        public double Weight { get; set; }

        public INode OriginNode { get; set; }
        public INode DestinationNode { get; set; }

        public DirectedEdge()
        { }

        public DirectedEdge(INode originNode, INode destinationNode, double cost)
        {
            OriginNode = originNode;
            DestinationNode = destinationNode;
            Weight = cost;
        }
    }
}