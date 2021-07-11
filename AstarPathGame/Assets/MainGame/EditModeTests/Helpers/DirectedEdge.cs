using Pathfinding;

namespace TestHelpers
{
    public class DirectedEdge : IWeightedEdge<LocationNode>
    {
        public double Weight { get; set; }

        public LocationNode OriginNode { get; set; }
        public LocationNode DestinationNode { get; set; }

        public DirectedEdge()
        { }

        public DirectedEdge(LocationNode originNode, LocationNode destinationNode, double cost)
        {
            OriginNode = originNode;
            DestinationNode = destinationNode;
            Weight = cost;
        }
    }
}