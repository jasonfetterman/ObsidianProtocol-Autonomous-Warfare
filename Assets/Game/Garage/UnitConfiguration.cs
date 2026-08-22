using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Garage
{
    public sealed class UnitConfiguration
    {
        private readonly Dictionary<
            string,
            string> settings =
            new Dictionary<
                string,
                string>(
                StringComparer.OrdinalIgnoreCase);

        public string OwnershipId { get; }

        public bool Locked { get; private set; }

        public UnitConfiguration(
            string ownershipId)
        {
            OwnershipId =
                ownershipId ?? string.Empty;

            Locked = false;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(
                OwnershipId);

        public bool Set(
            string key,
            string value)
        {
            if (Locked ||
                string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            settings[key] =
                value ?? string.Empty;

            return true;
        }

        public bool Remove(
            string key)
        {
            if (Locked ||
                string.IsNullOrWhiteSpace(key))
            {
                return false;
            }

            return settings.Remove(key);
        }

        public bool TryGet(
            string key,
            out string value)
        {
            return settings.TryGetValue(
                key,
                out value);
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
            string>
            GetSettings()
        {
            return settings;
        }

        public void Clear()
        {
            if (!Locked)
                settings.Clear();
        }
    }

    public sealed class UnitConfigurationRegistry
    {
        private readonly Dictionary<
            string,
            UnitConfiguration> configurations =
            new Dictionary<
                string,
                UnitConfiguration>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            UnitConfiguration configuration)
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
            out UnitConfiguration configuration)
        {
            return configurations.TryGetValue(
                ownershipId,
                out configuration);
        }

        public IReadOnlyCollection<
            UnitConfiguration>
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
