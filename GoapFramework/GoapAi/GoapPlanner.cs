using System.Collections.Generic;
using InsightsLogger;

namespace GoapAi
{
    public class GoapPlanner
    {
        private readonly IFindActionPlan planFinder;
        private readonly ISimpleLogger logger;


        public GoapPlanner(
            IFindActionPlan planFinder,
            ISimpleLogger logger)
        {
            this.planFinder = planFinder;
            this.logger = logger;
        }

        public IList<GoapAction> FindPlanForAgent(IUseGoapPlan agent) 
        {
            return planFinder.FindPlanForAgent(agent);
        }
    }
}