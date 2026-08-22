using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Garage
{
    public enum AIPersonalityType
    {
        Balanced,
        Aggressive,
        Defensive,
        Cautious,
        Reconnaissance,
        Support,
        Adaptive
    }

    public sealed class AIPersonalityConfiguration
    {
        private readonly Dictionary<
            string,
            float> traits =
            new Dictionary<
                string,
                float>(
                StringComparer.OrdinalIgnoreCase);

        public string OwnershipId { get; }

        public AIPersonalityType Personality
        {
            get;
            private set;
        }

        public bool Enabled { get; private set; }
        public bool Locked { get; private set; }

        public AIPersonalityConfiguration(
            string ownershipId)
        {
            OwnershipId =
                ownershipId ?? string.Empty;

            Personality =
                AIPersonalityType.Balanced;

            Enabled = true;
            Locked = false;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                OwnershipId);

        public void SetPersonality(
            AIPersonalityType personality)
        {
            if (Locked)
                return;

            Personality = personality;
        }

        public bool SetTrait(
            string traitId,
            float value)
        {
            if (Locked ||
                string.IsNullOrWhiteSpace(
                    traitId))
            {
                return false;
            }

            traits[traitId] =
                Math.Max(
                    0f,
                    Math.Min(
                        1f,
                        value));

            return true;
        }

        public bool TryGetTrait(
            string traitId,
            out float value)
        {
            return traits.TryGetValue(
                traitId,
                out value);
        }

        public void Enable()
        {
            if (!Locked)
                Enabled = true;
        }

        public void Disable()
        {
            if (!Locked)
                Enabled = false;
        }

        public void Lock()
        {
            Locked = true;
        }

        public void Unlock()
        {
            Locked = false;
        }

        public IReadOnlyDictionary<
            string,
            float>
            GetTraits()
        {
            return traits;
        }

        public void Reset()
        {
            if (Locked)
                return;

            Personality =
                AIPersonalityType.Balanced;

            Enabled = true;
            traits.Clear();
        }
    }

    public sealed class AIPersonalityRegistry
    {
        private readonly Dictionary<
            string,
            AIPersonalityConfiguration> configurations =
            new Dictionary<
                string,
                AIPersonalityConfiguration>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            AIPersonalityConfiguration configuration)
        {
            if (configuration == null ||
                !configuration.Valid ||
                configurations.ContainsKey(
                    configuration.OwnershipId))
            {
                return false;
            }

            configurations.Add(
                configuration.OwnershipId,
                configuration);

            return true;
        }

        public bool Remove(
            string ownershipId)
        {
            if (string.IsNullOrWhiteSpace(
                    ownershipId))
            {
                return false;
            }

            return configurations.Remove(
                ownershipId);
        }

        public bool TryGet(
            string ownershipId,
            out AIPersonalityConfiguration configuration)
        {
            return configurations.TryGetValue(
                ownershipId,
                out configuration);
        }

        public IReadOnlyCollection<
            AIPersonalityConfiguration>
            GetConfigurations()
        {
            return configurations.Values;
        }

        public void Clear()
        {
            configurations.Clear();
        }
    }
}
