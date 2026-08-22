using UnityEngine;

namespace ObsidianProtocol.Game.World.Routes
{
    public sealed class TraversalRoute : MonoBehaviour
    {
        [SerializeField] private TraversalRouteDefinition definition;
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform endPoint;

        public TraversalRouteDefinition Definition => definition;
        public Transform StartPoint => startPoint;
        public Transform EndPoint => endPoint;

        public float Length
        {
            get
            {
                if (startPoint == null || endPoint == null)
                {
                    return 0f;
                }

                return Vector3.Distance(
                    startPoint.position,
                    endPoint.position);
            }
        }
    }
}
