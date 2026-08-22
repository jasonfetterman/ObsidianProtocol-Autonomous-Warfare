using UnityEngine;

namespace ObsidianProtocol.Game.World.State
{
    [CreateAssetMenu(
        fileName = "WorldStateDefinition",
        menuName = "Obsidian Protocol/World/World State Definition")]
    public sealed class WorldStateDefinition : ScriptableObject
    {
        [SerializeField] private string stateId;
        [SerializeField] private string displayName;

        public string StateId => stateId;
        public string DisplayName => displayName;
    }
}
