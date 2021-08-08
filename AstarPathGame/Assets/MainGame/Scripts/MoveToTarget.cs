using UnityEngine;

namespace MainGame
{
    public class MoveToTarget : PatrolBehaviour
    {
        private void Start() 
        {
            waypoints = new Vector3[1];
        }

        public void Move(Vector3 target)
        {
            waypoints[0] = target;
            currentWaypoint = 0;
            
            Behave();
        }
    }
}