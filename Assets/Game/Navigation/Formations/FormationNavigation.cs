using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Formations
{
    public sealed class FormationNavigation : MonoBehaviour
    {
        [SerializeField] private FormationNavigationDefinition definition;

        public FormationNavigationDefinition Definition => definition;

        public float Spacing =>
            definition != null ? definition.Spacing : 5f;

        public int MaximumUnits =>
            definition != null ? definition.MaximumUnits : 32;

        public Vector3 GetOffset(
            int formationIndex,
            int unitsPerRow = 4)
        {
            unitsPerRow = Mathf.Max(1, unitsPerRow);

            int row = formationIndex / unitsPerRow;
            int column = formationIndex % unitsPerRow;

            float centeredColumn =
                column - ((unitsPerRow - 1) * 0.5f);

            return new Vector3(
                centeredColumn * Spacing,
                0f,
                -row * Spacing);
        }
    }
}
