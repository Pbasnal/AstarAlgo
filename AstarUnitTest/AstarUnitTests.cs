using System;
using System.Collections.Generic;
using AstarAlgo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AstarUnitTest
{
    [TestClass]
    public class AstarUnitTests
    {
        /// (A) -- 7 --> (B) 
        /// (A) -- 7 --> (C) 
        /// (B) -- 8 --> (G)
        /// (C) -- 12 --> (G)
        /// 
        /// Shortest path from (A) to (G)
        /// (A) -- 7 --> (B) -- 8 --> (G)
        [TestMethod]
        public void Simple_4_node_graph()
        {
            var allNodes = new List<LocationNode>();
            var allEdges = new List<DirectedEdge>();

            var nodeA = new LocationNode("A", 0, 0);
            var nodeB = new LocationNode("B", 2, 5);
            var nodeC = new LocationNode("C", 5, 1);
            var nodeG = new LocationNode("G", 7, 7);
            allNodes.Add(nodeA);
            allNodes.Add(nodeB);
            allNodes.Add(nodeC);
            allNodes.Add(nodeG);

            var edgeAB = new DirectedEdge(nodeA, nodeB, 7);
            var edgeAC = new DirectedEdge(nodeA, nodeC, 7);
            var edgeBG = new DirectedEdge(nodeB, nodeG, 8);
            var edgeCG = new DirectedEdge(nodeC, nodeG, 12);
            allEdges.Add(edgeAB);
            allEdges.Add(edgeAC);
            allEdges.Add(edgeBG);
            allEdges.Add(edgeCG);

            var graph = new LocationAstar(allNodes, allEdges);
            var path = graph.FindPathBetween(0, 3);

            Assert.AreEqual(2, path.Count);

            Assert.AreEqual("A", path[0].OriginNode.NodeId);
            Assert.AreEqual("B", path[0].DestinationNode.NodeId);
            Assert.AreEqual(7, path[0].Weight);

            Assert.AreEqual("B", path[1].OriginNode.NodeId);
            Assert.AreEqual("G", path[1].DestinationNode.NodeId);
            Assert.AreEqual(8, path[1].Weight);
        }

        /// (A) -- 7 --> (B) 
        /// (A) -- 7 --> (C) 
        /// (B) -- 12 --> (G)
        /// (C) -- 8 --> (G)
        /// 
        /// Shortest path from (A) to (G)
        /// (A) -- 7 --> (C) -- 8 --> (G)
        [TestMethod]
        public void Simple_4_node_graph2()
        {
            var allNodes = new List<LocationNode>();
            var allEdges = new List<DirectedEdge>();

            var nodeA = new LocationNode("A", 0, 0);
            var nodeB = new LocationNode("B", 2, 5);
            var nodeC = new LocationNode("C", 5, 1);
            var nodeG = new LocationNode("G", 7, 7);
            allNodes.Add(nodeA);
            allNodes.Add(nodeB);
            allNodes.Add(nodeC);
            allNodes.Add(nodeG);

            var edgeAB = new DirectedEdge(nodeA, nodeB, 7);
            var edgeAC = new DirectedEdge(nodeA, nodeC, 7);
            var edgeBG = new DirectedEdge(nodeB, nodeG, 12);
            var edgeCG = new DirectedEdge(nodeC, nodeG, 8);
            allEdges.Add(edgeAB);
            allEdges.Add(edgeAC);
            allEdges.Add(edgeBG);
            allEdges.Add(edgeCG);

            var graph = new LocationAstar(allNodes, allEdges);
            var path = graph.FindPathBetween(0, 3);
            Assert.AreEqual(2, path.Count);

            Assert.AreEqual("A", path[0].OriginNode.NodeId);
            Assert.AreEqual("C", path[0].DestinationNode.NodeId);
            Assert.AreEqual(7, path[0].Weight);

            Assert.AreEqual("C", path[1].OriginNode.NodeId);
            Assert.AreEqual("G", path[1].DestinationNode.NodeId);
            Assert.AreEqual(8, path[1].Weight);
        }
    }

    public class LocationNode : INode
    {
        public int X { get; set; }
        public int Y { get; set; }

        public string NodeId { get; set; }

        int INode.Id { get; set; }

        public LocationNode()
        { }

        public LocationNode(string nodeId, int x, int y)
        {
            NodeId = nodeId;
            X = x;
            Y = y;
        }
    }

    public class DirectedEdge : IWeightedEdge
    {
        public double Weight { get; set; }

        public INode OriginNode { get; set; }
        public INode DestinationNode { get; set; }

        public DirectedEdge()
        { }

        public DirectedEdge(INode originNode, INode destinationNode, float cost)
        {
            OriginNode = originNode;
            DestinationNode = destinationNode;
            Weight = cost;
        }
    }

    public class LocationAstar : AstarPath<LocationNode, DirectedEdge>
    {
        public LocationAstar(List<LocationNode> nodes, List<DirectedEdge> edges)
            : base(nodes, edges)
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