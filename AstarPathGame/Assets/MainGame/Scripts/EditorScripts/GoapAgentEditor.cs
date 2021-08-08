using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using MainGame;
using MainGame.Sensors;

namespace MainGame.GameEditor
{
    [CustomEditor(typeof(GoapAgent))]
    public class GoapAgentEditor : Editor
    {
        private GoapAgent agent;
        private FieldOfView fieldOfView;
        private PatrolBehaviour patrolBehaviour;

        private AstarGrid grid;

        private void OnEnable()
        {
            agent = (GoapAgent)target;
            fieldOfView = agent.GetComponent<FieldOfView>();
            patrolBehaviour = agent.GetComponent<PatrolBehaviour>();
            grid = FindObjectOfType<AstarGrid>();
            if(grid != null) grid.LoadGrid();
        }

        private void OnSceneGUI()
        {
            DrawFieldOfView(fieldOfView);
            DrawPatrolPoints(patrolBehaviour);

            if(agent.drawGrid) AstarGridEditor.DrawGrid(grid);
        }
        
        private void DrawPatrolPoints(PatrolBehaviour patrolBehaviour)
        {
            if (patrolBehaviour == null)
            {
                return;
            }
            patrolBehaviour.LoadWaypoints();

            var currentWaypoint = patrolBehaviour.waypoints[patrolBehaviour.currentWaypoint];
            var agentPosition = agent.transform.position;
            Handles.color = new Color(0, 1f, 1f, 0.5f);
            Handles.DrawSolidDisc(currentWaypoint, new Vector3(0, 1, 0), patrolBehaviour.waypointIconRadius);
            Handles.ArrowHandleCap(0,
                    agentPosition,
                    Quaternion.LookRotation(currentWaypoint - agentPosition),
                    (currentWaypoint - agentPosition).magnitude * 0.7f,
                    EventType.Repaint);

            Handles.color = patrolBehaviour.waypointIconColor;
            var totalWaypoints = patrolBehaviour.waypoints.Length;
            for (int i = 0; i < patrolBehaviour.waypoints.Length; i++)
            {
                Handles.DrawSolidDisc(patrolBehaviour.waypoints[i],
                    new Vector3(0, 1, 0), patrolBehaviour.waypointIconRadius);

                var toWaypointIndex = (i + 1) % totalWaypoints;
                var distanceVector = patrolBehaviour.waypoints[toWaypointIndex] - patrolBehaviour.waypoints[i];
                Handles.ArrowHandleCap(0,
                    patrolBehaviour.waypoints[i],
                    Quaternion.LookRotation(distanceVector),
                    distanceVector.magnitude * 0.7f,
                    EventType.Repaint);
            }
        }

        private void DrawFieldOfView(FieldOfView fieldOfView)
        {
            if (fieldOfView == null)
            {
                return;
            }
            Handles.color = new Color(1, 0, 0, 0.2f);

            DrawFieldOfView(fieldOfView.GetFieldStartDirection(fieldOfView.shortRangeViewAngle),
                fieldOfView.shortRangeViewAngle,
                fieldOfView.shortRangeRadius);

            DrawFieldOfView(fieldOfView.GetFieldStartDirection(fieldOfView.midRangeViewAngle),
                fieldOfView.midRangeViewAngle,
                fieldOfView.midRangeRadius);

            DrawFieldOfView(fieldOfView.GetFieldStartDirection(fieldOfView.longRangeViewAngle),
                fieldOfView.longRangeViewAngle,
                fieldOfView.longRangeRadius);
        }

        private void DrawFieldOfView(Vector3 startDirection, int viewAngle, int radius)
        {
            Handles.DrawSolidArc(agent.transform.position,
                    new Vector3(0, 1, 0),
                    startDirection,
                    viewAngle,
                    radius);
        }
    }
}