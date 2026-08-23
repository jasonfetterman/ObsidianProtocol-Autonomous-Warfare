using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ContentCompletion
{
    public sealed class ContentCompletionWeapons
    {
        private readonly HashSet<string> weapons =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int WeaponCount =>
            weapons.Count;

        public bool Complete =>
            WeaponCount > 0;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            weapons.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterWeapon(
            string weaponId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(weaponId))
            {
                return false;
            }

            return weapons.Add(
                weaponId.Trim());
        }

        public bool ContainsWeapon(
            string weaponId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(weaponId))
            {
                return false;
            }

            return weapons.Contains(
                weaponId.Trim());
        }

        public IReadOnlyCollection<string>
            GetWeapons()
        {
            return weapons;
        }

        public void Reset()
        {
            weapons.Clear();
            Initialized = false;
        }
    }
}
