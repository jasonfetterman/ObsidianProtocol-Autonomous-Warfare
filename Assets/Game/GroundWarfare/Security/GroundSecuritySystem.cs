using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.GroundWarfare
{
    public enum GroundSecurityState
    {
        Normal,
        Alert,
        Engaged,
        Defensive,
        Disengaging
    }

    public sealed class GroundSecurityProfile
    {
        public string UnitId { get; }

        public GroundSecurityState State { get; private set; }

        public float DetectionRange { get; private set; }
        public float ThreatThreshold { get; private set; }

        public bool SecurityEnabled { get; private set; }

        public GroundSecurityProfile(
            string unitId)
        {
            UnitId =
                unitId ?? string.Empty;

            State =
                GroundSecurityState.Normal;

            SecurityEnabled = true;
        }

        public void Configure(
            float detectionRange,
            float threatThreshold)
        {
            DetectionRange =
                Math.Max(
                    0f,
                    detectionRange);

            ThreatThreshold =
                Math.Clamp(
                    threatThreshold,
                    0f,
                    1f);
        }

        public void EvaluateThreat(
            float threatLevel)
        {
            if (!SecurityEnabled)
            {
                return;
            }

            float normalizedThreat =
                Math.Clamp(
                    threatLevel,
                    0f,
                    1f);

            if (normalizedThreat >= 0.8f)
            {
                State =
                    GroundSecurityState.Engaged;
            }
            else if (normalizedThreat >= ThreatThreshold)
            {
                State =
                    GroundSecurityState.Alert;
            }
            else
            {
                State =
                    GroundSecurityState.Normal;
            }
        }

        public void SetDefensive()
        {
            if (SecurityEnabled)
            {
                State =
                    GroundSecurityState.Defensive;
            }
        }

        public void BeginDisengagement()
        {
            State =
                GroundSecurityState.Disengaging;
        }

        public void Enable()
        {
            SecurityEnabled = true;
        }

        public void Disable()
        {
            SecurityEnabled = false;
            State = GroundSecurityState.Normal;
        }
    }

    public sealed class GroundSecuritySystem
    {
        private readonly Dictionary<string, GroundSecurityProfile> profiles =
            new Dictionary<string, GroundSecurityProfile>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(
            string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!profiles.ContainsKey(unitId))
            {
                profiles.Add(
                    unitId,
                    new GroundSecurityProfile(unitId));
            }
        }

        public void ConfigureUnit(
            string unitId,
            float detectionRange,
            float threatThreshold)
        {
            RegisterUnit(unitId);

            profiles[unitId].Configure(
                detectionRange,
                threatThreshold);
        }

        public void EvaluateThreat(
            string unitId,
            float threatLevel)
        {
            if (profiles.TryGetValue(
                    unitId,
                    out GroundSecurityProfile profile))
            {
                profile.EvaluateThreat(
                    threatLevel);
            }
        }

        public void SetDefensive(
            string unitId)
        {
            if (profiles.TryGetValue(
                    unitId,
                    out GroundSecurityProfile profile))
            {
                profile.SetDefensive();
            }
        }

        public void BeginDisengagement(
            string unitId)
        {
            if (profiles.TryGetValue(
                    unitId,
                    out GroundSecurityProfile profile))
            {
                profile.BeginDisengagement();
            }
        }

        public void SetEnabled(
            string unitId,
            bool enabled)
        {
            RegisterUnit(unitId);

            if (enabled)
            {
                profiles[unitId].Enable();
            }
            else
            {
                profiles[unitId].Disable();
            }
        }

        public bool TryGetProfile(
            string unitId,
            out GroundSecurityProfile profile)
        {
            return profiles.TryGetValue(
                unitId,
                out profile);
        }

        public void RemoveUnit(
            string unitId)
        {
            profiles.Remove(unitId);
        }

        public void Clear()
        {
            profiles.Clear();
        }
    }
}
