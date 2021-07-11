using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AstarUnitTest
{
    [TestClass]
    public class AstarUnitTests
    {
        private TestContext testContextInstance;

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

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
            var timeToGenerateGraph = Timer.CaptureExecutionTime(
                () => A.Graph
                    .WithNode("A", 0, 0)
                    .WithNode("B", 2, 5)
                    .WithNode("C", 5, 1)
                    .WithNode("G", 7, 7)

                    .WithEdge("A", "B", 7)
                    .WithEdge("A", "C", 7)
                    .WithEdge("B", "G", 8)
                    .WithEdge("C", "G", 12)
                    .WithBasicAlgo(),
                out var graph);
            
            TestContext.WriteLine($"Time to set up test graph {timeToGenerateGraph.TotalMilliseconds}");

            var executionTime = Timer.CaptureExecutionTime(
                    () => graph.FindPathBetween(0, 3),
                    out var path);

            TestContext.WriteLine($"Time to find path {executionTime.TotalMilliseconds}");

            var timer = Stopwatch.StartNew();
            Assert.AreEqual(2, path.Count);

            Assert.AreEqual("A", path[0].OriginNode.NodeId);
            Assert.AreEqual("B", path[0].DestinationNode.NodeId);
            Assert.AreEqual(7, path[0].Weight);

            Assert.AreEqual("B", path[1].OriginNode.NodeId);
            Assert.AreEqual("G", path[1].DestinationNode.NodeId);
            Assert.AreEqual(8, path[1].Weight);
            timer.Stop();
            TestContext.WriteLine($"Time to assert test  {timer.Elapsed.TotalMilliseconds}");
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
            var timeToGenerateGraph = Timer.CaptureExecutionTime(
                () => A.Graph
                    .WithNode("A", 0, 0)
                    .WithNode("B", 2, 5)
                    .WithNode("C", 5, 1)
                    .WithNode("G", 7, 7)

                    .WithEdge("A", "B", 7)
                    .WithEdge("A", "C", 7)
                    .WithEdge("B", "G", 12)
                    .WithEdge("C", "G", 8)
                    .WithBasicAlgo(),
                out var graph);

            TestContext.WriteLine($"Time to set up test graph {timeToGenerateGraph.TotalMilliseconds}");

            var executionTime = Timer.CaptureExecutionTime(
                    () => graph.FindPathBetween(0, 3),
                    out var path);

            TestContext.WriteLine($"Time to find path {executionTime.TotalMilliseconds}");

            var timer = Stopwatch.StartNew();
            Assert.AreEqual(2, path.Count);

            Assert.AreEqual("A", path[0].OriginNode.NodeId);
            Assert.AreEqual("C", path[0].DestinationNode.NodeId);
            Assert.AreEqual(7, path[0].Weight);

            Assert.AreEqual("C", path[1].OriginNode.NodeId);
            Assert.AreEqual("G", path[1].DestinationNode.NodeId);
            Assert.AreEqual(8, path[1].Weight);

            timer.Stop();
            TestContext.WriteLine($"Time to assert test  {timer.Elapsed.TotalMilliseconds}");
        }
    }
}