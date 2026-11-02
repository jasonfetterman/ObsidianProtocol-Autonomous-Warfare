using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Units
{
    [CreateAssetMenu(
        fileName = "UnitDatabase",
        menuName = "Obsidian Protocol/Units/Unit Database")]
    public sealed class UnitDatabase : ScriptableObject
    {
        [SerializeField]
        private List<UnitDefinition> units = new List<UnitDefinition>();

        private Dictionary<string, UnitDefinition> lookup;

        public IReadOnlyList<UnitDefinition> Units => units;

        private void OnEnable()
        {
            RebuildLookup();
        }

        public void RebuildLookup()
        {
            lookup = new Dictionary<string, UnitDefinition>();

            foreach (UnitDefinition unit in units)
            {
                if (unit == null)
                    continue;

                if (string.IsNullOrWhiteSpace(unit.UnitId))
                    continue;

                if (!lookup.ContainsKey(unit.UnitId))
                    lookup.Add(unit.UnitId, unit);
            }
        }

        public UnitDefinition GetById(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
                return null;

            if (lookup == null)
                RebuildLookup();

            lookup.TryGetValue(unitId, out UnitDefinition result);
            return result;
        }

        public List<UnitDefinition> GetByCategory(UnitCategory category)
        {
            List<UnitDefinition> results = new List<UnitDefinition>();

            foreach (UnitDefinition unit in units)
            {
                if (unit != null && unit.Category == category)
                    results.Add(unit);
            }

            return results;
        }
    }
}
