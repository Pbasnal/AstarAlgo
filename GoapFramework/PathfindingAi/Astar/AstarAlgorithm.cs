using System.Collections.Generic;
using System.Linq;
using InsightsLogger;

namespace PathfindingAi.Astar
{
    public class AstarAlgorithm
    {
        private readonly ISimpleLogger logger;
        private readonly ICalculateHeuristicCost heuristicCalculator;
        private readonly AstarGraph astarGraph;

        public AstarAlgorithm(
            AstarGraph astarGraph,
            ICalculateHeuristicCost heuristicCalculator,
            ISimpleLogger logger)
        {
            this.astarGraph = astarGraph;
            this.heuristicCalculator = heuristicCalculator;
            this.logger = logger;
        }

        public IList<IWeightedEdge> FindPathBetweenStates(
            IAstarPathNode currentState,
            IAstarPathNode goalState,
            IList<IWeightedEdge> actions)
        {
            astarGraph.AddOrigin(currentState);
            astarGraph.AddDestination(goalState);

            foreach (var action in actions)
            {
                astarGraph.AddEdge(action);
            }

            return FindPathBetweenNodes();
        }

        private IList<IWeightedEdge> FindPathBetweenNodes()
        {
            var originNode = astarGraph.Origin;
            var destinationNode = astarGraph.Destination;
            logger.LogInfo($"Finding path between {destinationNode.StateName} and {originNode.StateName}");

            astarGraph.ResetGraph();
            destinationNode.HeuristicCost = heuristicCalculator.GetHeuristicCostBetween(destinationNode, originNode);
            destinationNode.CostOfPathToNode = 0;
            astarGraph.AddFrontierNode(destinationNode);

            while (astarGraph.HasFrontierNodes())
            {
                var currentNode = astarGraph.GetNextClosestNode();
                if (currentNode.Equals(originNode))
                {
                    return GetPathTo(astarGraph, currentNode);
                }

                logger.LogDebug($"Got next node {currentNode.StateName} with cost: {destinationNode.CostOfPathToNode} | Heuristic: {currentNode.HeuristicCost}");
                foreach (var edge in astarGraph.GetEdgesFromNode(currentNode))
                {
                    var nextNode = (IAstarPathNode)edge.OriginNode;
                    if (nextNode.IsVisited)
                    {
                        continue;
                    }
                    nextNode.EdgeFromPreviousNode = edge;
                    nextNode.PreviousNode = currentNode;

                    nextNode.HeuristicCost = heuristicCalculator.GetHeuristicCostBetween(nextNode, originNode);
                    nextNode.CostOfPathToNode += edge.Cost + currentNode.CostOfPathToNode;
                    
                    nextNode.IsVisited = true;

                    logger.LogDebug($"Edge next node {nextNode.StateName} with cost: {destinationNode.CostOfPathToNode} | Heuristic: {nextNode.HeuristicCost}");

                    astarGraph.AddFrontierNode(nextNode);
                }
            }

            return new List<IWeightedEdge>();
        }


        private IList<IWeightedEdge> GetPathTo(
            AstarGraph astarGraph,
            IAstarPathNode destinationNode)
        {
            logger.LogDebug($"PrevNode of destination: {destinationNode.StateName} prevEdge {destinationNode.EdgeFromPreviousNode}");
            var path = astarGraph.GetCalculatedPathTo(destinationNode);
            path.Reverse();

            return path;
        }
    }
}