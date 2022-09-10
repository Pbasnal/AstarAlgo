using System;
using System.Collections.Generic;
using InsightsLogger;

namespace PathfindingAi.Astar
{
    public class MinHeapFrontier : IFrontier
    {
        public readonly List<IAstarPathNode> heapNodes;
        public int _size;
        private readonly ISimpleLogger logger;

        public MinHeapFrontier(ISimpleLogger logger)
        {
            heapNodes = new List<IAstarPathNode>();
            this.logger = logger;
        }

        public void Reset()
        {
            heapNodes.Clear();
        }

        public static float GetTotalCostOfNode(IAstarPathNode node) => node.CostOfPathToNode + node.HeuristicCost;
        private static int GetLeftChildIndex(int elementIndex) => 2 * elementIndex + 1;
        private static int GetRightChildIndex(int elementIndex) => 2 * elementIndex + 2;
        private static int GetParentIndex(int elementIndex) => (elementIndex - 1) / 2;
        private static bool IsRoot(int elementIndex) => elementIndex == 0;


        private bool HasLeftChild(int elementIndex) => GetLeftChildIndex(elementIndex) < _size;
        private bool HasRightChild(int elementIndex) => GetRightChildIndex(elementIndex) < _size;

        private IAstarPathNode GetLeftChild(int elementIndex) => heapNodes[GetLeftChildIndex(elementIndex)];
        private IAstarPathNode GetRightChild(int elementIndex) => heapNodes[GetRightChildIndex(elementIndex)];
        private IAstarPathNode GetParent(int elementIndex) => heapNodes[GetParentIndex(elementIndex)];

        public bool IsEmpty() => heapNodes.Count == 0;

        private void Swap(int firstIndex, int secondIndex)
        {
            (heapNodes[secondIndex], heapNodes[firstIndex]) = (heapNodes[firstIndex], heapNodes[secondIndex]);
        }

        public void AddNodeToFrontier(IAstarPathNode node)
        {
            heapNodes.Add(node);
            ReCalculateUp(heapNodes.Count - 1);

            logger.LogDebug($"Frontier add id:{node.StateName} c:{node.CostOfPathToNode} h: {node.HeuristicCost}. #nodes in frontier {heapNodes.Count}");
        }

        public IAstarPathNode GetClosestNode()
        {
            if (IsEmpty())
                throw new IndexOutOfRangeException();

            var result = heapNodes[0];
            heapNodes[0] = heapNodes[^1];
            heapNodes.RemoveAt(heapNodes.Count - 1);

            ReCalculateDown(0);
            return result;
        }

        private void ReCalculateDown(int index)
        {
            while (HasLeftChild(index))
            {
                var smallerIndex = GetLeftChildIndex(index);

                if (HasRightChild(index) && LeftNodeHasHigherCost(index))
                {
                    smallerIndex = GetRightChildIndex(index);
                }

                if (GetTotalCostOfNode(heapNodes[smallerIndex]) >= GetTotalCostOfNode(heapNodes[index]))
                {
                    break;
                }

                Swap(smallerIndex, index);
                index = smallerIndex;
            }
        }

        private void ReCalculateUp(int index)
        {
            while (!IsRoot(index) && HasLesserCostThanParent(index))
            {
                var parentIndex = GetParentIndex(index);
                Swap(parentIndex, index);
                index = parentIndex;
            }
        }

        public bool HasLesserCostThanParent(int index)
        {
            return GetTotalCostOfNode(heapNodes[index]) < GetTotalCostOfNode(GetParent(index));
        }

        public bool LeftNodeHasHigherCost(int parentIndex)
        {
            var leftNodePriority = GetTotalCostOfNode(GetLeftChild(parentIndex));
            var rightNodePriority = GetTotalCostOfNode(GetRightChild(parentIndex));

            return rightNodePriority < leftNodePriority;
        }
    }
}
