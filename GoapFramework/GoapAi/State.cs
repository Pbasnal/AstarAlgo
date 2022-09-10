using System;
using System.Collections.Generic;
using System.Text;

namespace GoapAi
{
    public class State
    {
        public string StateName { get; }

        private readonly IDictionary<string, bool> facts;

        public State(string stateName)
        {
            this.StateName = stateName;
            facts = new Dictionary<string, bool>();
        }

        public override string ToString()
        {
            var str = new StringBuilder();
            foreach (var fact in facts)
            {
                str.Append($"{fact.Key}: {fact.Value}\n");
            }

            return str.ToString();
        }

        public float DistanceFrom(State fromNode)
        {
            var distance = Math.Abs(facts.Count - fromNode.facts.Count);

            foreach (var fact in facts)
            {
                if (fromNode.GetFact(fact.Key) != fact.Value)
                {
                    distance++;
                }
            }

            return distance;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not State state)
            {
                return false;
            }

            return IsSameAs(state);
        }

        public bool IsSameAs(State currentState)
        {
            foreach (var fact in facts)
            {
                if (currentState.GetFact(fact.Key) != fact.Value)
                {
                    return false;
                }
            }

            return true;
        }

        public bool GetFact(string factName)
        {
            if (facts.ContainsKey(factName))
            {
                return facts[factName];
            }
            return false;
        }

        public void SetFact(string factName, bool factValue)
        {
            if (facts.ContainsKey(factName))
            {
                facts[factName] = factValue;
            }
            else
            {
                facts.Add(factName, factValue);
            }
        }

        public void RemoveFact(string factName)
        {
            if (facts.ContainsKey(factName))
            {
                facts.Remove(factName);
            }
        }
    }
}
