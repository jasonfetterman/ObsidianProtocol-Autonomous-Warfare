using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Permissions
{
    public sealed class NavigationPermission : MonoBehaviour
    {
        [SerializeField] private NavigationPermissionDefinition definition;

        public NavigationPermissionDefinition Definition => definition;

        public bool IsAllowed =>
            definition == null || definition.AllowedByDefault;
    }
}
