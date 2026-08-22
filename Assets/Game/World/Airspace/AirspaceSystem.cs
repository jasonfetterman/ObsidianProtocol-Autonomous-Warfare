using UnityEngine;

namespace ObsidianProtocol.Game.World.Airspace
{
    public sealed class AirspaceSystem : MonoBehaviour
    {
        [SerializeField] private AirspaceDefinition definition;

        public AirspaceDefinition Definition => definition;

        public bool IsAltitudeAllowed(float altitude)
        {
            if (definition == null)
            {
                return true;
            }

            return altitude >= definition.MinimumAltitude &&
                   altitude <= definition.MaximumAltitude;
        }

        public float ClampAltitude(float altitude)
        {
            if (definition == null)
            {
                return altitude;
            }

            return Mathf.Clamp(
                altitude,
                definition.MinimumAltitude,
                definition.MaximumAltitude);
        }
    }
}
