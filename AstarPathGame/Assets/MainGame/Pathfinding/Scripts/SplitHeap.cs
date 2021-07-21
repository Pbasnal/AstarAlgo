using System.Threading;

namespace Pathfinding
{
    public class SplitHeap
    {
        public int _numberOfSplits;

        public SimpleMinHeap<SimpleHeapNode>[] _heaps;

        public float avgTimeToEnqueue;
        public float avgWaitTime;
        public float avgPopTime;
        public float avgInitTime;

        public SplitHeap(int size, int numberOfSplits)
        {
            _numberOfSplits = numberOfSplits;
            _heaps = new SimpleMinHeap<SimpleHeapNode>[numberOfSplits];

            for (int i = 0; i < numberOfSplits; i++)
            {
                _heaps[i] = new SimpleMinHeap<SimpleHeapNode>(size / numberOfSplits + 1);
            }
        }

        public bool IsEmpty()
        {
            return _heaps[0].IsEmpty();
            //for (int i = 0; i < _numberOfSplits; i++)
            //{
            //    if (!_heaps[i].IsEmpty()) return false;
            //}

            //return true;
        }

        //public SimpleHeapNode Peek()
        //{
        //    return 
        //}

        public SimpleHeapNode Pop()
        {
            for (int i = _numberOfSplits - 1; i >= 0; i--)
            {
                if (_heaps[i].IsEmpty()) continue;
                return _heaps[i].Pop();
            }

            return null;
        }

        public void Add(SimpleHeapNode element)
        {
            for (int i = 0; i < _numberOfSplits; i++)
            {
                if (_heaps[i].Add(element)) return;
                if (element.Priority < _heaps[i].Peek().Priority) continue;

                var elem = _heaps[i].Pop();
                _heaps[i].Add(element);
                element = elem;
            }
        }
    }
}
