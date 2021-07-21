//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Threading;
//using System.Threading.Tasks;
//using NUnit.Framework;
//using Pathfinding;
//using UnityEngine;

//namespace PathfindingTests
//{
//    public class MinHeapAnalyzer
//    {
//        [Test]
//        public void Test_memory_allocation()
//        {
//            var startingMemory = GC.GetTotalMemory(true);
//            var size = 1000;
//            var pQueue = new MinHeap<TestHeapNode>(size);
//            var memoryAtHeapCreation = GC.GetTotalMemory(true);

//            Debug.Log($"Generation of priority queue : {GC.GetGeneration(pQueue)}");
//            Debug.Log($"Memory consumed by priority queue: {memoryAtHeapCreation - startingMemory}");

//            TestHeapNode[] nodes = new TestHeapNode[size];
//            for (int i = 0; i < size; i++)
//            {
//                nodes[i] = TestHeapNode.New(i, (i + 1) * size);
//            }
//            var memoryConsumedByNodes = GC.GetTotalMemory(true);
//            Debug.Log($"Generation of priority queue : {GC.GetGeneration(nodes)}");
//            Debug.Log($"Memory consumed by nodes: {memoryConsumedByNodes - memoryAtHeapCreation}");


//            var memoryBeforeExecution = GC.GetTotalMemory(true);
//            for (int i = 0; i < size; i++)
//            {
//                try
//                {
//                    pQueue.Add(nodes[i]);
//                }
//                catch
//                {
//                    Debug.Log($"Failed for szie={size} i={i}");
//                    throw;
//                }
//            }
//            var memoryAfterExecution = GC.GetTotalMemory(true);
//            Debug.Log($"Memory consumed during execution: {memoryAfterExecution - memoryBeforeExecution}");

//            Assert.AreEqual(0, pQueue.Peek().Id);
//            nodes[size - 1].Priority = 0;
//            try
//            {
//                pQueue.Add(nodes[size - 1]);
//            }
//            catch
//            {
//                Debug.Log($"Failed for szie={size} i={nodes[size - 1].Id}");
//                throw;
//            }
//            Assert.AreEqual(size - 1, pQueue.Peek().Id);
//        }


//        [Test]
//        public void Test_memory_allocation_of_simple_heap()
//        {
//            var startingMemory = GC.GetTotalMemory(true);
//            var size = 1000;
//            var tokenSource = new CancellationTokenSource();
//            var pQueue = new SimpleMinHeap(size, 1, tokenSource.Token);
//            var memoryAtHeapCreation = GC.GetTotalMemory(true);

//            Debug.Log($"Generation of priority queue : {GC.GetGeneration(pQueue)}");
//            Debug.Log($"Memory consumed by priority queue: {memoryAtHeapCreation - startingMemory}");

//            var nodes = new SimpleHeapNode[size];
//            for (int i = 0; i < size; i++)
//            {
//                nodes[i] = new SimpleHeapNode(i, (i + 1) * size);
//            }
//            var memoryConsumedByNodes = GC.GetTotalMemory(true);
//            Debug.Log($"Generation of nodes : {GC.GetGeneration(nodes)}");
//            Debug.Log($"Memory consumed by nodes: {memoryConsumedByNodes - memoryAtHeapCreation}");

//            var memoryBeforeExecution = GC.GetTotalMemory(true);
//            for (int i = 0; i < size; i++)
//            {
//                pQueue.Add(nodes[i]);
//            }
//            var memoryAfterExecution = GC.GetTotalMemory(true);
//            Debug.Log($"Memory consumed during execution: {memoryAfterExecution - memoryBeforeExecution}");

//            Assert.AreEqual(0, pQueue.Peek().Id);
//            nodes[size - 1].Priority = 0;
//            pQueue.Add(nodes[size - 1]);
//            Assert.AreEqual(size - 1, pQueue.Peek().Id);
//        }

//        [Test]
//        public void Test_heap_add_processing_time()
//        {
//            var size = 5000;
//            var timer = System.Diagnostics.Stopwatch.StartNew();
//            var random = new System.Random();
//            var tokenSource = new CancellationTokenSource();
//            var minHeapGeneric = new MinHeap<TestHeapNode>(size);
//            var tNodes = new TestHeapNode[size];

//            var pQueue = new SimpleMinHeap(size, 1, tokenSource.Token);
//            var nodes = new SimpleHeapNode[size];

//            for (int i = 0; i < size; i++)
//            {
//                tNodes[i] = TestHeapNode.New(i, random.Nextfloat() * 100);
//                nodes[i] = new SimpleHeapNode(i, random.Nextfloat() * 100);
//            }

//            Debug.Log("\nAnalysis of generic heap");
//            timer.Restart();
//            for (int i = 0; i < size; i++) minHeapGeneric.Add(tNodes[i]);
//            Debug.Log($"Time taken to add all nodes: {timer.ElapsedMilliseconds} ms");
//            Debug.Log($"Avg time taken to add all nodes: {timer.ElapsedMilliseconds / (float)size}ms");

//            /*================================================*/
//            Debug.Log("\nAnalysis of simple heap");
//            timer.Restart();
//            for (int i = 0; i < size; i++) pQueue.Add(nodes[i]);
//            Debug.Log($"Time taken to add all nodes: {timer.ElapsedMilliseconds} ms");
//            Debug.Log($"Avg time taken to add all nodes: {timer.ElapsedMilliseconds / (float)size}ms");
//        }

//        [Test]
//        public void Test_heap_pop_processing_time()
//        {
//            var size = 10000;
//            var timer = System.Diagnostics.Stopwatch.StartNew();
//            var random = new System.Random();

//            var minHeapGeneric = new MinHeap<TestHeapNode>(size);
//            var tNodes = new TestHeapNode[size];

//            for (int i = 0; i < size; i++)
//            {
//                tNodes[i] = TestHeapNode.New(i, random.Nextfloat() * 100);
//            }

//            Debug.Log("\nAnalysis of generic heap");
//            timer.Restart();
//            for (int i = 0; i < size; i++) minHeapGeneric.Add(tNodes[i]);
//            Debug.Log($"Time taken to add all nodes: {timer.ElapsedMilliseconds} ms");
//            Debug.Log($"Avg time taken to add nodes: {timer.ElapsedMilliseconds / (float)size}ms");

//            /*================================================*/
//            Debug.Log("\nAnalysis of heap pop");
//            timer.Restart();
//            while (!minHeapGeneric.IsEmpty()) minHeapGeneric.Pop();
//            Debug.Log($"Time taken to pop all nodes: {timer.ElapsedMilliseconds} ms");
//            Debug.Log($"Avg time taken to pop nodes: {timer.ElapsedMilliseconds / (float)size}ms");
//        }


//        [Test]
//        public void Parallel_processing()
//        {
//            var size = 1000;
//            var cancellationSource = new CancellationTokenSource();
//            var random = new System.Random();
//            var numberOfJobs = 2;
//            var increments = size / numberOfJobs;

//            var heap = new SimpleMinHeap(size, numberOfJobs, cancellationSource.Token);
//            for (int i = 0; i < size; i++)
//            {
//                heap.Add(new SimpleHeapNode(i, random.Nextfloat() * 100));
//            }

//            var timer = System.Diagnostics.Stopwatch.StartNew();
//            var prevValue = heap.Pop();
//            while (!heap.IsEmpty())
//            {
//                //Assert.IsTrue(prevValue.Priority < heap.Peek().Priority);
//                prevValue = heap.Pop();
//            }
//            Debug.Log($"Normal pop time: {timer.ElapsedMilliseconds}ms");
//            Debug.Log($"Avg normal pop time: {timer.ElapsedMilliseconds / (float)size}ms");
//            Debug.Log("\n\n");

//            heap.Reset();
//            for (int i = 0; i < size; i++) heap.Add(new SimpleHeapNode(i, random.Nextfloat() * 100));
//            cancellationSource.CancelAfter(1000 * 1); // cancel after 5 secs

//            var token = cancellationSource.Token;
//            timer.Restart();

//            var popTicks = DateTime.UtcNow.Ticks;
//            prevValue = heap.PopParallel(increments);
//            while (!heap.IsEmpty() && !token.IsCancellationRequested)
//            {
//                //Assert.IsTrue(prevValue.Priority < heap.Peek().Priority, $"Failed at i:{heap.Peek()}");
//                prevValue = heap.PopParallel(increments);
//            }
//            Debug.Log($"Number of msgs: {heap.numberOfMsgs}\n\n");
//            Debug.Log($"Parallel pop time: {timer.ElapsedMilliseconds}ms");
//            Debug.Log($"Avg parallel pop time: {timer.ElapsedMilliseconds / (float)size}ms");

//            Debug.Log($"{DateTime.UtcNow.Ticks} Total process time {heap.totalTime} ");

//            //Debug.Log($"Total time to enqueue {heap.avgTimeToEnqueue }");
//            //Debug.Log($"Total wait time {heap.avgWaitTime }");
//            //Debug.Log($"Total pop call time {heap.avgPopTime}");
//            //Debug.Log($"Total init time {heap.avgInitTime}");

//            //Debug.Log($"Avg time to enqueue {heap.avgTimeToEnqueue / size}");
//            //Debug.Log($"Avg wait time {heap.avgWaitTime / size}");
//            //Debug.Log($"Avg pop call time {heap.avgPopTime / size}");
//            //Debug.Log($"Avg init time {heap.avgInitTime / size}");

//            MsgStats(heap.msgPool);
//        }

//        private void MsgStats(Msg[] msgs)
//        {
//            float totalDequeueTime = 0;
//            float totalProcessTime = 0;
//            for (int i = 0; i < msgs.Length; i++)
//            {
//                totalDequeueTime += msgs[i].dequeuedAt - msgs[i].queuedAt;
//                totalProcessTime += msgs[i].finishedAt - msgs[i].dequeuedAt;
//            }

//            Debug.Log($"\n\nTime to dequeue all messages {totalDequeueTime / (float)10000}");
//            Debug.Log($"Average to dequeue messages {totalDequeueTime / (float)10000 / msgs.Length}\n");

//            Debug.Log($"Time to process all messages {totalProcessTime / (float)10000}");
//            Debug.Log($"Average to process messages {totalProcessTime / (float)10000 / msgs.Length}");
//        }


//        [Test]
//        public void Time_to_enqueue()
//        {
//            var datetime = new DateTime();
//            int size = 10 * 10000;
//            var primaryQueue = new Queue<int>();
//            var refQueue = new Queue<TestMsg>();

//            var startingTicks = DateTime.UtcNow.Ticks;
//            for (int i = 0; i < size; i++)
//            {
//                primaryQueue.Enqueue(i);
//            }
//            Debug.Log($"Primary datatype enqueue time {(DateTime.UtcNow.Ticks - startingTicks) / (float)10000}");

//            var msgPool = new TestMsg[size];
//            for (int i = 0; i < size; i++)
//            {
//                msgPool[i] = new TestMsg();
//            }

//            startingTicks = DateTime.UtcNow.Ticks;
//            for (int i = 0; i < size; i++)
//            {
//                var time = datetime.Ticks;
//                refQueue.Enqueue(msgPool[i]);
//            }
//            Debug.Log($"Ref datatype enqueue time {(DateTime.UtcNow.Ticks - startingTicks) / (float)10000}");
//        }

//        [Test]
//        public void TaskStartTime()
//        {
//            var numberOfTasks = 1000;

//            var tasks = new Task[numberOfTasks];
//            var timeTaken = new ConcurrentQueue<float>();

//            var timer = System.Diagnostics.Stopwatch.StartNew();
//            for (int i = 0; i < numberOfTasks; i++)
//            {
//                tasks[i] = Task.Run(() => 
//                {
//                    timeTaken.Enqueue(timer.ElapsedMilliseconds);
//                });
//            }

//            Task.WaitAll(tasks);
//            Debug.Log($"Time to complete the tasks {timer.ElapsedMilliseconds}");

//            var totalTime = 0.0;
//            while (!timeTaken.IsEmpty)
//            {
//                if (!timeTaken.TryDequeue(out var time)) continue;
//                totalTime += time;
//            }
//            Debug.Log($"Total time to start tasks {totalTime / numberOfTasks}");
//        }


//        [Test]
//        public void SplitHeap()
//        {
//            var size = 1000;
//            var cancellationSource = new CancellationTokenSource();
//            var random = new System.Random();
//            var numberOfSplits = 4;

//            var heap = new SimpleMinHeap(size, numberOfSplits, cancellationSource.Token);
//            for (int i = 0; i < size; i++)
//            {
//                heap.Add(new SimpleHeapNode(i, random.Nextfloat() * 100));
//            }

//            var timer = DateTime.UtcNow.Ticks;// System.Diagnostics.Stopwatch.StartNew();
//            var prevValue = heap.Pop();
//            while (!heap.IsEmpty())
//            {
//                //Assert.IsTrue(prevValue.Priority < heap.Peek().Priority);
//                prevValue = heap.Pop();
//            }
//            Debug.Log($"Normal pop time: {(DateTime.UtcNow.Ticks - timer)/(float)10000}ms");
//            //Debug.Log($"Avg normal pop time: {timer.ElapsedMilliseconds / (float)size}ms");
//            Debug.Log("\n\n");

//            var splitHeap = new SplitHeap(size, numberOfSplits);
//            for (int i = 0; i < size; i++) 
//                splitHeap.Add(new SimpleHeapNode(i, random.Nextfloat() * 100));

//            timer = DateTime.UtcNow.Ticks;//.Restart();
//            prevValue = splitHeap.Pop();
//            while (!splitHeap.IsEmpty())
//            {
//                //Assert.IsTrue(prevValue.Priority < heap.Peek().Priority, $"Failed at i:{heap.Peek()}");
//                prevValue = splitHeap.Pop();
//            }
//            Debug.Log($"Split pop time: {(DateTime.UtcNow.Ticks - timer) / (float)10000}ms");
//            //Debug.Log($"Parallel pop time: {timer.ElapsedMilliseconds}ms");
//            //Debug.Log($"Avg parallel pop time: {timer.ElapsedMilliseconds / (float)size}ms");
//        }
//    }

//    public class TestMsg
//    {
//        public int msgId;
//    }
//}



