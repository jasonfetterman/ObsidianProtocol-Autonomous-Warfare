using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.RetreatRouting
{
    public sealed class RetreatRoutingService : MonoBehaviour
    {
        [SerializeField] private float retreatDistance = 100f;
        [SerializeField] private float minimumRetreatDistance = 25f;

        public float RetreatDistance =>
            Mathf.Max(1f, retreatDistance);

        public float MinimumRetreatDistance =>
            Mathf.Max(1f, minimumRetreatDistance);

        public Vector3 GetRetreatDestination(
            Vector3 currentPosition,
            Vector3 threatPosition)
        {
            Vector3 direction =
                currentPosition - threatPosition;

            direction.y = 0f;

            if (direction.sqrMagnitude <= 0.0001f)
            {
                direction = Vector3.back;
            }

            direction.Normalize();

            return currentPosition +
                   direction *
                   Mathf.Max(
                       RetreatDistance,
                       MinimumRetreatDistance);
        }

        public Vector3 GetRetreatDestination(
            Vector3 currentPosition,
            Vector3 threatPosition,
            Vector3 fallbackDirection)
        {
            Vector3 direction =
                currentPosition - threatPosition;

            direction.y = 0f;

            if (direction.sqrMagnitude <= 0.0001f)
            {
                fallbackDirection.y = 0f;

                direction =
                    fallbackDirection.sqrMagnitude >
                    0.0001f
                        ? fallbackDirection
                        : Vector3.back;
            }

            direction.Normalize();

            return currentPosition +
                   direction *
                   Mathf.Max(
                       RetreatDistance,
                       MinimumRetreatDistance);
        }

        public List<Vector3> BuildRetreatRoute(
            Vector3 currentPosition,
            Vector3 threatPosition,
            Vector3 fallbackDirection)
        {
            var route = new List<Vector3>();

            Vector3 destination =
                GetRetreatDestination(
                    currentPosition,
                    threatPosition,
                    fallbackDirection);

            route.Add(currentPosition);
            route.Add(destination);

            return route;
        }
    }
}
