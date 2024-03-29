﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Pathfinding;
using UnityEngine;

namespace PathfindingTests
{
    public struct TestHeapNode : IMinHeapNode
    {
        public float Priority { get; set; }
        public int Id { get; set; }

        public string NodeId { get; }
        public int PreviousNode { get; set; }
        public float NodeCost { get; set; }
        public float HeuristicCost { get; set; }

        public TestHeapNode(int id, float val)
        {
            Id = id;
            Priority = val;

            NodeId = string.Empty;
            PreviousNode = 0;
            NodeCost = 0;
            HeuristicCost = 0;
        }

        public static TestHeapNode New(int id, float val)
            => new TestHeapNode(id, val);
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
                var newNode = TestHeapNode.New(i, random.Next(0, 101));
                pQueue.Add(newNode);
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

        [Test]
        public void Duplicate_node_test()
        {
            var pQueue = new MinHeap<TestHeapNode>(100);
            var random = new System.Random();

            TestHeapNode node = default(TestHeapNode);
            for (int i = 0; i < 10; i++)
            {
                node = TestHeapNode.New(i + 1, (i + 1) * 10);
                pQueue.Add(node);
            }

            Assert.AreEqual(1, pQueue.Peek().Id);
            node.Priority = 0;
            pQueue.Add(node);
            Assert.AreEqual(10, pQueue.Peek().Id);
        }
    }
}