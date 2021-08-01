using System;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace MainGame
{
    [Serializable]
    public class StateElement
    {
        public AgentStateKey stateName;

        public int value;

        public StateElement() { }
        public StateElement(AgentStateKey stateName, int value)
        {
            this.stateName = stateName;
            this.value = value;
        }
    }

    [Serializable]
    public class State
    {
        [SerializeField]
        private List<StateElement> stateData;

        private bool _indexHasBeenInitialized => _stateIndex.Count == stateData.Count;

        private Dictionary<AgentStateKey, int> _stateIndex;

        public State()
        {
            stateData = new List<StateElement>();
            _stateIndex = new Dictionary<AgentStateKey, int>();
        }

        public void InitializeIndex()
        {
            for (int i = 0; i < stateData.Count; i++)
            {
                _stateIndex.Add(stateData[i].stateName, i);
            }
        }

        public bool ContainsKey(AgentStateKey key)
        {
            if (!_indexHasBeenInitialized) InitializeIndex();
            return _stateIndex.ContainsKey(key);
        }

        public int Get(AgentStateKey stateKey)
        {
            if (!_indexHasBeenInitialized) InitializeIndex();

            if (!_stateIndex.ContainsKey(stateKey)) return 0;
            return stateData[_stateIndex[stateKey]].value;
        }

        public bool Set(AgentStateKey stateKey, int value)
        {
            if (!_indexHasBeenInitialized) InitializeIndex();

            if (_stateIndex.ContainsKey(stateKey))
            {
                if (stateData[_stateIndex[stateKey]].value == value) return false;

                stateData[_stateIndex[stateKey]].value = value;
                return true;
            }

            //* Add the new state key
            _stateIndex.Add(stateKey, stateData.Count);
            stateData.Add(new StateElement(stateKey, value));
            return true;
            // for (int i = 0; i < stateData.Count; i++)
            // {
            //     if (stateData[i].stateName != stateKey) continue;

            //     _stateIndex.Add (stateKey, i);
            //     stateData[i].value = value;

            //     //* return after setting state key
            //     return;
            // }

        }

        public IEnumerable<StateElement> GetStateElements()
        {
            foreach (var state in stateData)
            {
                yield return state;
            }
        }
    }

    [
        CreateAssetMenu(
            fileName = "AgentState",
            menuName = "Goap/AgentState",
            order = 52)
    ]
    public class AgentState : ScriptableObject, INodeWithPriority
    {
        [SerializeField]
        private State state;
        public static int StateSize = Enum.GetValues(typeof(AgentStateKey)).Length;

        public string NodeId => string.Empty;

        public bool IsVisited { get; set; }

        public int Id { get; set; }

        public IAgentAction ActionForThisNode { get; set; }

        public float NodeCost { get; set; }

        public float HeuristicCost { get; set; }

        public float Priority { get; set; }

        public int PreviousNode { get; set; }

        public static AgentState New()
        {
            var state = ScriptableObject.CreateInstance<AgentState>();
            state.Id = -1;
            state.state = new State();

            return state;
        }

        public AgentState Clone()
        {
            var newState = AgentState.New();
            foreach (var stateElement in state.GetStateElements())
            {
                newState.Set(stateElement.stateName, stateElement.value);
            }

            return newState;
        }

        public bool ContainsKey(AgentStateKey key) => state.ContainsKey(key);

        public int Get(AgentStateKey stateKey) => state.Get(stateKey);

        public bool Set(AgentStateKey stateKey, int value) =>
            state.Set(stateKey, value);

        public IEnumerable<StateElement> GetStateElements() =>
            state.GetStateElements();
    }

    public enum AgentStateKey
    {
        TargetInSight = 0,
        AgentOutOfSight,
        TargetInRange,
        CanWalk,
        EnemyIsDead
    }
}
