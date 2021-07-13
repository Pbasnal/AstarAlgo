using System;
using Pathfinding;

namespace TestHelpers
{
    public class LocationAstar : AstarPath<LocationNode, DirectedEdge>
    {
        public LocationAstar(IAstarData<LocationNode, DirectedEdge> astarData)
            : base(astarData)
        {
        }

        public override double HeuristicCost(ref LocationNode fromNode, ref LocationNode toNode)
        {
            var xDiffSquared = Math.Pow(toNode.X - fromNode.X, 2);
            var yDiffSquared = Math.Pow(toNode.Y - fromNode.Y, 2);

            return Math.Sqrt(xDiffSquared + yDiffSquared);
        }
    }
}