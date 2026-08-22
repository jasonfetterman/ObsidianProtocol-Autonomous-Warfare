using UnityEngine;

namespace ObsidianProtocol.Game.Resources
{
    [CreateAssetMenu(
        fileName = "ResourceDefinition",
        menuName = "Obsidian Protocol/Resources/Resource Definition")]
    public sealed class ResourceDefinition : ScriptableObject
    {
        [SerializeField] private string resourceId;
        [SerializeField] private string displayName;

        public string ResourceId => resourceId;
        public string DisplayName => displayName;
    }
}
