using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class GarageUnitRegistry : MonoBehaviour
    {
        [Header("Owned Units")]
        [SerializeField]
        private List<OwnedUnit> ownedUnits =
            new List<OwnedUnit>();

        public IReadOnlyList<OwnedUnit> OwnedUnits =>
            ownedUnits;

        public int Count =>
            ownedUnits.Count;

        public void Add(OwnedUnit unit)
        {
            if (unit == null)
                return;

            if (ownedUnits.Contains(unit))
                return;

            ownedUnits.Add(unit);
        }

        public bool Remove(OwnedUnit unit)
        {
            if (unit == null)
                return false;

            return ownedUnits.Remove(unit);
        }

        public OwnedUnit GetByInstanceId(
            string instanceId)
        {
            if (string.IsNullOrWhiteSpace(instanceId))
                return null;

            foreach (OwnedUnit unit in ownedUnits)
            {
                if (unit == null)
                    continue;

                if (unit.instanceId == instanceId)
                    return unit;
            }

            return null;
        }

        public List<OwnedUnit> GetByDefinitionId(
            string definitionId)
        {
            List<OwnedUnit> results =
                new List<OwnedUnit>();

            foreach (OwnedUnit unit in ownedUnits)
            {
                if (unit == null)
                    continue;

                if (unit.UnitId == definitionId)
                    results.Add(unit);
            }

            return results;
        }

        public void Clear()
        {
            ownedUnits.Clear();
        }
    }
}
