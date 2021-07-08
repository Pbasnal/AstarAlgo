using System.Collections.Generic;
using AstarAlgo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AstarUnitTest
{
    [TestClass]
    public class AstarUnitTests
    {
        [TestMethod]
        public void Simple_4_node_graph()
        {
            var graph = GenerateTestGraph();
            var path = graph.FindPathBetween(0, 3);

            Assert.AreEqual(2, path.Count);

            foreach (var edge in path)
            {
                Assert.AreNotEqual("3", edge.FromNode.NodeId);
                Assert.AreNotEqual("3", edge.ToNode.NodeId);
            }
        }

        /// <summary>
        /// (1) -- 1 --> (2) 
        /// (1) -- 1 --> (3) 
        /// (2) -- 2 --> (4)
        /// (3) -- 3 --> (4)
        /// 
        /// Shortest path from (1) to (4)
        /// (1) -- 1 --> (2) -- 2 --> (4)
        /// </summary>
        /// <returns></returns>
        public Graph GenerateTestGraph()
        {
            var allNodes = new List<INode>();
            var allEdges = new List<IEdge>();

            var node1 = new Node("1");
            var node2 = new Node("2");
            var node3 = new Node("3");
            var node4 = new Node("4");
            allNodes.Add(node1);
            allNodes.Add(node2);
            allNodes.Add(node3);
            allNodes.Add(node4);

            var edge12 = new Edge(node1, node2, 1);
            var edge13 = new Edge(node1, node3, 1);
            var edge24 = new Edge(node2, node4, 2);
            var edge34 = new Edge(node3, node4, 3);
            allEdges.Add(edge12);
            allEdges.Add(edge13);
            allEdges.Add(edge24);
            allEdges.Add(edge34);

            return new Graph(allNodes, allEdges);
        }
    }

    public class Node : INode
    {
        public string NodeId { get; set; }

        //public float HeuristicCost(INode goalNode)
        //{
        //    return 1; // should be changed to something better
        //}

        public Node(string nodeId)
        {
            NodeId = nodeId;
        }
    }

    public class Edge : IEdge
    {
        public INode FromNode { get; }

        public INode ToNode { get; }

        public float Cost { get; }

        public Edge(Node fromNode, Node toNode, float cost)
        {
            FromNode = fromNode;
            ToNode = toNode;
            Cost = cost;
        }
    }
}
