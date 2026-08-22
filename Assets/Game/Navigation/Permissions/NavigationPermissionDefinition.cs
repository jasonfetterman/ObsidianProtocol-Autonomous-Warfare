using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Permissions
{
    [CreateAssetMenu(
        fileName = "NavigationPermissionDefinition",
        menuName = "Obsidian Protocol/Navigation/Navigation Permission Definition")]
    public sealed class NavigationPermissionDefinition : ScriptableObject
    {
        [SerializeField] private string permissionId;
        [SerializeField] private string displayName;
        [SerializeField] private bool allowedByDefault = true;

        public string PermissionId => permissionId;
        public string DisplayName => displayName;
        public bool AllowedByDefault => allowedByDefault;
    }
}
