using System.Collections.Generic;

namespace AstarAlgo
{
    // Adjaceny list implementation
    public abstract class AstarPath<TNode, TEdge>
        where TNode : INode, new()
        where TEdge : IWeightedEdge, new()
    {
        private IAstarData<TNode, TEdge> _astarData;

        public AstarPath(IAstarData<TNode, TEdge> astarData)
        {
            _astarData = astarData;
        }

        public List<TEdge> FindPathBetween(int originNodeIndex, int destinationNodeIndex)
        {
            var nodeToProcess = _astarData.Nodes[originNodeIndex];
            var destinationNode = _astarData.Nodes[destinationNodeIndex];

            _astarData.ResetForNewOriginNode(originNodeIndex);

            _astarData.AddAFrontierNode(nodeToProcess, nodeToProcess, 0, 0);
            while (_astarData.TryGetNodeWithMinimumCost(out nodeToProcess))
            {
                if (nodeToProcess.Id == destinationNode.Id)
                {
                    // why to move anywhere else from here.
                    continue;
                }
                var edgesOriginatingFromNode = _astarData.GetEdgesOriginatingFromNode(nodeToProcess);
                foreach (var edge in edgesOriginatingFromNode)
                {
                    var newFrontierNode = _astarData.Nodes[edge.DestinationNode.Id];
                    var costToNode = (_astarData.GetNodeCostOf(nodeToProcess.Id) + edge.Weight) +
                        HeuristicCost(newFrontierNode, destinationNode);

                    _astarData.AddAFrontierNode(
                        newFrontierNode,
                        nodeToProcess,
                        edge.Weight,
                        costToNode);
                }
                _astarData.SetNodeVisited(nodeToProcess.Id);
            }

            return _astarData.GetPathTo(destinationNode.Id);
        }

        public abstract double HeuristicCost(TNode fromNode, TNode toNode);
    }
}
