using System;
using System.Collections.Generic;
using System.Linq;
using InsightsLogger;
using PathfindingAi.Astar;

namespace GoapAstarAi
{
    public class SortedSetFrontier : IFrontier
    {
        private SortedSet<IAstarPathNode> frontier;

        public SortedSetFrontier(ISimpleLogger logger)
        {
            frontier = new SortedSet<IAstarPathNode>();
        }

        public void AddNodeToFrontier(IAstarPathNode node)
        {
            frontier.Add(node);
        }

        public IAstarPathNode GetClosestNode()
        {
            var topNode = frontier.FirstOrDefault();
            frontier.Remove(topNode);

            return topNode;
        }

        public bool IsEmpty()
        {
            return frontier.Count == 0;
        }

        public void Reset()
        {
            frontier.Clear();
        }
    }
}