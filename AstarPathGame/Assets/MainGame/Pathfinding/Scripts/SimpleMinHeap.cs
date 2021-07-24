using System;
using System.Threading;


namespace Pathfinding
{
    public class SimpleHeapNode : INodeWithPriority
    {
        public bool IsVisited { get; set; }
        public int Id { get; set; }
        public float Priority { get; set; }

        public SimpleHeapNode(int id, float priority)
        {
            Id = id;
            Priority = priority;
        }

        public string NodeId { get; }

        public int PreviousNode { get; set; }
        public float NodeCost { get; set; }
        public float HeuristicCost { get; set; }
    }

    public class SimpleMinHeap<TNode> where TNode : INodeWithPriority
    {
        public readonly TNode[] _elements;
        public int _size;
        public int[] _elementIndexes;

        public SimpleMinHeap(int size)
        {
            _elements = new TNode[size];
            _elementIndexes = new int[size];
            Reset();
        }

        private int GetLeftChildIndex(int elementIndex) => 2 * elementIndex + 1;
        private int GetRightChildIndex(int elementIndex) => 2 * elementIndex + 2;
        private int GetParentIndex(int elementIndex) => (elementIndex - 1) / 2;

        private bool HasLeftChild(int elementIndex) => GetLeftChildIndex(elementIndex) < _size;
        private bool HasRightChild(int elementIndex) => GetRightChildIndex(elementIndex) < _size;
        private bool IsRoot(int elementIndex) => elementIndex == 0;

        private TNode GetLeftChild(int elementIndex) => _elements[GetLeftChildIndex(elementIndex)];
        private TNode GetRightChild(int elementIndex) => _elements[GetRightChildIndex(elementIndex)];
        private TNode GetParent(int elementIndex) => _elements[GetParentIndex(elementIndex)];

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

        public TNode Peek()
        {
            return _size == 0 ?
                throw new IndexOutOfRangeException() : _elements[0];
        }

        public TNode Pop()
        {
            if (_size == 0)
                throw new IndexOutOfRangeException();

            var result = _elements[0];
            _elements[0] = _elements[_size - 1];
            _size--;
            ReCalculateDown(0);
            return result;
        }

        public bool Add(TNode element)
        {
            if (_elementIndexes[element.Id] == -1)
            {
                if (_size == _elements.Length)
                    return false; // throw new IndexOutOfRangeException();

                _elements[_size] = element;
                _elementIndexes[element.Id] = _size;

                ReCalculateUp(_size);
                _size++;
                return true;
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

            return true;
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
