using UnityEngine;

namespace ObsidianProtocol.Game.Combat.CriticalDamage
{
    [CreateAssetMenu(
        fileName = "CriticalDamageDefinition",
        menuName = "Obsidian Protocol/Combat/Critical Damage Definition")]
    public sealed class CriticalDamageDefinition : ScriptableObject
    {
        [SerializeField] private float criticalChance = 0.1f;
        [SerializeField] private float criticalMultiplier = 2f;

        public float CriticalChance =>
            Mathf.Clamp01(criticalChance);

        public float CriticalMultiplier =>
            Mathf.Max(1f, criticalMultiplier);
    }
}
