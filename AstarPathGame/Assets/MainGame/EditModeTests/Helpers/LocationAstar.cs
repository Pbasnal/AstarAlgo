using System;
using BasnalGames.Pathfinding;

namespace TestHelpers
{
    public class LocationAstar : AstarPath<LocationNode, DirectedEdge>
    {
        public LocationAstar(IAstarData<LocationNode, DirectedEdge> astarData)
            : base(astarData)
        {
        }

        public override float HeuristicCost(LocationNode fromNode, LocationNode toNode)
        {
            var xDiffSquared = Math.Pow(toNode.X - fromNode.X, 2);
            var yDiffSquared = Math.Pow(toNode.Y - fromNode.Y, 2);

            return (float)Math.Sqrt(xDiffSquared + yDiffSquared);
        }
    }
}