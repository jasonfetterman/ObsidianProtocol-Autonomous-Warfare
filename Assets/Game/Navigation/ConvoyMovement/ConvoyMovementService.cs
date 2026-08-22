using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.ConvoyMovement
{
    public sealed class ConvoyMovementService : MonoBehaviour
    {
        [SerializeField] private float vehicleSpacing = 8f;
        [SerializeField] private float lateralSpacing = 4f;

        public float VehicleSpacing => Mathf.Max(0.1f, vehicleSpacing);
        public float LateralSpacing => Mathf.Max(0.1f, lateralSpacing);

        public List<Vector3> BuildConvoyPositions(
            Vector3 leaderPosition,
            Vector3 forward,
            int vehicleCount)
        {
            var positions = new List<Vector3>();

            if (vehicleCount <= 0)
            {
                return positions;
            }

            forward.y = 0f;

            if (forward.sqrMagnitude <= 0.0001f)
            {
                forward = Vector3.forward;
            }

            forward.Normalize();

            Vector3 right =
                Vector3.Cross(Vector3.up, forward).normalized;

            for (int i = 0; i < vehicleCount; i++)
            {
                int row = i / 2;
                int side = i % 2 == 0 ? -1 : 1;

                Vector3 position =
                    leaderPosition
                    - forward * ((row + 1) * VehicleSpacing)
                    + right * (side * LateralSpacing);

                positions.Add(position);
            }

            return positions;
        }
    }
}
