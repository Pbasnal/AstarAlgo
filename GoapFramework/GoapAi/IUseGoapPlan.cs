using System.Collections;
using System.Collections.Generic;

namespace GoapAi
{
    public interface IUseGoapPlan
    {
        State GetCurrentState();
        State GetGoalStateToPlanFor();
        IList<GoapAction> GetAgentActions();
    }
}