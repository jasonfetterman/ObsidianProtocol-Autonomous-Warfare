using UnityEngine;

namespace ObsidianProtocol.Game.Combat.Armor
{
    [CreateAssetMenu(
        fileName = "ArmorDefinition",
        menuName = "Obsidian Protocol/Combat/Armor Definition")]
    public sealed class ArmorDefinition : ScriptableObject
    {
        [SerializeField] private float frontArmor = 100f;
        [SerializeField] private float rearArmor = 60f;
        [SerializeField] private float sideArmor = 80f;
        [SerializeField] private float topArmor = 50f;

        public float FrontArmor => Mathf.Max(0f, frontArmor);
        public float RearArmor => Mathf.Max(0f, rearArmor);
        public float SideArmor => Mathf.Max(0f, sideArmor);
        public float TopArmor => Mathf.Max(0f, topArmor);
    }
}
