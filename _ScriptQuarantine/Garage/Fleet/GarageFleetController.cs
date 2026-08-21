using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class GarageFleetController : MonoBehaviour
    {
        [Header("Fleet")]
        [SerializeField]
        private GarageFleetState fleet =
            new GarageFleetState();

        public GarageFleetState Fleet => fleet;

        public void SetFleetIdentity(
            string fleetId,
            string fleetName)
        {
            fleet.fleetId = fleetId;
            fleet.fleetName = fleetName;
        }

        public void AddUnit(string instanceId)
        {
            if (string.IsNullOrWhiteSpace(instanceId))
                return;

            if (fleet.unitInstanceIds.Contains(instanceId))
                return;

            fleet.unitInstanceIds.Add(instanceId);
        }

        public bool RemoveUnit(string instanceId)
        {
            if (string.IsNullOrWhiteSpace(instanceId))
                return false;

            if (fleet.activeUnitInstanceId == instanceId)
                fleet.activeUnitInstanceId = null;

            return fleet.unitInstanceIds.Remove(instanceId);
        }

        public void SetActiveUnit(string instanceId)
        {
            if (!fleet.unitInstanceIds.Contains(instanceId))
                return;

            fleet.activeUnitInstanceId = instanceId;
        }

        public bool ContainsUnit(string instanceId)
        {
            return fleet.unitInstanceIds.Contains(instanceId);
        }

        public void DeployFleet()
        {
            fleet.deployed = true;
            fleet.locked = true;
        }

        public void RecallFleet()
        {
            fleet.deployed = false;
            fleet.locked = false;
        }

        public void LockFleet()
        {
            fleet.locked = true;
        }

        public void UnlockFleet()
        {
            fleet.locked = false;
        }
    }
}
