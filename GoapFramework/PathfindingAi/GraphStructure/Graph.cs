using System.Collections.Generic;
using InsightsLogger;

namespace PathfindingAi.GraphStructure
{
    public class Graph<TNode, TEdge>
        where TNode : INode
        where TEdge : IEdge
    {
        public TNode Origin { get; protected set; }
        public TNode Destination { get; protected set; }
        protected readonly List<TEdge> Edges;

        private ISimpleLogger logger;

        public Graph(ISimpleLogger logger)
        {
            this.logger = logger;
            Edges = new List<TEdge>();
        }

        public void AddOrigin(TNode node)
        {
            Origin = node;
            Origin.Id = "origin";
        }

        public void AddDestination(TNode node)
        {
            Destination = node;
            Destination.Id = "destination";
        }

        public void AddEdge(TEdge edge) => Edges.Add(edge);

        public void RemoveEdge(TEdge edge) => Edges.Remove(edge);

        public IEnumerable<TEdge> GetEdgesFromNode(TNode node)
        {
            logger.LogDebug($"Getting edges for {node.StateName}");

            foreach (var edge in Edges)
            {
                // logger.LogDebug($"OriginName: {edge.OriginNode.StateName} DestinationNode: {edge.DestinationNode.StateName}");
                if (edge.IsDestinationNode(node))
                {
                    // var typeofDestinationNode = edge.DestinationNode.GetType();
                    // logger.LogDebug($"Type of desitnation node {typeofDestinationNode}");
                    yield return edge;
                }
            }
        }
    }
}