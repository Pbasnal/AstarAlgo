using UnityEngine;

namespace MainGame
{
    public class PatrolBehaviour : MonoBehaviour
    {
        public Transform waypointHolder;
        public float walkingSpeed = 1.0f;

        [HideInInspector] public Vector3[] waypoints;
        [HideInInspector] public int currentWaypoint = 0;

        [Space]
        [Header("Debug")]
        public int waypointIconRadius;
        public Color waypointIconColor;

        private void Start()
        {
            if(waypointHolder == null) return;
            LoadWaypoints();
        }

        public void LoadWaypoints()
        {
            waypoints = new Vector3[waypointHolder.childCount];
            for (int i = 0; i < waypointHolder.childCount; i++)
            {
                waypoints[i] = waypointHolder.GetChild(i).position;
            }
        }

        public void Behave()
        {
            // float * float * Vector3 performance better than 
            // Vector3 * float * float
            var distanceToMove = Time.deltaTime * walkingSpeed;
            var directionToMove = (waypoints[currentWaypoint] - transform.position).normalized;
            directionToMove.y = 0;
            var position = transform.position + distanceToMove * directionToMove;

            if (Vector3.Distance(position, waypoints[currentWaypoint]) < 1.5f)
            {
                currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            }

            var rotation = Quaternion.LookRotation(directionToMove);
            transform.SetPositionAndRotation(position, rotation);
        }
    }
}