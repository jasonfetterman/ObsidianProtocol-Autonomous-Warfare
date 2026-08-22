using System.Collections.Generic;
using UnityEngine;
using ObsidianProtocol.Game.Units;

namespace ObsidianProtocol.Game.Squads
{
    public sealed class Squad : MonoBehaviour
    {
        [SerializeField] private SquadDefinition definition;

        private readonly List<UnitInstance> units = new();

        public SquadDefinition Definition => definition;
        public IReadOnlyList<UnitInstance> Units => units;

        public bool AddUnit(UnitInstance unit)
        {
            if (unit == null || units.Contains(unit))
            {
                return false;
            }

            if (definition != null && units.Count >= definition.MaximumUnits)
            {
                return false;
            }

            units.Add(unit);
            return true;
        }

        public bool RemoveUnit(UnitInstance unit)
        {
            if (unit == null)
            {
                return false;
            }

            return units.Remove(unit);
        }

        public void Clear()
        {
            units.Clear();
        }
    }
}
