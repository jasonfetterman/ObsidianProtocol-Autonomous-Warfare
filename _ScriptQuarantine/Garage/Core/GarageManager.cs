using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class GarageManager : MonoBehaviour
    {
        public static GarageManager Instance { get; private set; }

        [Header("Database")]
        [SerializeField]
        private UnitDefinitionDatabase unitDatabase;

        [Header("Owned Fleet")]
        [SerializeField]
        private List<OwnedUnit> ownedUnits = new List<OwnedUnit>();

        public UnitDefinitionDatabase UnitDatabase => unitDatabase;

        public IReadOnlyList<OwnedUnit> OwnedUnits => ownedUnits;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            BuildFleetIds();
        }

        private void BuildFleetIds()
        {
            foreach (OwnedUnit unit in ownedUnits)
            {
                if (unit == null)
                    continue;

                if (string.IsNullOrEmpty(unit.instanceId))
                {
                    unit.Initialize(unit.definition, System.Guid.NewGuid().ToString("N"));
                }
            }
        }

        public OwnedUnit AddUnit(UnitDefinition definition)
        {
            if (definition == null)
            {
                Debug.LogWarning("Cannot add a null UnitDefinition.", this);
                return null;
            }

            OwnedUnit ownedUnit = new OwnedUnit();
            ownedUnit.Initialize(definition, System.Guid.NewGuid().ToString("N"));

            ownedUnits.Add(ownedUnit);

            return ownedUnit;
        }

        public bool RemoveUnit(string instanceId)
        {
            if (string.IsNullOrEmpty(instanceId))
                return false;

            for (int i = 0; i < ownedUnits.Count; i++)
            {
                OwnedUnit unit = ownedUnits[i];

                if (unit != null && unit.instanceId == instanceId)
                {
                    ownedUnits.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public OwnedUnit GetOwnedUnit(string instanceId)
        {
            if (string.IsNullOrEmpty(instanceId))
                return null;

            foreach (OwnedUnit unit in ownedUnits)
            {
                if (unit != null && unit.instanceId == instanceId)
                    return unit;
            }

            return null;
        }

        public List<OwnedUnit> GetUnitsByType(MobilityUnitType type)
        {
            List<OwnedUnit> results = new List<OwnedUnit>();

            foreach (OwnedUnit unit in ownedUnits)
            {
                if (unit == null ||
                    unit.definition == null ||
                    unit.definition.stats == null ||
                    unit.definition.stats.physical == null)
                {
                    continue;
                }

                if (unit.definition.stats.physical.mobilityType == type)
                {
                    results.Add(unit);
                }
            }

            return results;
        }
    }
}

