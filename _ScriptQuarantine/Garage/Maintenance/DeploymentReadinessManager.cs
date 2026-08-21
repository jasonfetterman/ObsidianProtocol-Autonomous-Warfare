using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class DeploymentReadinessManager : MonoBehaviour
    {
        [Header("Readiness")]
        private readonly Dictionary<string, DeploymentReadiness> readiness =
            new Dictionary<string, DeploymentReadiness>();

        public DeploymentReadiness GetOrCreate(string unitInstanceId)
        {
            if (string.IsNullOrWhiteSpace(unitInstanceId))
                return null;

            if (!readiness.TryGetValue(
                    unitInstanceId,
                    out DeploymentReadiness state))
            {
                state = new DeploymentReadiness
                {
                    unitInstanceId = unitInstanceId
                };

                readiness.Add(unitInstanceId, state);
            }

            return state;
        }

        public void SetMaintenanceReady(
            string unitInstanceId,
            bool value)
        {
            DeploymentReadiness state =
                GetOrCreate(unitInstanceId);

            if (state == null)
                return;

            state.maintenanceReady = value;
            state.Calculate();
        }

        public void SetInspectionPassed(
            string unitInstanceId,
            bool value)
        {
            DeploymentReadiness state =
                GetOrCreate(unitInstanceId);

            if (state == null)
                return;

            state.inspectionPassed = value;
            state.Calculate();
        }

        public void SetResourcesReady(
            string unitInstanceId,
            bool value)
        {
            DeploymentReadiness state =
                GetOrCreate(unitInstanceId);

            if (state == null)
                return;

            state.resourcesReady = value;
            state.Calculate();
        }

        public void SetCrewReady(
            string unitInstanceId,
            bool value)
        {
            DeploymentReadiness state =
                GetOrCreate(unitInstanceId);

            if (state == null)
                return;

            state.crewReady = value;
            state.Calculate();
        }

        public bool IsDeploymentReady(
            string unitInstanceId)
        {
            DeploymentReadiness state =
                GetOrCreate(unitInstanceId);

            return state != null &&
                   state.deploymentReady;
        }

        public void Remove(string unitInstanceId)
        {
            if (string.IsNullOrWhiteSpace(unitInstanceId))
                return;

            readiness.Remove(unitInstanceId);
        }

        public void Clear()
        {
            readiness.Clear();
        }
    }
}
