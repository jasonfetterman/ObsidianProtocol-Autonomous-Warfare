using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.ReinforcementRouting
{
    public sealed class ReinforcementRoutingService : MonoBehaviour
    {
        [SerializeField] private float arrivalDistance = 5f;
        [SerializeField] private float stagingDistance = 15f;

        public float ArrivalDistance =>
            Mathf.Max(0.1f, arrivalDistance);

        public float StagingDistance =>
            Mathf.Max(0.1f, stagingDistance);

        public Vector3 GetReinforcementDestination(
            Vector3 reinforcementPosition,
            Vector3 supportedPosition)
        {
            return supportedPosition;
        }

        public Vector3 GetStagingPosition(
            Vector3 reinforcementPosition,
            Vector3 supportedPosition)
        {
            Vector3 direction =
                reinforcementPosition -
                supportedPosition;

            direction.y = 0f;

            if (direction.sqrMagnitude <= 0.0001f)
            {
                direction = Vector3.back;
            }

            direction.Normalize();

            return supportedPosition +
                   direction *
                   StagingDistance;
        }

        public bool HasArrived(
            Vector3 reinforcementPosition,
            Vector3 supportedPosition)
        {
            return Vector3.Distance(
                reinforcementPosition,
                supportedPosition) <=
                ArrivalDistance;
        }

        public List<Vector3> BuildReinforcementRoute(
            Vector3 reinforcementPosition,
            Vector3 supportedPosition)
        {
            var route = new List<Vector3>();

            Vector3 stagingPosition =
                GetStagingPosition(
                    reinforcementPosition,
                    supportedPosition);

            route.Add(reinforcementPosition);
            route.Add(stagingPosition);
            route.Add(
                GetReinforcementDestination(
                    reinforcementPosition,
                    supportedPosition));

            return route;
        }
    }
}
