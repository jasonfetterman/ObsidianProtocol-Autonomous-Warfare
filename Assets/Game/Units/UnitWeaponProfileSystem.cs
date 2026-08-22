using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Units
{
    public sealed class UnitWeaponProfile
    {
        public string UnitId { get; }

        public float PrimaryRange { get; private set; }
        public float SecondaryRange { get; private set; }

        public float PrimaryDamage { get; private set; }
        public float SecondaryDamage { get; private set; }

        public float PrimaryFireRate { get; private set; }
        public float SecondaryFireRate { get; private set; }

        public int PrimaryWeaponSlots { get; private set; }
        public int SecondaryWeaponSlots { get; private set; }

        public bool HasPrimaryWeapon { get; private set; }
        public bool HasSecondaryWeapon { get; private set; }

        public UnitWeaponProfile(string unitId)
        {
            UnitId = unitId ?? string.Empty;
        }

        public void Configure(
            float primaryRange,
            float secondaryRange,
            float primaryDamage,
            float secondaryDamage,
            float primaryFireRate,
            float secondaryFireRate,
            int primaryWeaponSlots,
            int secondaryWeaponSlots,
            bool hasPrimaryWeapon,
            bool hasSecondaryWeapon)
        {
            PrimaryRange = Math.Max(0f, primaryRange);
            SecondaryRange = Math.Max(0f, secondaryRange);

            PrimaryDamage = Math.Max(0f, primaryDamage);
            SecondaryDamage = Math.Max(0f, secondaryDamage);

            PrimaryFireRate =
                Math.Max(0f, primaryFireRate);

            SecondaryFireRate =
                Math.Max(0f, secondaryFireRate);

            PrimaryWeaponSlots =
                Math.Max(0, primaryWeaponSlots);

            SecondaryWeaponSlots =
                Math.Max(0, secondaryWeaponSlots);

            HasPrimaryWeapon = hasPrimaryWeapon;
            HasSecondaryWeapon = hasSecondaryWeapon;
        }
    }

    public sealed class UnitWeaponProfileSystem
    {
        private readonly Dictionary<string, UnitWeaponProfile> profiles =
            new Dictionary<string, UnitWeaponProfile>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!profiles.ContainsKey(unitId))
            {
                profiles.Add(
                    unitId,
                    new UnitWeaponProfile(unitId));
            }
        }

        public void ConfigureUnit(
            string unitId,
            float primaryRange,
            float secondaryRange,
            float primaryDamage,
            float secondaryDamage,
            float primaryFireRate,
            float secondaryFireRate,
            int primaryWeaponSlots,
            int secondaryWeaponSlots,
            bool hasPrimaryWeapon,
            bool hasSecondaryWeapon)
        {
            RegisterUnit(unitId);

            profiles[unitId].Configure(
                primaryRange,
                secondaryRange,
                primaryDamage,
                secondaryDamage,
                primaryFireRate,
                secondaryFireRate,
                primaryWeaponSlots,
                secondaryWeaponSlots,
                hasPrimaryWeapon,
                hasSecondaryWeapon);
        }

        public bool TryGetProfile(
            string unitId,
            out UnitWeaponProfile profile)
        {
            return profiles.TryGetValue(
                unitId,
                out profile);
        }

        public void RemoveUnit(string unitId)
        {
            profiles.Remove(unitId);
        }

        public void Clear()
        {
            profiles.Clear();
        }
    }
}
