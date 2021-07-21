using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PriorityQueue.Tests
{
    [TestClass()]
    public class MinHeapPerformanceTests
    {
        static int size = 50000;
        static TestHeapNode[] testNodes;

        [ClassInitializeAttribute]
        public static void ClassInitialize(TestContext context)
        {
            var random = new Random();
            testNodes = new TestHeapNode[size];
            for (int i = 0; i < size; i++)
            {
                testNodes[i] = TestHeapNode.New(random.Next(0, size));
            }
        }

        [TestMethod()]
        public void PopTest()
        {
            var pQueue = new MinHeap<TestHeapNode>(size);
            for (int i = 0; i < size; i++)
            {
                pQueue.Add(testNodes[i]);
            }

            while (!pQueue.IsEmpty()) pQueue.Pop();
        }

        [TestMethod()]
        public void PopAsyncTest()
        {
            var pQueueAsync = new MinHeapAsync<TestHeapNode>(size);
            for (int i = 0; i < size; i++)
            {
                pQueueAsync.Add(testNodes[i]);
            }

            while (!pQueueAsync.IsEmpty()) pQueueAsync.PopAsync();
        }

        [TestMethod()]
        public void ThreadTest()
        {
            var numberOfThreads = 4;
            var tknSource = new CancellationTokenSource();
            var cVector = new CorrelationVector("test");

            Console.WriteLine($"{cVector.Vector} Starting tests");

            var threadSync = new ThreadSync(cVector,
                numberOfThreads,
                tknSource.Token);

            tknSource.CancelAfter(1000);

            threadSync.Run();
            for (int i = 0; i < threadSync.data.Length; i++)
            {
                if (threadSync.data[i]) continue;
                foreach (var log in threadSync.logs)
                {
                    Console.WriteLine(log);
                }
            }
        }
    }

    public class ThreadSync
    {
        Task[] backgroundTasks;
        ConcurrentQueue<int> jobQueue;
        int _numberOfTasks;
        public bool[] data;

        public List<string> logs;

        CorrelationVector[] cVectors;

        private CancellationToken _token;
        public ThreadSync(CorrelationVector cVector, int numberOfTasks, CancellationToken token)
        {
            _numberOfTasks = numberOfTasks;
            _token = token;
            jobQueue = new ConcurrentQueue<int>();
            backgroundTasks = new Task[numberOfTasks];
            cVectors = new CorrelationVector[numberOfTasks];
            data = new bool[numberOfTasks];
            logs = new List<string>();

            for (int i = 0; i < numberOfTasks; i++)
            {
                cVectors[i] = cVector.Extends();

                data[i] = true;
                jobQueue.Enqueue(i);
                backgroundTasks[i] = new Task(RunAsync);
            }

            for (int i = 0; i < numberOfTasks; i++)
            {
                backgroundTasks[i].Start();
            }
        }

        public bool IsComplete()
        {
            for (int i = 0; i < _numberOfTasks; i++)
            {
                if (data[i] == false) return false;
            }
            return true;
        }

        public void RunAsync()
        {
            while (!_token.IsCancellationRequested)
            {
                if (!jobQueue.TryDequeue(out var jobIndex)) return;

                var cVector = cVectors[jobIndex];

                if (data[jobIndex]) continue;

                Thread.Sleep(1);
                logs.Add($"{cVector.Vector} job[{jobIndex}]={data[jobIndex]}");
                data[jobIndex] = true;
                cVector.Increment();
            }
        }

        public void Run()
        {
            var cVector = CorrelationVector.correlationVector;
            for (int i = 0; i < _numberOfTasks; i++)
            {
                data[i] = false;
                logs.Add($"{cVector.Vector} setting job[{i}]={data[i]}");
                cVector.Increment();
            }


            while (!IsComplete()
                && !_token.IsCancellationRequested)
                continue;
        }
    }
}