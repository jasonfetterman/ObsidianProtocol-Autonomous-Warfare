using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Garage
{
    public sealed class UnitEmblemConfiguration
    {
        public string OwnershipId { get; }

        public string EmblemId { get; private set; }
        public string Position { get; private set; }

        public bool Enabled { get; private set; }
        public bool Locked { get; private set; }

        public UnitEmblemConfiguration(
            string ownershipId)
        {
            OwnershipId =
                ownershipId ?? string.Empty;

            EmblemId = string.Empty;
            Position = string.Empty;

            Enabled = false;
            Locked = false;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                OwnershipId);

        public bool SetEmblem(
            string emblemId)
        {
            if (Locked ||
                string.IsNullOrWhiteSpace(emblemId))
            {
                return false;
            }

            EmblemId = emblemId;
            Enabled = true;

            return true;
        }

        public bool SetPosition(
            string position)
        {
            if (Locked)
                return false;

            Position =
                position ?? string.Empty;

            return true;
        }

        public void Enable()
        {
            if (!string.IsNullOrWhiteSpace(EmblemId))
                Enabled = true;
        }

        public void Disable()
        {
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

        public void Reset()
        {
            if (Locked)
                return;

            EmblemId = string.Empty;
            Position = string.Empty;
            Enabled = false;
        }
    }

    public sealed class EmblemSystem
    {
        private readonly Dictionary<
            string,
            UnitEmblemConfiguration> configurations =
            new Dictionary<
                string,
                UnitEmblemConfiguration>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            UnitEmblemConfiguration configuration)
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
            out UnitEmblemConfiguration configuration)
        {
            return configurations.TryGetValue(
                ownershipId,
                out configuration);
        }

        public bool SetEmblem(
            string ownershipId,
            string emblemId)
        {
            if (!configurations.TryGetValue(
                    ownershipId,
                    out UnitEmblemConfiguration configuration))
            {
                return false;
            }

            return configuration.SetEmblem(
                emblemId);
        }

        public bool SetPosition(
            string ownershipId,
            string position)
        {
            if (!configurations.TryGetValue(
                    ownershipId,
                    out UnitEmblemConfiguration configuration))
            {
                return false;
            }

            return configuration.SetPosition(
                position);
        }

        public IReadOnlyCollection<
            UnitEmblemConfiguration>
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
