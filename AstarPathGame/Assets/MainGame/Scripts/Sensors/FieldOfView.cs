using UnityEngine;
using MainGame;
using GoapFramework;

namespace MainGame.Sensors
{
    [RequireComponent(typeof(AgentMemory))]
    public class FieldOfView : MonoBehaviour
    {
        public LayerMask visibleLayers;

        [Header("Short Range")]
        public int shortRangeRadius = 10;
        public int shortRangeViewAngle = 120;

        [Space]
        [Header("Mid Range")]
        public int midRangeRadius = 10;
        public int midRangeViewAngle = 120;

        [Space]
        [Header("Long Range")]
        public int longRangeRadius = 10;
        public int longRangeViewAngle = 120;

        private GoapAgent agent;

        private void Start()
        {
            agent = GetComponent<GoapAgent>();
        }
        private void Update()
        {
            var colliders = Physics.OverlapSphere(transform.position, midRangeRadius, visibleLayers);
            if (colliders == null || colliders.Length == 0) return;

            for (int i = 0; i < colliders.Length; i++)
            {
                if (!colliders[i].TryGetComponent<IInteractable>(out var interactable)) continue;

                agent.UpdateMemory(interactable);
            }
        }

        public Vector3 GetFieldStartDirection(int viewAngle)
        {
            if (viewAngle >= 360)
            {
                return -transform.forward;
            }

            var viewAngleHalf = -viewAngle / 2;
            return Quaternion.Euler(0, viewAngleHalf, 0) * transform.forward;
        }
    }
}