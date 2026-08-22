using UnityEngine;

namespace ObsidianProtocol.Game.World.Destruction
{
    [CreateAssetMenu(
        fileName = "DestructibleObjectDefinition",
        menuName = "Obsidian Protocol/World/Destructible Object Definition")]
    public sealed class DestructibleObjectDefinition : ScriptableObject
    {
        [SerializeField] private string objectId;
        [SerializeField] private string displayName;
        [SerializeField] private float maximumHealth = 100f;

        public string ObjectId => objectId;
        public string DisplayName => displayName;
        public float MaximumHealth => Mathf.Max(1f, maximumHealth);
    }
}
