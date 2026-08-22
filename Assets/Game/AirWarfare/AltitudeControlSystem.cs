using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AirWarfare
{
    public sealed class AltitudeControlState
    {
        public string UnitId { get; }

        public float CurrentAltitude { get; private set; }
        public float TargetAltitude { get; private set; }

        public float MinimumAltitude { get; private set; }
        public float MaximumAltitude { get; private set; }

        public float ClimbRate { get; private set; }
        public float DescentRate { get; private set; }

        public bool Enabled { get; private set; }

        public AltitudeControlState(string unitId)
        {
            UnitId = unitId ?? string.Empty;

            MinimumAltitude = 0f;
            MaximumAltitude = float.MaxValue;

            Enabled = true;
        }

        public void Configure(
            float minimumAltitude,
            float maximumAltitude,
            float climbRate,
            float descentRate)
        {
            MinimumAltitude =
                Math.Max(0f, minimumAltitude);

            MaximumAltitude =
                Math.Max(
                    MinimumAltitude,
                    maximumAltitude);

            ClimbRate =
                Math.Max(0f, climbRate);

            DescentRate =
                Math.Max(0f, descentRate);

            CurrentAltitude =
                Math.Clamp(
                    CurrentAltitude,
                    MinimumAltitude,
                    MaximumAltitude);

            TargetAltitude =
                Math.Clamp(
                    TargetAltitude,
                    MinimumAltitude,
                    MaximumAltitude);
        }

        public void SetCurrentAltitude(float altitude)
        {
            CurrentAltitude =
                Math.Clamp(
                    altitude,
                    MinimumAltitude,
                    MaximumAltitude);
        }

        public void SetTargetAltitude(float altitude)
        {
            TargetAltitude =
                Math.Clamp(
                    altitude,
                    MinimumAltitude,
                    MaximumAltitude);
        }

        public void Update(float deltaTime)
        {
            if (!Enabled)
            {
                return;
            }

            float delta =
                Math.Max(0f, deltaTime);

            if (CurrentAltitude < TargetAltitude)
            {
                CurrentAltitude =
                    Math.Min(
                        TargetAltitude,
                        CurrentAltitude +
                        ClimbRate * delta);
            }
            else if (CurrentAltitude > TargetAltitude)
            {
                CurrentAltitude =
                    Math.Max(
                        TargetAltitude,
                        CurrentAltitude -
                        DescentRate * delta);
            }
        }

        public void SetEnabled(bool enabled)
        {
            Enabled = enabled;
        }
    }

    public sealed class AltitudeControlSystem
    {
        private readonly Dictionary<string, AltitudeControlState> states =
            new Dictionary<string, AltitudeControlState>(
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
                    new AltitudeControlState(unitId));
            }
        }

        public void ConfigureDrone(
            string unitId,
            float minimumAltitude,
            float maximumAltitude,
            float climbRate,
            float descentRate)
        {
            RegisterDrone(unitId);

            states[unitId].Configure(
                minimumAltitude,
                maximumAltitude,
                climbRate,
                descentRate);
        }

        public void SetCurrentAltitude(
            string unitId,
            float altitude)
        {
            RegisterDrone(unitId);

            states[unitId].SetCurrentAltitude(altitude);
        }

        public void SetTargetAltitude(
            string unitId,
            float altitude)
        {
            RegisterDrone(unitId);

            states[unitId].SetTargetAltitude(altitude);
        }

        public void UpdateDrone(
            string unitId,
            float deltaTime)
        {
            if (!states.TryGetValue(
                    unitId,
                    out AltitudeControlState state))
            {
                return;
            }

            state.Update(deltaTime);
        }

        public bool TryGetState(
            string unitId,
            out AltitudeControlState state)
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
