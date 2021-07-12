using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Pathfinding;
using UnityEngine;

namespace PathfindingTests
{
    public struct TestHeapNode : IMinHeapNode
    {
        public double Priority { get; set; }

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
            var random = new System.Random();

            for (int i = 0; i < 100; i++)
            {
                pQueue.Add(TestHeapNode.New(random.Next(0, 101)));
            }

            List<TestHeapNode> tmp = new List<TestHeapNode>();
            var topvalue = pQueue.Pop();
            while (!pQueue.IsEmpty())
            {
                tmp.Add(topvalue);
                Assert.IsTrue(topvalue.Priority <= pQueue.Peek().Priority);
                topvalue = pQueue.Pop();
            }

            var tmp2 = tmp.Select(t => t.Priority).ToArray();

            Debug.Log($"Total tested values {tmp2.Length}");
        }
    }
}