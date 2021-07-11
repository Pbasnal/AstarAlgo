using Pathfinding;

namespace TestHelpers
{
    public class LocationNode : INodeWithPriority
    {
        public int Priority { get; set; }

        public string NodeId { get; set; }

        public int Id { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

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