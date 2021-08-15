using System;
using System.Collections.Generic;
using BasnalGames.InsightsLogger;
using UnityEditor;
using UnityEngine;

namespace MainGame
{
    public class Pathfinder : MonoBehaviour
    {
        public bool debugPath = false;
        public bool debugPathFindingProcess = false;
        public Vector3 debugTextOffset;
        private string _correlationVectorBase = "Pathfinder";

        [Space]
        [Tooltip("Assign grid explicitely if multiple grids are present")]
        public AstarGrid grid;
        private AstarGrid _grid;

        public Transform target;
        private Vector3 _targetPrevPosition;

        private AstarPathfinder _astarPathFinder;
        private List<WeightedEdge> _path;

        private void Awake()
        {
            _grid = grid == null ? _grid = FindObjectOfType<AstarGrid>() : grid;
        }

        // Start is called before the first frame update
        void Start()
        {
            _astarPathFinder = new AstarPathfinder(_grid.GridGraph);
            _targetPrevPosition = transform.position;

            RuntimeLogger.AddDebugListener(DebugMessageListener);
        }

        private void OnDestroy()
        {
            RuntimeLogger.RemoveDebugListener(DebugMessageListener);
        }

        // Update is called once per frame
        void Update()
        {
            if (target == null) return;

            CorrelationVector.Reset($"{_correlationVectorBase}{Time.time}");

            currentOriginNode = _grid.NodeFromWorldPoint(transform.position);
            currentDestinationNode = _grid.NodeFromWorldPoint(target.position);

            if (!currentDestinationNode.isWalkable) return;

            var pathTime = ExecutionTimer.Time(() =>
                _path = _astarPathFinder
                    .FindPathBetweenNodes(currentOriginNode, currentDestinationNode));
        }

        private void ResetDebugInformation()
        {
            _edges.Clear();
        }

        public Node currentOriginNode;
        public Node currentDestinationNode;
        private Dictionary<int, List<int>> _edges = new Dictionary<int, List<int>>();
        private void DebugMessageListener(string eventName, string eventMessage, string payload)
        {
            if (eventName != "Pathfinding") return;

            if (eventMessage == "Running")
            {
                ResetDebugInformation();
                currentOriginNode = JsonUtility.FromJson<Node>(payload);
            }
            if (eventMessage == "AddedFrontier")
            {
                var edgeNodeIds = JsonUtility.FromJson<ListWrapper<int>>(payload)?.collection;
                if (edgeNodeIds == null) return;

                if (!_edges.ContainsKey(edgeNodeIds[0]))
                {
                    _edges.Add(edgeNodeIds[0], new List<int>());
                }
                _edges[edgeNodeIds[0]].Add(edgeNodeIds[1]);
            }
        }

        private void OnDrawGizmos()
        {
            if (!RuntimeLogger.DebuggerIsOn || !Application.isPlaying) return;

            var size = new Vector3(0.3f, 1.1f, 0.3f);
            Gizmos.color = Color.red;
            Gizmos.DrawCube(currentDestinationNode.worldPosition, size);

            Gizmos.color = Color.black;
            Gizmos.DrawCube(currentOriginNode.worldPosition, size);

            DrawNodesWhichWereProcessed();
            DrawPath(size);

            if (Application.isPlaying && RuntimeLogger.BreakPerFrame) Debug.Break();
        }

        private void DrawPath(Vector3 size)
        {
            if (_path == null || !debugPath) return;

            foreach (var edge in _path)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(edge.OriginNode.worldPosition, size);
            }
        }

        private void DrawNodesWhichWereProcessed()
        {
            if (!debugPathFindingProcess
                || _edges == null
                || _edges.Count == 0) return;

            foreach (var edge in _edges)
            {
                var fromNode = _grid.GridGraph.Nodes[edge.Key];
                Gizmos.DrawWireSphere(fromNode.worldPosition, 0.3f);

                foreach (var destNodeId in edge.Value)
                {
                    var toNode = _grid.GridGraph.Nodes[destNodeId];
                    Gizmos.DrawLine(fromNode.worldPosition, toNode.worldPosition);

                    var nodeCost = Math.Round(toNode.NodeCost, 2);
                    var heuristicCost = Math.Round(toNode.HeuristicCost, 2);
                    var totalCost = nodeCost + heuristicCost;
                    Handles.Label(toNode.worldPosition - debugTextOffset,
                        $"{toNode.Id}\n{nodeCost}+{heuristicCost}\n={totalCost}\n <{toNode.PreviousNode}>");
                }
            }
        }
    }
}
