using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PriorityQueue.Tests
{
    public class TestHeapNode : IMinHeapNode
    {
        public int Value { get; }

        public TestHeapNode(int val)
        {
            Value = val;
        }

        public static TestHeapNode New(int val) => new TestHeapNode(val);
    }

    [TestClass()]
    public class MinHeapTests
    {
        [TestMethod()]
        public void PopTest()
        {
            var pQueue = new MinHeap<TestHeapNode>(100);
            var random = new Random();

            for (int i = 0; i < 100; i++)
            {
                pQueue.Add(TestHeapNode.New(random.Next(0, 101)));
            }

            var topvalue = pQueue.Pop();
            while (!pQueue.IsEmpty())
            {
                Assert.IsTrue(topvalue.Value <= pQueue.Peek().Value);
                topvalue = pQueue.Pop();
            }
        }
    }
}