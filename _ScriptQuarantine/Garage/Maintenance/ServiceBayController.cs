using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class ServiceBayController : MonoBehaviour
    {
        [Header("Service Bays")]
        [SerializeField]
        private List<MaintenanceStation> stations =
            new List<MaintenanceStation>();

        [Header("Current Bay")]
        [SerializeField]
        private int activeStationIndex;

        public IReadOnlyList<MaintenanceStation> Stations =>
            stations;

        public MaintenanceStation ActiveStation
        {
            get
            {
                if (stations.Count == 0)
                    return null;

                activeStationIndex =
                    Mathf.Clamp(
                        activeStationIndex,
                        0,
                        stations.Count - 1);

                return stations[activeStationIndex];
            }
        }

        public void RegisterStation(
            MaintenanceStation station)
        {
            if (station == null)
                return;

            if (stations.Contains(station))
                return;

            stations.Add(station);
        }

        public void UnregisterStation(
            MaintenanceStation station)
        {
            if (station == null)
                return;

            stations.Remove(station);

            if (stations.Count == 0)
            {
                activeStationIndex = 0;
                return;
            }

            activeStationIndex =
                Mathf.Clamp(
                    activeStationIndex,
                    0,
                    stations.Count - 1);
        }

        public void SelectStation(int index)
        {
            if (stations.Count == 0)
                return;

            if (index < 0 ||
                index >= stations.Count)
                return;

            activeStationIndex = index;
        }

        public bool ServiceUnit(
            string unitInstanceId,
            float repairAmount)
        {
            MaintenanceStation station =
                ActiveStation;

            if (station == null)
                return false;

            return station.Service(
                unitInstanceId,
                repairAmount);
        }

        public void InspectUnit(
            string unitInstanceId)
        {
            MaintenanceStation station =
                ActiveStation;

            if (station == null)
                return;

            station.Inspect(
                unitInstanceId);
        }

        public bool CanDeploy(
            string unitInstanceId)
        {
            MaintenanceStation station =
                ActiveStation;

            if (station == null)
                return false;

            return station.CanDeploy(
                unitInstanceId);
        }

        public void ClearStations()
        {
            stations.Clear();
            activeStationIndex = 0;
        }
    }
}
