using System.Collections.Generic;
using MainGame;
using NUnit.Framework;
using UnityEngine;

namespace AstarGridTests
{
    public class AstarHeapGridTests
    {
        private readonly string UnWalkableLayerName = "UnWalkableTmp";
        // A Test behaves as an ordinary method
        [Test]
        public void All_nodes_in_astardata_have_correct_id()
        {
            var gridSize = new Vector2Int(10, 10);
            var grid = new Node[gridSize.x * gridSize.y];

            var gameObject = new GameObject();
            float nodeRadius = 0.5f;
            LayerMask unwalkableLayers = LayerMask.GetMask(UnWalkableLayerName);

            var graphBuilder = A.GraphBuilder(grid);
            var gridBottomLeft = gameObject.transform.position;

            AstarGrid.CreateGrid(
                gridSize,
                nodeRadius,
                grid,
                graphBuilder,
                gridBottomLeft,
                unwalkableLayers);

            var astarData = graphBuilder.WithHeapOptimization();

            Assert.NotNull(astarData.Nodes);
            Assert.AreEqual(gridSize.x * gridSize.y, grid.Length);

            for (int i = 0; i < astarData.Nodes.Length; i++)
            {
                Assert.AreEqual(i, astarData.Nodes[i].Id);
            }
        }

        [Test]
        public void All_nodes_in_grid_have_correct_id()
        {
            var gridSize = new Vector2Int(10, 10);
            var grid = new Node[gridSize.x * gridSize.y];

            var gameObject = new GameObject();
            float nodeRadius = 0.5f;
            LayerMask unwalkableLayers = LayerMask.GetMask(UnWalkableLayerName);

            var graphBuilder = A.GraphBuilder(grid);
            var gridBottomLeft = gameObject.transform.position;

            AstarGrid.CreateGrid(
                gridSize,
                nodeRadius,
                grid,
                graphBuilder,
                gridBottomLeft,
                unwalkableLayers);

            Assert.NotNull(grid);
            Assert.AreEqual(gridSize.x * gridSize.y, grid.Length);

            HashSet<int> idCollection = new HashSet<int>();

            for (int i = 0; i < grid.Length; i++)
            {
                Assert.IsFalse(idCollection.Contains(grid[i].Id));
                idCollection.Add(grid[i].Id);
            }
        }

        [Test]
        public void All_nodes_should_be_walkable()
        {
            var gridSize = new Vector2Int(10, 10);
            var grid = new Node[gridSize.x * gridSize.y];

            var gameObject = new GameObject();
            float nodeRadius = 0.5f;
            LayerMask unwalkableLayers = LayerMask.GetMask(UnWalkableLayerName);

            var graphBuilder = A.GraphBuilder(grid);
            var gridBottomLeft = gameObject.transform.position;

            AstarGrid.CreateGrid(
                gridSize,
                nodeRadius,
                grid,
                graphBuilder,
                gridBottomLeft,
                unwalkableLayers);

            var astarData = graphBuilder.WithHeapOptimization();

            Assert.NotNull(astarData.Nodes);
            Assert.AreEqual(gridSize.x * gridSize.y, grid.Length);

            for (int i = 0; i < astarData.Nodes.Length; i++)
            {
                Assert.IsTrue(astarData.Nodes[i].isWalkable);
            }
        }
    }
}