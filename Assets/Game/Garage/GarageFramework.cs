using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Garage
{
    public enum GarageState
    {
        Closed,
        Open,
        Maintenance,
        Configuration,
        Deployment
    }

    public sealed class GarageFramework
    {
        private readonly HashSet<string> registeredAreas =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public GarageState State { get; private set; }

        public bool IsOpen =>
            State != GarageState.Closed;

        public GarageFramework()
        {
            State = GarageState.Closed;
        }

        public void Open()
        {
            State = GarageState.Open;
        }

        public void Close()
        {
            State = GarageState.Closed;
        }

        public void EnterMaintenance()
        {
            if (State != GarageState.Closed)
                State = GarageState.Maintenance;
        }

        public void EnterConfiguration()
        {
            if (State != GarageState.Closed)
                State = GarageState.Configuration;
        }

        public void EnterDeployment()
        {
            if (State != GarageState.Closed)
                State = GarageState.Deployment;
        }

        public bool RegisterArea(string areaId)
        {
            if (string.IsNullOrWhiteSpace(areaId))
                return false;

            return registeredAreas.Add(areaId);
        }

        public bool RemoveArea(string areaId)
        {
            if (string.IsNullOrWhiteSpace(areaId))
                return false;

            return registeredAreas.Remove(areaId);
        }

        public bool HasArea(string areaId)
        {
            if (string.IsNullOrWhiteSpace(areaId))
                return false;

            return registeredAreas.Contains(areaId);
        }

        public IReadOnlyCollection<string> GetAreas()
        {
            return registeredAreas;
        }

        public void Reset()
        {
            State = GarageState.Closed;
            registeredAreas.Clear();
        }
    }
}
