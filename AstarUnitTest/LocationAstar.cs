using System;
using AstarAlgo;

namespace AstarUnitTest
{
    public class LocationAstar : AstarPath<LocationNode, DirectedEdge>
    {
        public LocationAstar(IAstarData<LocationNode, DirectedEdge> astarData)
            : base(astarData)
        {
        }

        public override double HeuristicCost(LocationNode fromNode, LocationNode toNode)
        {
            var xDiffSquared = Math.Pow(toNode.X - fromNode.X, 2);
            var yDiffSquared = Math.Pow(toNode.Y - fromNode.Y, 2);

            return Math.Sqrt(xDiffSquared + yDiffSquared);
        }
    }
}