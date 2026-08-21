using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class MaintenanceNotificationController : MonoBehaviour
    {
        [Header("Notifications")]
        private readonly List<string> notifications =
            new List<string>();

        public IReadOnlyList<string> Notifications =>
            notifications;

        private void OnEnable()
        {
            MaintenanceEvents.UnitMaintenanceRequired +=
                OnMaintenanceRequired;

            MaintenanceEvents.UnitMaintenanceCompleted +=
                OnMaintenanceCompleted;

            MaintenanceEvents.UnitInspectionCompleted +=
                OnInspectionCompleted;

            MaintenanceEvents.UnitDeploymentReady +=
                OnDeploymentReady;
        }

        private void OnDisable()
        {
            MaintenanceEvents.UnitMaintenanceRequired -=
                OnMaintenanceRequired;

            MaintenanceEvents.UnitMaintenanceCompleted -=
                OnMaintenanceCompleted;

            MaintenanceEvents.UnitInspectionCompleted -=
                OnInspectionCompleted;

            MaintenanceEvents.UnitDeploymentReady -=
                OnDeploymentReady;
        }

        private void OnMaintenanceRequired(
            string unitInstanceId)
        {
            AddNotification(
                $"MAINTENANCE REQUIRED: {unitInstanceId}");
        }

        private void OnMaintenanceCompleted(
            string unitInstanceId)
        {
            AddNotification(
                $"MAINTENANCE COMPLETE: {unitInstanceId}");
        }

        private void OnInspectionCompleted(
            string unitInstanceId)
        {
            AddNotification(
                $"INSPECTION COMPLETE: {unitInstanceId}");
        }

        private void OnDeploymentReady(
            string unitInstanceId)
        {
            AddNotification(
                $"UNIT READY FOR DEPLOYMENT: {unitInstanceId}");
        }

        private void AddNotification(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            notifications.Add(message);
        }

        public string GetLatest()
        {
            if (notifications.Count == 0)
                return string.Empty;

            return notifications[notifications.Count - 1];
        }

        public void ClearNotifications()
        {
            notifications.Clear();
        }
    }
}
