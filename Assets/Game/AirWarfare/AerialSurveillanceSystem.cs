using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AirWarfare
{
    public enum SurveillanceMode
    {
        Passive,
        Active,
        Persistent,
        SectorSweep,
        Track
    }

    public sealed class AerialSurveillanceState
    {
        public string UnitId { get; }

        public SurveillanceMode Mode { get; private set; }

        public float DetectionRadius { get; private set; }
        public float ScanInterval { get; private set; }

        public string AssignedSector { get; private set; }

        public bool Enabled { get; private set; }

        public AerialSurveillanceState(string unitId)
        {
            UnitId = unitId ?? string.Empty;

            Mode = SurveillanceMode.Passive;
            AssignedSector = string.Empty;

            Enabled = false;
        }

        public void Configure(
            float detectionRadius,
            float scanInterval)
        {
            DetectionRadius =
                Math.Max(0f, detectionRadius);

            ScanInterval =
                Math.Max(0f, scanInterval);
        }

        public void SetMode(
            SurveillanceMode mode)
        {
            Mode = mode;
        }

        public void AssignSector(
            string sectorId)
        {
            AssignedSector =
                sectorId ?? string.Empty;
        }

        public void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }

        public bool CanScan()
        {
            return Enabled &&
                   !string.IsNullOrWhiteSpace(
                       AssignedSector);
        }
    }

    public sealed class AerialSurveillanceSystem
    {
        private readonly Dictionary<string, AerialSurveillanceState> states =
            new Dictionary<string, AerialSurveillanceState>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterDrone(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!states.ContainsKey(unitId))
            {
                states.Add(
                    unitId,
                    new AerialSurveillanceState(unitId));
            }
        }

        public void ConfigureDrone(
            string unitId,
            float detectionRadius,
            float scanInterval)
        {
            RegisterDrone(unitId);

            states[unitId].Configure(
                detectionRadius,
                scanInterval);
        }

        public void SetMode(
            string unitId,
            SurveillanceMode mode)
        {
            RegisterDrone(unitId);

            states[unitId].SetMode(mode);
        }

        public void AssignSector(
            string unitId,
            string sectorId)
        {
            RegisterDrone(unitId);

            states[unitId].AssignSector(
                sectorId);
        }

        public void EnableDrone(string unitId)
        {
            RegisterDrone(unitId);

            states[unitId].Enable();
        }

        public void DisableDrone(string unitId)
        {
            if (states.TryGetValue(
                    unitId,
                    out AerialSurveillanceState state))
            {
                state.Disable();
            }
        }

        public bool CanScan(string unitId)
        {
            return states.TryGetValue(
                       unitId,
                       out AerialSurveillanceState state) &&
                   state.CanScan();
        }

        public bool TryGetState(
            string unitId,
            out AerialSurveillanceState state)
        {
            return states.TryGetValue(
                unitId,
                out state);
        }

        public void RemoveDrone(string unitId)
        {
            states.Remove(unitId);
        }

        public void Clear()
        {
            states.Clear();
        }
    }
}
