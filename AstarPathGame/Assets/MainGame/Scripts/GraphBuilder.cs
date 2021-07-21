using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pathfinding;
using UnityEngine;

namespace MainGame
{
    public static class A
    {
        public static GraphBuilder GraphBuilder(Node[] grid)
            => new GraphBuilder(grid);
    }

    public class GraphBuilder
    {
        private Dictionary<string, WeightedEdge> _edges;
        private Node[] _grid;

        public GraphBuilder(Node[] grid)
        {
            _grid = grid;
            _edges = new Dictionary<string, WeightedEdge>();
        }

        public GraphBuilder WithEdge(
            int fromNodeIndex,
            int toNodeIndex,
            double weight)
        {
            var edgeKey = $"{fromNodeIndex}{toNodeIndex}";

            if (_edges.ContainsKey(edgeKey)
                || !(_grid[fromNodeIndex].isWalkable 
                    & _grid[toNodeIndex].isWalkable))
            {
                // this edge already exists
                // or one of the node is not walkable
                return this;
            }

            _edges.Add(edgeKey, 
                new WeightedEdge(_grid[fromNodeIndex], 
                    _grid[toNodeIndex], 
                    weight));

            return this;
        }

        public IAstarData<Node, WeightedEdge> WithHeapOptimization()
        {
            var edges = _edges.Values.ToArray();
            var starGraphData = new AstarDataWithHeap<Node, WeightedEdge>
                (_grid, edges);

            return starGraphData;
        }

        public IAstarData<Node, WeightedEdge> WithoutHeap()
        {
            var starGraphData = new BasicAstarData<Node, WeightedEdge>
                (_grid, _edges.Values.ToArray());

            return starGraphData;
        }
    }
}
