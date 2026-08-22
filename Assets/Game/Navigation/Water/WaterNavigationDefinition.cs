using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Water
{
    [CreateAssetMenu(
        fileName = "WaterNavigationDefinition",
        menuName = "Obsidian Protocol/Navigation/Water Navigation Definition")]
    public sealed class WaterNavigationDefinition : ScriptableObject
    {
        [SerializeField] private float movementSpeed = 8f;
        [SerializeField] private float stoppingDistance = 2f;
        [SerializeField] private float minimumDepth = 0.5f;

        public float MovementSpeed => Mathf.Max(0f, movementSpeed);
        public float StoppingDistance => Mathf.Max(0.1f, stoppingDistance);
        public float MinimumDepth => Mathf.Max(0f, minimumDepth);
    }
}
