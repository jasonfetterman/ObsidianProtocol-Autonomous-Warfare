using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AirWarfare
{
    public sealed class FlightStabilizationState
    {
        public string UnitId { get; }

        public float Roll { get; private set; }
        public float Pitch { get; private set; }
        public float Yaw { get; private set; }

        public float RollCorrection { get; private set; }
        public float PitchCorrection { get; private set; }
        public float YawCorrection { get; private set; }

        public bool Enabled { get; private set; }

        public FlightStabilizationState(string unitId)
        {
            UnitId = unitId ?? string.Empty;
            Enabled = true;
        }

        public void SetOrientation(
            float roll,
            float pitch,
            float yaw)
        {
            Roll = roll;
            Pitch = pitch;
            Yaw = yaw;
        }

        public void CalculateCorrection(
            float targetRoll,
            float targetPitch,
            float targetYaw,
            float correctionStrength)
        {
            if (!Enabled)
            {
                return;
            }

            float strength =
                Math.Max(0f, correctionStrength);

            RollCorrection =
                (targetRoll - Roll) * strength;

            PitchCorrection =
                (targetPitch - Pitch) * strength;

            YawCorrection =
                (targetYaw - Yaw) * strength;
        }

        public void SetEnabled(bool enabled)
        {
            Enabled = enabled;

            if (!enabled)
            {
                RollCorrection = 0f;
                PitchCorrection = 0f;
                YawCorrection = 0f;
            }
        }
    }

    public sealed class FlightStabilizationSystem
    {
        private readonly Dictionary<string, FlightStabilizationState> states =
            new Dictionary<string, FlightStabilizationState>(
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
                    new FlightStabilizationState(unitId));
            }
        }

        public void SetOrientation(
            string unitId,
            float roll,
            float pitch,
            float yaw)
        {
            RegisterDrone(unitId);

            states[unitId].SetOrientation(
                roll,
                pitch,
                yaw);
        }

        public void CalculateCorrection(
            string unitId,
            float targetRoll,
            float targetPitch,
            float targetYaw,
            float correctionStrength)
        {
            RegisterDrone(unitId);

            states[unitId].CalculateCorrection(
                targetRoll,
                targetPitch,
                targetYaw,
                correctionStrength);
        }

        public bool TryGetState(
            string unitId,
            out FlightStabilizationState state)
        {
            return states.TryGetValue(
                unitId,
                out state);
        }

        public void SetEnabled(
            string unitId,
            bool enabled)
        {
            RegisterDrone(unitId);

            states[unitId].SetEnabled(enabled);
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
