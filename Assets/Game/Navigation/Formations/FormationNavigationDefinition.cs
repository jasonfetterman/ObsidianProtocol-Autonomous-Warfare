using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Formations
{
    [CreateAssetMenu(
        fileName = "FormationNavigationDefinition",
        menuName = "Obsidian Protocol/Navigation/Formation Navigation Definition")]
    public sealed class FormationNavigationDefinition : ScriptableObject
    {
        [SerializeField] private float spacing = 5f;
        [SerializeField] private int maximumUnits = 32;

        public float Spacing => Mathf.Max(0.1f, spacing);
        public int MaximumUnits => Mathf.Max(1, maximumUnits);
    }
}
