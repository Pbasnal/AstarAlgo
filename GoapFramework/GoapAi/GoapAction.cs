using System.Diagnostics.Contracts;

namespace GoapAi
{
    public class GoapAction 
    {
        public string ActionName { get; set; }
        public State PreConditions { get; set; }
        public State Effects { get; set; }

        public float Cost { get; set; }

        public GoapAction(string actionName)
        {
            this.ActionName = actionName;
            PreConditions = new State($"{actionName}-precondition");
            Effects = new State($"{actionName}-effects");
        }

        // Is this edge connected to origin state
        public bool IsActionPossible(State currentState) => PreConditions.IsSameAs(currentState);

        // Can this action lead to state
        public bool CanReachStateWithAction(State resultingState) => Effects.IsSameAs(resultingState);

        public State PerformAction() => Effects;

        public override string ToString()
        {
            return $"{ActionName} > o:{PreConditions.StateName} d:{Effects.StateName}";
            // return ActionName + "\n" + PreConditions.ToString() + "\n" + Effects.ToString();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not GoapAction)
            {
                return false;
            }

            var action = obj as GoapAction;

            return PreConditions.Equals(action.PreConditions) && Effects.Equals(action.Effects);
        }
    }
}