using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace MainGame
{
    public class AstarGrid : MonoBehaviour
    {
        public LayerMask unwalkableLayers;
        public Vector2Int gridSize;
        public float nodeRadius;
        public bool drawGrid;

        public Transform player;
        public Transform target;

        public Vector3 GridBottomLeft;
        private Node[] _grid;
        private float _nodeDiameter => nodeRadius * 2;

        public IAstarData<Node, WeightedEdge> GridGraph { get; private set; }

        private void Start()
        {
            _grid = new Node[gridSize.x * gridSize.y];            
            GridBottomLeft = transform.position;

            var graphBuilder = A.GraphBuilder(_grid);
            _grid = CreateGrid(
                gridSize, 
                nodeRadius, 
                _grid, 
                graphBuilder, 
                GridBottomLeft,
                unwalkableLayers);
            GridGraph = graphBuilder.WithoutHeap();
        }

        public static Node[] CreateGrid(
            Vector2Int gridSize, 
            float nodeRadius,
            Node[] gridToFill,
            GraphBuilder graphBuilder,
            Vector3 GridBottomLeft,
            LayerMask unwalkableLayers)
        {
            GraphBuilder AddNeighbourNode(int originNodeIndex, int x, int y, double weight) => CreateNeighborNode(
                    graphBuilder,
                    GridBottomLeft,
                    gridToFill,
                    gridSize,
                    nodeRadius,
                    originNodeIndex,
                    x, y, weight,
                    unwalkableLayers);

            for (int x = 0; x < gridSize.x; x += 1)
            {
                for (int y = 0; y < gridSize.y; y += 1)
                {
                    var nodePosition = GetNodePosition(GridBottomLeft, nodeRadius, x, y);
                    var originNodeIndex = x * gridSize.x + y;
                    CreateNewNode(
                        ref gridToFill[originNodeIndex],
                        ref nodePosition,
                        nodeRadius,
                        unwalkableLayers);
                    gridToFill[originNodeIndex].Id = originNodeIndex;

                    graphBuilder = AddNeighbourNode(originNodeIndex, x, y + 1, 10);
                    graphBuilder = AddNeighbourNode(originNodeIndex, x, y - 1, 10);
                    graphBuilder = AddNeighbourNode(originNodeIndex, x + 1, y, 10);
                    graphBuilder = AddNeighbourNode(originNodeIndex, x - 1, y, 10);

                    graphBuilder = AddNeighbourNode(originNodeIndex, x + 1, y + 1, 10);
                    graphBuilder = AddNeighbourNode(originNodeIndex, x + 1, y - 1, 10);
                    graphBuilder = AddNeighbourNode(originNodeIndex, x - 1, y + 1, 10);
                    graphBuilder = AddNeighbourNode(originNodeIndex, x - 1, y - 1, 10);
                }
            }

            return gridToFill;
        }

        private static GraphBuilder CreateNeighborNode(
            GraphBuilder graphBuilder, 
            Vector3 GridBottomLeft,
            Node[] grid,
            Vector2Int gridSize,
            float nodeRadius,
            int fromNodeIndex,
            int x, int y, double weight,
            LayerMask unwalkableLayers)
        {
            if (x >= gridSize.x || y >= gridSize.y
                || x < 0 || y < 0)
            {
                return graphBuilder;
            }
            var neighborPosition = GetNodePosition(
                GridBottomLeft,
                nodeRadius, x, y);
            
            var toNodeIndex = x * gridSize.x + y;            
            CreateNewNode(
                ref grid[toNodeIndex],
                ref neighborPosition, 
                nodeRadius, 
                unwalkableLayers);
            grid[toNodeIndex].Id = toNodeIndex;

            return graphBuilder
                .WithEdge(fromNodeIndex, toNodeIndex, weight);
        }

        private static void CreateNewNode(
            ref Node newNodeToCreate,
            ref Vector3 position,
            float nodeRadius, 
            LayerMask unwalkableLayers)
        {
            bool isWalkable = !Physics.CheckSphere(
                position, 
                nodeRadius, 
                unwalkableLayers);

            newNodeToCreate = new Node(isWalkable, position);
        }

        private static Vector3 GetNodePosition(Vector3 GridBottomLeft, float nodeRadius, int x, int y)
        {
            return GridBottomLeft
                + Vector3.right * (x * nodeRadius * 2 + nodeRadius)
                + Vector3.forward * (y * nodeRadius  * 2 + nodeRadius);
        }

        public Node NodeFromWorldPoint(Vector3 worldPosition)
        {
            int x, y;
            if (worldPosition.x < transform.position.x + 1)
            {
                x = 0;
            }
            else if (worldPosition.x > transform.position.x + gridSize.x - 1) // since every box is 1 unit
            {
                x = gridSize.x - 1;
            }
            else
            {
                var percentX = (worldPosition.x - transform.position.x - nodeRadius);
                x = Mathf.RoundToInt(percentX);
            }

            if (worldPosition.z < transform.position.z + 1)
            {
                y = 0;
            }
            else if (worldPosition.z > transform.position.z + gridSize.y - 1)
            {
                y = gridSize.x - 1;
            }
            else
            {
                var percentY = (worldPosition.z - transform.position.z - nodeRadius);
                y = Mathf.RoundToInt(percentY);
            }

            return _grid[x * gridSize.x + y];
        }

        public void OnDrawGizmos()
        {
            if (!drawGrid)
            {
                return;
            }
            var gridCenter = transform.position + Vector3.right * gridSize.x / 2 + Vector3.forward * gridSize.y / 2;
            Gizmos.DrawWireCube(gridCenter, new Vector3(gridSize.x, 1, gridSize.y));

            if (_grid == null) return;
            DrawGrid();

            var size = new Vector3(0.3f, 1.1f, 0.3f);

            if (player == null) return;
            MarkPlayerOnGrid(size);

            if (target == null) return;
            MarkPlayerTargetOnGrid(size);
        }

        private void MarkPlayerTargetOnGrid(Vector3 size)
        {
            var destination = NodeFromWorldPoint(target.position);
            Gizmos.color = Color.red;
            Gizmos.DrawCube(destination.worldPosition, size);
        }

        private void MarkPlayerOnGrid(Vector3 size)
        {
            var playerNode = NodeFromWorldPoint(player.position);
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(playerNode.worldPosition, Vector3.one * (_nodeDiameter - 0.3f));
            Gizmos.DrawCube(player.position, size);
        }

        private void DrawGrid()
        {
            foreach (var node in _grid)
            {
                Gizmos.color = node.isWalkable ? Color.white : Color.red;
                if (node.isWalkable)
                {
                    Gizmos.DrawWireCube(node.worldPosition, Vector3.one * (_nodeDiameter - 0.1f));
                }
                else
                {
                    Gizmos.DrawCube(node.worldPosition, Vector3.one * (_nodeDiameter - 0.1f));
                }
            }

            //if (GridGraph == null || GridGraph.Edges == null) return;

            //Gizmos.color = Color.black;
            //foreach (var edge in GridGraph.Edges)
            //{
            //    Gizmos.DrawLine(edge.OriginNode.worldPosition, edge.DestinationNode.worldPosition);
            //}
        }
    }
}