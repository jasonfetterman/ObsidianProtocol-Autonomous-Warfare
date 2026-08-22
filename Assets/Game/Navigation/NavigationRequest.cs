using UnityEngine;

namespace ObsidianProtocol.Game.Navigation
{
    public readonly struct NavigationRequest
    {
        public Vector3 Destination { get; }
        public float AcceptanceRadius { get; }

        public NavigationRequest(Vector3 destination, float acceptanceRadius = 1f)
        {
            Destination = destination;
            AcceptanceRadius = Mathf.Max(0.1f, acceptanceRadius);
        }
    }
}
