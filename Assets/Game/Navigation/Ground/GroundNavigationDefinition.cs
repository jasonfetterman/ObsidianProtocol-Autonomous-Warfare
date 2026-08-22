using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Ground
{
    [CreateAssetMenu(
        fileName = "GroundNavigationDefinition",
        menuName = "Obsidian Protocol/Navigation/Ground Navigation Definition")]
    public sealed class GroundNavigationDefinition : ScriptableObject
    {
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float stoppingDistance = 1f;

        public float MovementSpeed => Mathf.Max(0f, movementSpeed);
        public float StoppingDistance => Mathf.Max(0.1f, stoppingDistance);
    }
}
