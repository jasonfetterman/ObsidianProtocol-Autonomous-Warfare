using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Commands
{
    [CreateAssetMenu(
        fileName = "MovementCommandDefinition",
        menuName = "Obsidian Protocol/Navigation/Movement Command Definition")]
    public sealed class MovementCommandDefinition : ScriptableObject
    {
        [SerializeField] private float acceptanceRadius = 1f;

        public float AcceptanceRadius =>
            Mathf.Max(0.1f, acceptanceRadius);
    }
}
