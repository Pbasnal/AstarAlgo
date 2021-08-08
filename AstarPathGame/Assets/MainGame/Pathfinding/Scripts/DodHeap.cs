using System;

namespace Pathfinding
{
    public struct DodHeapNode
    {
        public float Priority;
        public int DataIndex;
        private DodHeapNode(float priority, int dataIndex)
        {
            Priority = priority;
            DataIndex = dataIndex;
        }

        public static DodHeapNode New(float priority, int dataIndex)
        {
            return new DodHeapNode(priority, dataIndex);
        }
    }

    public class DodMinHeap<TNode>
    {
        public readonly DodHeapNode[] _elements;
        public TNode[] _data;

        public int _dataIndex;
        public int _size;

        public DodMinHeap(int size)
        {
            _elements = new DodHeapNode[size];
            _data = new TNode[size];
        }

        private int GetLeftChildIndex(int elementIndex) => 2 * elementIndex + 1;
        private int GetRightChildIndex(int elementIndex) => 2 * elementIndex + 2;
        private int GetParentIndex(int elementIndex) => (elementIndex - 1) / 2;

        private bool HasLeftChild(int elementIndex) => GetLeftChildIndex(elementIndex) < _size;
        private bool HasRightChild(int elementIndex) => GetRightChildIndex(elementIndex) < _size;
        private bool IsRoot(int elementIndex) => elementIndex == 0;

        private DodHeapNode GetLeftChild(int elementIndex) => _elements[GetLeftChildIndex(elementIndex)];
        private DodHeapNode GetRightChild(int elementIndex) => _elements[GetRightChildIndex(elementIndex)];
        private DodHeapNode GetParent(int elementIndex) => _elements[GetParentIndex(elementIndex)];

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

        public TNode Peek()
        {
            if (_size == 0)
            {
                throw new IndexOutOfRangeException();
            }
            return _data[_elements[0].DataIndex];
        }

        public TNode Pop()
        {
            if (_size == 0)
                throw new IndexOutOfRangeException();

            var result = _elements[0];
            _elements[0] = _elements[_size - 1];
            _size--;
            ReCalculateDown(0);

            return _data[result.DataIndex];
        }

        public bool Add(float priority, TNode element)
        {
            if (_size == _elements.Length)
                return false;
            _data[_dataIndex] = element;
            _elements[_size] = DodHeapNode.New(priority, _dataIndex);

            ReCalculateUp(_size);
            _size++;
            _dataIndex++;

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
