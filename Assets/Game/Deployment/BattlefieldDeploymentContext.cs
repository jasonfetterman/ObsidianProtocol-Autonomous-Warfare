using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Deployment
{
    public class BattlefieldDeploymentContext : MonoBehaviour
    {
        public static BattlefieldDeploymentContext Instance { get; private set; }

        private readonly List<DeploymentEntry> deployedEntries =
            new List<DeploymentEntry>();

        public IReadOnlyList<DeploymentEntry> DeployedEntries => deployedEntries;
        public string OperationId { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public string SelectedServiceRecordId { get; private set; } = string.Empty;

        public void SetSelectedServiceRecord(string serviceRecordId)
        {
            SelectedServiceRecordId = serviceRecordId ?? string.Empty;
            Debug.Log(
                "[BATTLEFIELD CONTEXT] SELECTED SERVICE RECORD // " +
                SelectedServiceRecordId);
        }

        public void SetDeployment(
            string operationId,
            IEnumerable<DeploymentEntry> entries)
        {
            deployedEntries.Clear();
            OperationId = operationId ?? string.Empty;

            if (entries == null)
                return;

            foreach (DeploymentEntry entry in entries)
            {
                if (entry != null)
                    deployedEntries.Add(entry);
            }

            Debug.Log(
                "[BATTLEFIELD CONTEXT] DEPLOYMENT STORED // " +
                deployedEntries.Count +
                " UNITS // OPERATION=" +
                OperationId);
        }

        public void ClearDeployment()
        {
            deployedEntries.Clear();
            OperationId = string.Empty;
        }
    }
}
