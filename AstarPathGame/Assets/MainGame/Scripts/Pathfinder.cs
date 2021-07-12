using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace MainGame
{
    public class Pathfinder : MonoBehaviour
    {
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
        }

        // Update is called once per frame
        void Update()
        {
            if (target == null 
                || Vector3.Distance(target.position, _targetPrevPosition) < 1)
            {
                return;
            }

            var origin = _grid.NodeFromWorldPoint(transform.position);
            var destination = _grid.NodeFromWorldPoint(target.position);

            //Debug.Log($"Finding a path between node ids {origin.Id} and {destination.Id}");
            _path = _astarPathFinder.FindPathBetweenNodes(origin, destination);
            //Debug.Log($"Found a path with {_path.Count} steps");
            _targetPrevPosition = target.position;
            // find new path
        }

        private void OnDrawGizmos()
        {
            if (_path == null || _path.Count == 0)
            {
                return;
            }
            
            var size = new Vector3(0.3f, 1.1f, 0.3f);

            var destination = _grid.NodeFromWorldPoint(target.position);
            Gizmos.color = Color.red;
            Gizmos.DrawCube(destination.worldPosition, size);
            
            Debug.Log("Drawing path");
            foreach (var edge in _path)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(edge.OriginNode.worldPosition, size);
            }
        }
    }
}