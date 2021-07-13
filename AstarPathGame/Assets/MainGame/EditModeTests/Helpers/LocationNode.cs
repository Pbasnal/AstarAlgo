using Pathfinding;

namespace TestHelpers
{
    public class LocationNode : INodeWithPriority
    {
        public double Priority { get; set; }

        public string NodeId { get; set; }

        public int Id { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public int PreviousNode { get; set; }
        public double NodeCost { get; set; }
        public double HeuristicCost { get; set; }

        public LocationNode()
        { }

        public LocationNode(string nodeId, int x, int y)
        {
            NodeId = nodeId;
            X = x;
            Y = y;
        }
    }
}