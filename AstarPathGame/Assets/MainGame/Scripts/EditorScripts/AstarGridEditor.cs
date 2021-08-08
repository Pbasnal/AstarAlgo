using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using MainGame;

namespace MainGame.GameEditor
{
    [CustomEditor(typeof(AstarGrid))]
    public class AstarGridEditor : Editor
    {
        AstarGrid grid;
        private float oldNodeRadius;
        private Vector2Int oldGridSize;
        private static Color walkableColor = new Color(1, 1, 1, 0.2f);
        private static Color unwalkableColor = new Color(1, 0, 0, 0.2f);

        private void OnEnable()
        {
            grid = FindObjectOfType<AstarGrid>();
            if (grid != null) grid.LoadGrid();
        }

        private void OnSceneGUI()
        {
            if (GridWasUpdated(grid)) grid.LoadGrid();
            DrawGrid(grid);
        }

        private bool GridWasUpdated(AstarGrid grid)
        {
            if (grid.nodeRadius != oldNodeRadius)
            {
                oldNodeRadius = grid.nodeRadius;
                return true;
            }
            if (grid.gridSize != oldGridSize)
            {
                oldGridSize = grid.gridSize;
                return true;
            }

            return false;
        }

        public static void DrawGrid(AstarGrid grid)
        {
            if (grid == null) return;

            var nodes = grid.GetGrid();

            Handles.color = walkableColor;
            for (int i = 0; i < nodes.Length; i++)
            {
                if (!nodes[i].isWalkable) continue;
                Handles.DrawWireCube(nodes[i].worldPosition,
                        Vector3.one * (grid.nodeDiameter - 0.1f));
            }
        }
    }
}