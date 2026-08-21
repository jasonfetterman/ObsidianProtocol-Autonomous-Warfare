using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class FleetManager : MonoBehaviour
    {
        public static FleetManager Instance { get; private set; }

        [Header("Garage")]
        [SerializeField]
        private GarageManager garageManager;

        [Header("Active Fleet")]
        [SerializeField]
        private List<OwnedUnit> activeUnits = new List<OwnedUnit>();

        public IReadOnlyList<OwnedUnit> ActiveUnits => activeUnits;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            if (garageManager == null)
            {
                garageManager = GarageManager.Instance;
            }
        }

        public bool DeployUnit(string instanceId)
        {
            if (garageManager == null)
                return false;

            OwnedUnit unit = garageManager.GetOwnedUnit(instanceId);

            if (unit == null)
                return false;

            if (unit.deployed || unit.underMaintenance)
                return false;

            unit.deployed = true;
            unit.missionsCompleted++;

            if (!activeUnits.Contains(unit))
            {
                activeUnits.Add(unit);
            }

            return true;
        }

        public bool RecallUnit(string instanceId)
        {
            OwnedUnit unit = GetActiveUnit(instanceId);

            if (unit == null)
                return false;

            unit.deployed = false;
            
            activeUnits.Remove(unit);

            return true;
        }

        public OwnedUnit GetActiveUnit(string instanceId)
        {
            if (string.IsNullOrEmpty(instanceId))
                return null;

            foreach (OwnedUnit unit in activeUnits)
            {
                if (unit != null && unit.instanceId == instanceId)
                    return unit;
            }

            return null;
        }

        public bool IsDeployed(string instanceId)
        {
            return GetActiveUnit(instanceId) != null;
        }

        public void RecallAll()
        {
            for (int i = activeUnits.Count - 1; i >= 0; i--)
            {
                OwnedUnit unit = activeUnits[i];

                if (unit == null)
                    continue;

                unit.deployed = false;
                            }

            activeUnits.Clear();
        }
    }
}

