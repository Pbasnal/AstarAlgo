using BasnalGames.Pathfinding;

namespace TestHelpers
{
    public class LocationNode : INodeWithPriority
    {
        public bool IsVisited { get; set; }
        public float Priority { get; set; }

        public string NodeId { get; set; }

        public int Id { get; set; }

        public int X { get; set; }
        public int Y { get; set; }

        public int PreviousNode { get; set; }
        public float NodeCost { get; set; }
        public float HeuristicCost { get; set; }

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