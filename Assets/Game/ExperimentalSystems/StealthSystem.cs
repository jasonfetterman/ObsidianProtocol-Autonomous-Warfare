using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ExperimentalSystems
{
    public enum StealthSignatureType
    {
        Visual,
        Thermal,
        Radar,
        Audio,
        Electronic
    }

    public sealed class StealthProfile
    {
        private readonly Dictionary<StealthSignatureType, float> signatures =
            new Dictionary<StealthSignatureType, float>();

        public string UnitId { get; }

        public bool Enabled { get; private set; }

        public float OverallSignature { get; private set; }

        public StealthProfile(
            string unitId)
        {
            UnitId =
                unitId ?? string.Empty;

            Enabled = false;

            foreach (StealthSignatureType type
                in Enum.GetValues(
                    typeof(StealthSignatureType)))
            {
                signatures[type] = 1f;
            }

            Recalculate();
        }

        public void SetEnabled(
            bool enabled)
        {
            Enabled =
                enabled;

            Recalculate();
        }

        public void SetSignature(
            StealthSignatureType type,
            float signature)
        {
            signatures[type] =
                Math.Clamp(
                    signature,
                    0f,
                    1f);

            Recalculate();
        }

        public float GetSignature(
            StealthSignatureType type)
        {
            return signatures.TryGetValue(
                type,
                out float signature)
                ? signature
                : 1f;
        }

        private void Recalculate()
        {
            float total = 0f;

            foreach (float signature
                in signatures.Values)
            {
                total += signature;
            }

            OverallSignature =
                signatures.Count == 0
                    ? 1f
                    : total / signatures.Count;
        }
    }

    public sealed class StealthSystem
    {
        private readonly Dictionary<string, StealthProfile> profiles =
            new Dictionary<string, StealthProfile>(
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
                    new StealthProfile(unitId));
            }
        }

        public void SetEnabled(
            string unitId,
            bool enabled)
        {
            RegisterUnit(unitId);

            profiles[unitId].SetEnabled(
                enabled);
        }

        public void SetSignature(
            string unitId,
            StealthSignatureType type,
            float signature)
        {
            RegisterUnit(unitId);

            profiles[unitId].SetSignature(
                type,
                signature);
        }

        public float GetSignature(
            string unitId,
            StealthSignatureType type)
        {
            return profiles.TryGetValue(
                       unitId,
                       out StealthProfile profile)
                ? profile.GetSignature(type)
                : 1f;
        }

        public float GetOverallSignature(
            string unitId)
        {
            return profiles.TryGetValue(
                       unitId,
                       out StealthProfile profile)
                ? profile.OverallSignature
                : 1f;
        }

        public bool IsEnabled(
            string unitId)
        {
            return profiles.TryGetValue(
                       unitId,
                       out StealthProfile profile) &&
                   profile.Enabled;
        }

        public bool TryGetProfile(
            string unitId,
            out StealthProfile profile)
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
