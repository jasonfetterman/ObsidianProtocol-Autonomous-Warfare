using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Modding
{
    public sealed class WeaponCreationDefinition
    {
        public string WeaponId { get; }

        public string WeaponName { get; }

        public string WeaponType { get; }

        public bool Enabled { get; private set; }

        public WeaponCreationDefinition(
            string weaponId,
            string weaponName,
            string weaponType)
        {
            WeaponId =
                weaponId ?? string.Empty;

            WeaponName =
                weaponName ?? string.Empty;

            WeaponType =
                weaponType ?? string.Empty;

            Enabled = true;
        }

        public bool SetEnabled(
            bool enabled)
        {
            Enabled = enabled;

            return true;
        }
    }

    public sealed class WeaponCreationFramework
    {
        private readonly Dictionary<
            string,
            WeaponCreationDefinition> definitions =
            new Dictionary<
                string,
                WeaponCreationDefinition>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int WeaponDefinitionCount =>
            definitions.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            definitions.Clear();
            Initialized = true;

            return true;
        }

        public bool CreateWeapon(
            string weaponId,
            string weaponName,
            string weaponType)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(weaponId) ||
                string.IsNullOrWhiteSpace(weaponName) ||
                string.IsNullOrWhiteSpace(weaponType))
            {
                return false;
            }

            string id =
                weaponId.Trim();

            if (definitions.ContainsKey(id))
            {
                return false;
            }

            definitions.Add(
                id,
                new WeaponCreationDefinition(
                    id,
                    weaponName.Trim(),
                    weaponType.Trim()));

            return true;
        }

        public bool RemoveWeapon(
            string weaponId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(weaponId))
            {
                return false;
            }

            return definitions.Remove(
                weaponId.Trim());
        }

        public WeaponCreationDefinition GetWeapon(
            string weaponId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(weaponId))
            {
                return null;
            }

            definitions.TryGetValue(
                weaponId.Trim(),
                out WeaponCreationDefinition definition);

            return definition;
        }

        public IReadOnlyCollection<
            WeaponCreationDefinition>
            GetWeapons()
        {
            return definitions.Values;
        }

        public void Reset()
        {
            definitions.Clear();
            Initialized = false;
        }
    }
}
