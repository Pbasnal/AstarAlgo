using GoapAi;
using PathfindingAi.Astar;
using PathfindingAi.GraphStructure;

namespace PathfindingAi.AstarWrapper
{
    public class AstarGoapEdge : IWeightedEdge
    {
        public GoapAction Action { get; }
        public INode OriginNode { get; }

        public INode DestinationNode { get; }

        public float Cost { get; }

        public AstarGoapEdge(GoapAction action)
        {
            Action = action;
            this.OriginNode = new AstarGoapNode(action.PreConditions);
            this.DestinationNode = new AstarGoapNode(action.Effects);
        }

        public bool IsDestinationNode(INode node)
        {
            AstarGoapNode pathNode = (AstarGoapNode)node;
            return Action.CanReachStateWithAction(pathNode.AstarState);
        }
    }
}