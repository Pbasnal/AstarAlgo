using System;
using NUnit.Framework;
using Pathfinding;

namespace PathfindingTests
{
    public struct TestHeapNode : IMinHeapNode
    {
        public int Priority { get; }

        public TestHeapNode(int val)
        {
            Priority = val;
        }

        public static TestHeapNode New(int val) => new TestHeapNode(val);
    }

    public class MinHeapTests
    {
        [Test]
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
                Assert.IsTrue(topvalue.Priority <= pQueue.Peek().Priority);
                topvalue = pQueue.Pop();
            }
        }
    }
}