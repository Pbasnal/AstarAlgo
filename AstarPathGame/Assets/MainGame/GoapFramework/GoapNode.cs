namespace GoapFramework
{
    public class GoapNode
    {
        public GoapNode PreviousNode { get; set; }
        public IAgentAction Action { get; set; }
        public float NodeCost { get; set; }
        public float HeuristicCost { get; set; }
        public IAgentState NodeData { get; set; }
        public bool IsVisited { get; set; }

        public static GoapNode New(IAgentState data, float cost, float heuristicCost, GoapNode prev)
        {
            return new GoapNode
            {
                NodeData = data,
                NodeCost = cost,
                HeuristicCost = heuristicCost,
                PreviousNode = prev
            };
        }
    }
}
