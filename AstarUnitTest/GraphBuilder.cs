﻿using System.Collections.Generic;
using System.Linq;
using AstarAlgo;

namespace AstarUnitTest
{
    public static class A
    {
        public static GraphBuilder Graph => new GraphBuilder();
    }

    public class GraphBuilder
    {
        private Dictionary<string, LocationNode> _nodes;
        private List<DirectedEdge> _edges;

        public GraphBuilder()
        {
            _nodes = new Dictionary<string, LocationNode>();
            _edges = new List<DirectedEdge>();
        }

        public GraphBuilder WithNode(string nodeId, int x, int y)
        {
            if (!_nodes.ContainsKey(nodeId))
            {
                _nodes.Add(nodeId, new LocationNode(nodeId, x, y));
            }
            return this;
        }

        public GraphBuilder WithEdge(string fromNode, string toNode, double weight)
        {
            if (!_nodes.ContainsKey(fromNode)
                || !_nodes.ContainsKey(toNode))
            {
                return this;
            }

            var nodeA = _nodes[fromNode];
            var nodeB = _nodes[toNode];

            _edges.Add(new DirectedEdge(nodeA, nodeB, weight));

            return this;
        }

        public LocationAstar WithHeapOptimization()
        {
            var starGraph = new AstarDataWithHeap<LocationNode, DirectedEdge>
                (_nodes.Values.ToList(), _edges);

            return new LocationAstar(starGraph);
        }

        public LocationAstar WithBasicAlgo()
        {
            var starGraph = new BasicAstarData<LocationNode, DirectedEdge>
                (_nodes.Values.ToList(), _edges);

            return new LocationAstar(starGraph);
        }
    }
}