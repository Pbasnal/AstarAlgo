using System;
using System.Diagnostics;
using InsightsLogger;

namespace Pathfinding
{
    public interface IMinHeapNode : INode
    {
        public double Priority { get; set; }
    }

    public class MinHeap<T> where T : IMinHeapNode
    {
        public readonly T[] _elements;
        private int _size;


        //private Dictionary<int, int> _elementIndexes;
        private int[] _elementIndexes;

        public MinHeap(int size)
        {
            _elements = new T[size];
            _elementIndexes = new int[size];//new Dictionary<int, int>();
            Reset();
        }

        private int GetLeftChildIndex(int elementIndex) => 2 * elementIndex + 1;
        private int GetRightChildIndex(int elementIndex) => 2 * elementIndex + 2;
        private int GetParentIndex(int elementIndex) => (elementIndex - 1) / 2;

        private bool HasLeftChild(int elementIndex) => GetLeftChildIndex(elementIndex) < _size;
        private bool HasRightChild(int elementIndex) => GetRightChildIndex(elementIndex) < _size;
        private bool IsRoot(int elementIndex) => elementIndex == 0;

        private T GetLeftChild(int elementIndex) => _elements[GetLeftChildIndex(elementIndex)];
        private T GetRightChild(int elementIndex) => _elements[GetRightChildIndex(elementIndex)];
        private T GetParent(int elementIndex) => _elements[GetParentIndex(elementIndex)];

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

        public T Peek()
        {
            return _size == 0 ?
                throw new IndexOutOfRangeException() : _elements[0];
        }

        public T Pop()
        {
            if (_size == 0)
                throw new IndexOutOfRangeException();

            var result = _elements[0];
            _elements[0] = _elements[_size - 1];
            _size--;

            ReCalculateDown(0);

            return result;
        }

        public void Add(ref T element)
        {
            var timer = Stopwatch.StartNew();
            if (_size == _elements.Length)
                throw new IndexOutOfRangeException();

            if (_elementIndexes[element.Id] == -1)
            {
                _elements[_size] = element;
                _elementIndexes[element.Id] = _size;

                //RuntimeLogger.LogDebug("MinHeap", $"UpdateElement time {timer.Elapsed.TotalMilliseconds}", timer);
                var heapUpTime = ExecutionTimer.Time(() => ReCalculateUp(_size));
                //RuntimeLogger.LogDebug("MinHeap", $"heapUp time {heapUpTime.TotalMilliseconds}", heapUpTime);
                _size++;
                return;
            }

            var elementId = _elementIndexes[element.Id];
            _elements[elementId] = element;
            if (element.Priority <= _elements[elementId].Priority)
            {
                var heapUpTime = ExecutionTimer.Time(() => ReCalculateUp(elementId));
                //RuntimeLogger.LogDebug("MinHeap", $"heapUp2 time {heapUpTime.TotalMilliseconds}", heapUpTime);
            }
            else
            {
                var heapDownTime = ExecutionTimer.Time(() => ReCalculateDown(elementId));
                //RuntimeLogger.LogDebug("MinHeap", $"heapDown time {heapDownTime.TotalMilliseconds}", heapDownTime);
            }
        }

        public void Reset()
        {
            _size = 0;
            for (int i = 0; i < _elementIndexes.Length; i++)
            {
                _elementIndexes[i] = -1;
            }
            //RuntimeLogger.LogDebug("MinHeap", "ResetSize", _size);
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
    }
}