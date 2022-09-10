using GoapAi;
using PathfindingAi.Astar;

namespace PathfindingAi.AstarWrapper
{
    public class AstarGoapNode : IAstarPathNode
    {
        public State AstarState { get; }
        public string StateName { get; }
        public IAstarPathNode PreviousNode { get; set; }
        public IWeightedEdge EdgeFromPreviousNode { get; set; }
        public float CostOfPathToNode { get; set; }
        public float HeuristicCost { get; set; }
        public bool IsVisited { get; set; }
        public string Id { get; set; }

        public AstarGoapNode(State state)
        {
            this.StateName = state.StateName;
            this.AstarState = state;
        }

        public float DistanceFrom(AstarGoapNode node)
        {
            return AstarState.DistanceFrom(node.AstarState);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not AstarGoapNode nodeToCompare)
            {
                return false;
            }
            return AstarState.Equals(nodeToCompare.AstarState);
        }

        public static float GetTotalCostOfNode(IAstarPathNode node) => node.CostOfPathToNode + node.HeuristicCost;

        public int CompareTo(IAstarPathNode? other)
        {
            return GetTotalCostOfNode(this).CompareTo(GetTotalCostOfNode(other));
        }
    }
}