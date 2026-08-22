using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Garage
{
    public sealed class UnitPaintConfiguration
    {
        public string OwnershipId { get; }

        public string PrimaryPaint { get; private set; }
        public string SecondaryPaint { get; private set; }
        public string AccentPaint { get; private set; }

        public bool Locked { get; private set; }

        public UnitPaintConfiguration(
            string ownershipId)
        {
            OwnershipId =
                ownershipId ?? string.Empty;

            PrimaryPaint = string.Empty;
            SecondaryPaint = string.Empty;
            AccentPaint = string.Empty;

            Locked = false;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                OwnershipId);

        public bool SetPrimary(
            string paintId)
        {
            if (Locked)
                return false;

            PrimaryPaint =
                paintId ?? string.Empty;

            return true;
        }

        public bool SetSecondary(
            string paintId)
        {
            if (Locked)
                return false;

            SecondaryPaint =
                paintId ?? string.Empty;

            return true;
        }

        public bool SetAccent(
            string paintId)
        {
            if (Locked)
                return false;

            AccentPaint =
                paintId ?? string.Empty;

            return true;
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

            PrimaryPaint = string.Empty;
            SecondaryPaint = string.Empty;
            AccentPaint = string.Empty;
        }
    }

    public sealed class PaintSystem
    {
        private readonly Dictionary<
            string,
            UnitPaintConfiguration> configurations =
            new Dictionary<
                string,
                UnitPaintConfiguration>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            UnitPaintConfiguration configuration)
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
            out UnitPaintConfiguration configuration)
        {
            return configurations.TryGetValue(
                ownershipId,
                out configuration);
        }

        public bool SetPrimary(
            string ownershipId,
            string paintId)
        {
            if (!configurations.TryGetValue(
                    ownershipId,
                    out UnitPaintConfiguration configuration))
            {
                return false;
            }

            return configuration.SetPrimary(
                paintId);
        }

        public bool SetSecondary(
            string ownershipId,
            string paintId)
        {
            if (!configurations.TryGetValue(
                    ownershipId,
                    out UnitPaintConfiguration configuration))
            {
                return false;
            }

            return configuration.SetSecondary(
                paintId);
        }

        public bool SetAccent(
            string ownershipId,
            string paintId)
        {
            if (!configurations.TryGetValue(
                    ownershipId,
                    out UnitPaintConfiguration configuration))
            {
                return false;
            }

            return configuration.SetAccent(
                paintId);
        }

        public IReadOnlyCollection<
            UnitPaintConfiguration>
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
