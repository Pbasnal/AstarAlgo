using System.Collections.Generic;

namespace AstarAlgo
{
    public interface INode
    {
        public string NodeId { get; }
        //float HeuristicCost(INode goalNode);
    }

    public interface IEdge
    {
        INode FromNode { get; }
        INode ToNode { get; }

        float Cost { get; }        
    }

    // Adjaceny list implementation
    public class Graph
    {
        public List<INode> Nodes;
        public List<IEdge> Edges;

        public Graph(List<INode> nodes, List<IEdge> edges)
        {
            Nodes = nodes;
            Edges = edges;
        }

        public List<IEdge> FindPathBetween(int fromNodeIndex, int toNodeIndex)
        {
            var path = new List<IEdge>();

            return path;
        }

        public float HeuristicCost(int fromNodeIndex, int toNodeIndex)
        {
            return 1;
        }
    }
}
