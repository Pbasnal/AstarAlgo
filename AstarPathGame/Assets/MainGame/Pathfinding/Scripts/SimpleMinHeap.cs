using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Pathfinding
{

    public struct Msg
    {
        public int index;
        public int size;

        public bool isComplete;

        public long timeToProcessMessage;

        public long queuedAt;
        public long dequeuedAt;
        public long finishedAt;

        public System.Diagnostics.Stopwatch stopwatch;

        public DateTime tQueueTime;

        //public Msg()
        //{
        //    isComplete = true;
        //}
    }

    public abstract class HeapBackgroundJob
    {
        public int numberOfMsgs = 0;

        public ConcurrentQueue<int> jobIndexes;
        public Msg[] msgPool;
        public object[] jobLocks;

        public bool allMsgsComplete = false;

        protected CancellationToken _cancellationToken;
        protected int _numberOfJobs;
        protected DateTime dateTime;

        public HeapBackgroundJob(int numberOfJobs, CancellationToken token)
        {
            _cancellationToken = token;
            _numberOfJobs = numberOfJobs;

            jobIndexes = new ConcurrentQueue<int>();
            msgPool = new Msg[numberOfJobs];
            jobLocks = new object[numberOfJobs];
            dateTime = new DateTime();

            for (int i = 0; i < numberOfJobs; i++)
            {
                msgPool[i] = new Msg
                {
                    isComplete = true,
                    stopwatch = new System.Diagnostics.Stopwatch()
                };
                jobIndexes.Enqueue(i);
                jobLocks[i] = new object();
                Task.Factory.StartNew(Run, TaskCreationOptions.LongRunning);
            }
        }

        public void EnqueueJob(int jobIndex, int index, int size)
        {
            lock (jobLocks[jobIndex])
            {
                msgPool[jobIndex].index = index;
                msgPool[jobIndex].size = size;
                msgPool[jobIndex].stopwatch.Restart();
                msgPool[jobIndex].isComplete = false;
            }
            numberOfMsgs++;
        }

        protected bool IsComplete()
        {
            return allMsgsComplete;
        }

        protected void Run()
        {
            if (!jobIndexes.TryDequeue(out int jobIndex)) return;

            while (!_cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(1);
                lock (jobLocks[jobIndex])
                {
                    if (msgPool[jobIndex].isComplete || msgPool[jobIndex].size <= 0)
                    {
                        var isComplete = true;
                        for (int i = 0; i < msgPool.Length; i++)
                        {
                            isComplete &= msgPool[i].isComplete & msgPool[jobIndex].size <= 0;
                        }
                        allMsgsComplete = isComplete;
                        continue;
                    }
                    try
                    {
                        var size = msgPool[jobIndex].size;
                        msgPool[jobIndex].dequeuedAt = msgPool[jobIndex].stopwatch.ElapsedTicks;
                        if (!RunAsync(msgPool[jobIndex].index, size))
                        { Debug.Log($"Processing {msgPool[jobIndex].index} <> {msgPool[jobIndex].size} ({size})"); }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogException(ex);
                    }
                    finally
                    {
                        msgPool[jobIndex].finishedAt = msgPool[jobIndex].stopwatch.ElapsedTicks;
                        msgPool[jobIndex].isComplete = true;
                        msgPool[jobIndex].size = -100;
                    }
                }
            }
        }

        protected abstract bool RunAsync(int index, int size);
    }

    public class SimpleHeapNode
    {
        public int Id;
        public double Priority;

        public SimpleHeapNode(int id, double priority)
        {
            Id = id;
            Priority = priority;
        }
    }

    public class SimpleMinHeap : HeapBackgroundJob
    {
        public readonly SimpleHeapNode[] _elements;
        private int _size;

        private int[] _elementIndexes;

        public double avgTimeToEnqueue;
        public double avgWaitTime;
        public double avgPopTime;
        public double avgInitTime;

        public SimpleMinHeap(int size, int numberOfJobs, CancellationToken token)
            : base(numberOfJobs, token)
        {
            _elements = new SimpleHeapNode[size];
            _elementIndexes = new int[size];
            Reset();
        }

        private int GetLeftChildIndex(int elementIndex) => 2 * elementIndex + 1;
        private int GetRightChildIndex(int elementIndex) => 2 * elementIndex + 2;
        private int GetParentIndex(int elementIndex) => (elementIndex - 1) / 2;

        private bool HasLeftChild(int elementIndex) => GetLeftChildIndex(elementIndex) < _size;
        private bool HasRightChild(int elementIndex) => GetRightChildIndex(elementIndex) < _size;
        private bool IsRoot(int elementIndex) => elementIndex == 0;

        private SimpleHeapNode GetLeftChild(int elementIndex) => _elements[GetLeftChildIndex(elementIndex)];
        private SimpleHeapNode GetRightChild(int elementIndex) => _elements[GetRightChildIndex(elementIndex)];
        private SimpleHeapNode GetParent(int elementIndex) => _elements[GetParentIndex(elementIndex)];

        private void Swap(int firstIndex, int secondIndex)
        {
            var temp = _elements[firstIndex];
            _elements[firstIndex] = _elements[secondIndex];
            _elements[secondIndex] = temp;

            _elementIndexes[_elements[firstIndex].Id] = firstIndex;
            _elementIndexes[_elements[secondIndex].Id] = secondIndex;
        }

        public bool IsEmpty()
        {
            return _size == 0;
        }

        public SimpleHeapNode Peek()
        {
            return _size == 0 ?
                throw new IndexOutOfRangeException() : _elements[0];
        }

        public int startingSize;
        public SimpleHeapNode PopParallel(int increment)
        {
            if (_size == 0)
                throw new IndexOutOfRangeException();

            var result = _elements[0];
            increment = _size / _numberOfJobs + 1;

            var index = 0;
            startingSize = _size;
            for (int i = 0; i < _numberOfJobs; i++)
            {
                var size = index + increment;
                size = (size > _size) ? _size : size;
                EnqueueJob(i, index, size);
                index += increment;
            }

            while (!IsComplete() && !_cancellationToken.IsCancellationRequested) ;

            _size--;
            return result;
        }

        public SimpleHeapNode Pop()
        {
            if (_size == 0)
                throw new IndexOutOfRangeException();

            var result = _elements[0];
            _elements[0] = _elements[_size - 1];
            _size--;
            ReCalculateDown(0);
            return result;
        }

        public void Add(SimpleHeapNode element)
        {
            if (_elementIndexes[element.Id] == -1)
            {
                if (_size == _elements.Length)
                    throw new IndexOutOfRangeException();

                _elements[_size] = element;
                _elementIndexes[element.Id] = _size;

                ReCalculateUp(_size);
                _size++;
                return;
            }

            var elementId = _elementIndexes[element.Id];
            _elements[elementId] = element;
            if (element.Priority <= _elements[elementId].Priority)
            {
                ReCalculateUp(elementId);
            }
            else
            {
                ReCalculateDown(elementId);
            }
        }

        public void Reset()
        {
            _size = 0;
            for (int i = 0; i < _elementIndexes.Length; i++)
            {
                _elementIndexes[i] = -1;
            }
        }

        private void ReCalculateDown(int index)
        {
            //int index = 0;
            while (HasLeftChild(index))
            {
                var smallerIndex = GetLeftChildIndex(index);
                if (HasRightChild(index)
                    && GetRightChild(index).Priority < GetLeftChild(index).Priority)
                {
                    smallerIndex = GetRightChildIndex(index);
                }

                if (_elements[smallerIndex].Priority >= _elements[index].Priority)
                {
                    break;
                }

                Swap(smallerIndex, index);
                index = smallerIndex;
            }
        }

        private void ReCalculateDown(int index, int size)
        {
            var leftChildIndex = GetLeftChildIndex(index);
            var rightChildIndex = GetRightChildIndex(index);

            while (leftChildIndex < size)
            {
                var smallerIndex = leftChildIndex;

                if (rightChildIndex < size
                    && _elements[rightChildIndex].Priority < _elements[leftChildIndex].Priority)
                {
                    smallerIndex = rightChildIndex;
                }

                if (_elements[smallerIndex].Priority >= _elements[index].Priority)
                {
                    break;
                }

                Swap(smallerIndex, index);
                index = smallerIndex;

                leftChildIndex = GetLeftChildIndex(index);
                rightChildIndex = GetRightChildIndex(index);
            }
        }

        private void ReCalculateUp(int index)
        {
            //var index = _size - 1;
            while (!IsRoot(index)
                && _elements[index].Priority < GetParent(index).Priority)
            {
                var parentIndex = GetParentIndex(index);
                Swap(parentIndex, index);
                index = parentIndex;
            }
        }

        protected override bool RunAsync(int index, int size)
        {
            try
            {
                size--;
                _elements[index] = _elements[size];
                ReCalculateDown(index, size);
            }
            catch (Exception ex)
            {
                Debug.Log($"{ex.Message} index: {index}  size:{size} actual size:{_size}  starting size:{startingSize}");
                return false;
            }
            return true;
        }
    }
}
