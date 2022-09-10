using System.Collections.Generic;

namespace GoapAi
{
    public interface IFindActionPlan
    {
        IList<GoapAction> FindPlanForAgent(IUseGoapPlan agent);
    }
}