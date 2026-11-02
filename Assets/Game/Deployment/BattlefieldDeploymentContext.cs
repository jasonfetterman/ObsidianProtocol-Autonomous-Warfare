using System.Collections.Generic;
using UnityEngine;
using ObsidianProtocol.Game.Core;

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


        public OperationsSnapshot CaptureSnapshot()
        {
            OperationsSnapshot snapshot = new OperationsSnapshot
            {
                OperationId = OperationId ?? string.Empty,
                SelectedServiceRecordId = SelectedServiceRecordId ?? string.Empty
            };

            foreach (DeploymentEntry entry in deployedEntries)
            {
                if (entry == null)
                    continue;

                snapshot.Entries.Add(new DeploymentEntrySnapshot
                {
                    DeploymentId = entry.DeploymentId,
                    ServiceRecordId = entry.ServiceRecordId,
                    UnitId = entry.UnitId,
                    DeploymentPoints = entry.DeploymentPoints,
                    State = (int)entry.State
                });
            }

            return snapshot;
        }

        public void RestoreSnapshot(OperationsSnapshot snapshot)
        {
            deployedEntries.Clear();

            if (snapshot == null)
            {
                OperationId = string.Empty;
                SelectedServiceRecordId = string.Empty;
                return;
            }

            OperationId = snapshot.OperationId ?? string.Empty;
            SelectedServiceRecordId = snapshot.SelectedServiceRecordId ?? string.Empty;

            if (snapshot.Entries == null)
                return;

            foreach (DeploymentEntrySnapshot saved in snapshot.Entries)
            {
                if (saved == null)
                    continue;

                DeploymentState restoredState = (DeploymentState)saved.State;

                if (!System.Enum.IsDefined(typeof(DeploymentState), restoredState))
                {
                    Debug.LogWarning(
                        "[BATTLEFIELD CONTEXT] Invalid saved deployment state skipped. DeploymentId=" +
                        saved.DeploymentId + " State=" + saved.State);
                    continue;
                }

                DeploymentEntry entry = new DeploymentEntry(
                    saved.DeploymentId,
                    saved.UnitId,
                    saved.DeploymentPoints,
                    saved.ServiceRecordId);

                entry.RestoreState(restoredState);

                if (entry.Valid)
                    deployedEntries.Add(entry);
                else
                    Debug.LogWarning(
                        "[BATTLEFIELD CONTEXT] Invalid saved deployment entry skipped. DeploymentId=" +
                        saved.DeploymentId);
            }

            Debug.Log(
                "[BATTLEFIELD CONTEXT] DEPLOYMENT RESTORED // " +
                deployedEntries.Count +
                " UNITS // OPERATION=" +
                OperationId);
        }
        public void ClearDeployment()
        {
            deployedEntries.Clear();
            OperationId = string.Empty;
            SelectedServiceRecordId = string.Empty;
        }
    }
}
