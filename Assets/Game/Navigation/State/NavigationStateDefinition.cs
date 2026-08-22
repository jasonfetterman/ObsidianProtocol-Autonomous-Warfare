using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.State
{
    [CreateAssetMenu(
        fileName = "NavigationStateDefinition",
        menuName = "Obsidian Protocol/Navigation/Navigation State Definition")]
    public sealed class NavigationStateDefinition : ScriptableObject
    {
        [SerializeField] private string stateId;
        [SerializeField] private string displayName;

        public string StateId => stateId;
        public string DisplayName => displayName;
    }
}
