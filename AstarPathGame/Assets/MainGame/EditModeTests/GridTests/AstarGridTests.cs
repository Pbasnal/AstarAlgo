using System.Collections.Generic;
using MainGame;
using NUnit.Framework;
using UnityEngine;

namespace AstarGridTests
{
    public class AstarGridTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void All_nodes_in_astardata_have_correct_id()
        {
            var gridSize = new Vector2Int(10, 10);
            var grid = new Node[gridSize.x * gridSize.y];

            var gameObject = new GameObject();
            float nodeRadius = 0.5f;
            LayerMask unwalkableLayers = LayerMask.GetMask("UnWalkable");

            var graphBuilder = A.GraphBuilder(grid);
            var gridBottomLeft = gameObject.transform.position;

            AstarGrid.CreateGrid(
                gridSize,
                nodeRadius,
                grid,
                graphBuilder,
                gridBottomLeft,
                unwalkableLayers);

            var astarData = graphBuilder.WithoutHeap();

            Assert.NotNull(astarData.Nodes);
            Assert.AreNotEqual(0, astarData.Nodes.Length);

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
            LayerMask unwalkableLayers = LayerMask.GetMask("UnWalkable");

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
            Assert.AreNotEqual(0, grid.Length);

            HashSet<int> idCollection = new HashSet<int>();

            for (int i = 0; i < grid.Length; i++)
            {
                Assert.IsFalse(idCollection.Contains(grid[i].Id));
                idCollection.Add(grid[i].Id);
            }
        }

        //    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        //    // `yield return null;` to skip a frame.
        //    [UnityTest]
        //    public IEnumerator PathfinderTestsWithEnumeratorPasses()
        //    {
        //        // Use the Assert class to test conditions.
        //        // Use yield to skip a frame.
        //        yield return null;
        //    }
        //}
    }
}