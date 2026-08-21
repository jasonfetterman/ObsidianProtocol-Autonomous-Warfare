using System;

namespace ObsidianProtocol.Garage
{
    public static class MaintenanceEvents
    {
        public static event Action<string> UnitMaintenanceRequired;
        public static event Action<string> UnitMaintenanceCompleted;
        public static event Action<string> UnitInspectionCompleted;
        public static event Action<string> UnitDeploymentReady;

        public static void RaiseMaintenanceRequired(
            string unitInstanceId)
        {
            UnitMaintenanceRequired?.Invoke(
                unitInstanceId);
        }

        public static void RaiseMaintenanceCompleted(
            string unitInstanceId)
        {
            UnitMaintenanceCompleted?.Invoke(
                unitInstanceId);
        }

        public static void RaiseInspectionCompleted(
            string unitInstanceId)
        {
            UnitInspectionCompleted?.Invoke(
                unitInstanceId);
        }

        public static void RaiseDeploymentReady(
            string unitInstanceId)
        {
            UnitDeploymentReady?.Invoke(
                unitInstanceId);
        }
    }
}
