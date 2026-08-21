using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class InspectionManager : MonoBehaviour
    {
        [Header("Inspections")]
        private readonly Dictionary<string, MaintenanceInspection> inspections =
            new Dictionary<string, MaintenanceInspection>();

        public MaintenanceInspection Inspect(string unitInstanceId)
        {
            if (string.IsNullOrWhiteSpace(unitInstanceId))
                return null;

            if (!inspections.TryGetValue(
                    unitInstanceId,
                    out MaintenanceInspection inspection))
            {
                inspection = new MaintenanceInspection
                {
                    unitInstanceId = unitInstanceId
                };

                inspections.Add(
                    unitInstanceId,
                    inspection);
            }

            inspection.Calculate();

            return inspection;
        }

        public MaintenanceInspection Get(
            string unitInstanceId)
        {
            if (string.IsNullOrWhiteSpace(unitInstanceId))
                return null;

            inspections.TryGetValue(
                unitInstanceId,
                out MaintenanceInspection inspection);

            return inspection;
        }

        public void Remove(string unitInstanceId)
        {
            if (string.IsNullOrWhiteSpace(unitInstanceId))
                return;

            inspections.Remove(unitInstanceId);
        }

        public void Clear()
        {
            inspections.Clear();
        }
    }
}
