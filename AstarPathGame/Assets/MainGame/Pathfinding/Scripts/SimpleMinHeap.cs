using System;

namespace Pathfinding
{
    public class SimpleHeapNode<T>
    {
        public float Priority { get; set; }
        public T Data { get; set; }
        private SimpleHeapNode(float priority, T data)
        {
            Priority = priority;
            Data = data;
        }

        public static SimpleHeapNode<T> New(float priority, T data)
        {
            return new SimpleHeapNode<T>(priority, data);
        }
    }

    public class SimpleMinHeap<TNode>
    {
        public readonly SimpleHeapNode<TNode>[] _elements;
        public int _size;

        public SimpleMinHeap(int size)
        {
            _elements = new SimpleHeapNode<TNode>[size];
        }

        private int GetLeftChildIndex(int elementIndex) => 2 * elementIndex + 1;
        private int GetRightChildIndex(int elementIndex) => 2 * elementIndex + 2;
        private int GetParentIndex(int elementIndex) => (elementIndex - 1) / 2;

        private bool HasLeftChild(int elementIndex) => GetLeftChildIndex(elementIndex) < _size;
        private bool HasRightChild(int elementIndex) => GetRightChildIndex(elementIndex) < _size;
        private bool IsRoot(int elementIndex) => elementIndex == 0;

        private SimpleHeapNode<TNode> GetLeftChild(int elementIndex) => _elements[GetLeftChildIndex(elementIndex)];
        private SimpleHeapNode<TNode> GetRightChild(int elementIndex) => _elements[GetRightChildIndex(elementIndex)];
        private SimpleHeapNode<TNode> GetParent(int elementIndex) => _elements[GetParentIndex(elementIndex)];

        private void Swap(int firstIndex, int secondIndex)
        {
            var temp = _elements[firstIndex];
            _elements[firstIndex] = _elements[secondIndex];
            _elements[secondIndex] = temp;
        }

        public bool IsEmpty()
        {
            return _size == 0;
        }

        public SimpleHeapNode<TNode> Peek()
        {
            return _size == 0 ?
                throw new IndexOutOfRangeException() : _elements[0];
        }

        public SimpleHeapNode<TNode> Pop()
        {
            if (_size == 0)
                throw new IndexOutOfRangeException();

            var result = _elements[0];
            _elements[0] = _elements[_size - 1];
            _size--;
            ReCalculateDown(0);
            return result;
        }

        public bool Add(float priority, TNode element)
        {
            if (_size == _elements.Length)
                return false;
            _elements[_size] = SimpleHeapNode<TNode>.New(priority, element);

            ReCalculateUp(_size);
            _size++;
            return true;
        }

        private void ReCalculateDown(int index)
        {
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
