using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    [CreateAssetMenu(
        fileName = "UnitDefinitionDatabase",
        menuName = "Obsidian Protocol/Garage/Unit Definition Database"
    )]
    public class UnitDefinitionDatabase : ScriptableObject
    {
        [Header("All Unit Definitions")]
        public List<UnitDefinition> units = new List<UnitDefinition>();

        private Dictionary<string, UnitDefinition> lookup;

        public IReadOnlyList<UnitDefinition> Units => units;

        private void OnEnable()
        {
            BuildLookup();
        }

        public void BuildLookup()
        {
            lookup = new Dictionary<string, UnitDefinition>();

            foreach (UnitDefinition unit in units)
            {
                if (unit == null || unit.identity == null)
                    continue;

                string id = unit.identity.unitId;

                if (string.IsNullOrWhiteSpace(id))
                    continue;

                if (!lookup.ContainsKey(id))
                    lookup.Add(id, unit);
            }
        }

        public UnitDefinition GetUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
                return null;

            if (lookup == null)
                BuildLookup();

            lookup.TryGetValue(unitId, out UnitDefinition unit);
            return unit;
        }

        public bool Contains(string unitId)
        {
            return GetUnit(unitId) != null;
        }
    }
}
