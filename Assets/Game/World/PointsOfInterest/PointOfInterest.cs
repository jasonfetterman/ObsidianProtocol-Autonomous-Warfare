using UnityEngine;

namespace ObsidianProtocol.Game.World.PointsOfInterest
{
    public sealed class PointOfInterest : MonoBehaviour
    {
        [SerializeField] private PointOfInterestDefinition definition;

        public PointOfInterestDefinition Definition => definition;
        public string PointId =>
            definition != null ? definition.PointId : string.Empty;

        public Vector3 Position => transform.position;
    }
}
