using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VR
{
    public sealed class OperatorWeapon
    {
        public string WeaponId { get; }

        public string TargetId { get; private set; }

        public float Cooldown { get; }

        public float CooldownRemaining { get; private set; }

        public bool Armed { get; private set; }

        public OperatorWeapon(
            string weaponId,
            float cooldown)
        {
            WeaponId =
                weaponId ?? string.Empty;

            TargetId =
                string.Empty;

            Cooldown =
                Math.Max(0f, cooldown);

            CooldownRemaining = 0f;
            Armed = false;
        }

        public bool SetArmed(
            bool armed)
        {
            Armed = armed;

            return true;
        }

        public bool SetTarget(
            string targetId)
        {
            if (string.IsNullOrWhiteSpace(targetId))
            {
                TargetId =
                    string.Empty;

                return true;
            }

            TargetId =
                targetId.Trim();

            return true;
        }

        public bool CanFire()
        {
            return Armed &&
                   CooldownRemaining <= 0f;
        }

        public bool Fire()
        {
            if (!CanFire())
            {
                return false;
            }

            CooldownRemaining =
                Cooldown;

            return true;
        }

        public void Update(
            float deltaTime)
        {
            if (deltaTime <= 0f ||
                CooldownRemaining <= 0f)
            {
                return;
            }

            CooldownRemaining =
                Math.Max(
                    0f,
                    CooldownRemaining - deltaTime);
        }

        public void Reset()
        {
            TargetId =
                string.Empty;

            CooldownRemaining = 0f;
            Armed = false;
        }
    }

    public sealed class OperatorWeapons
    {
        private readonly Dictionary<
            string,
            OperatorWeapon> weapons =
            new Dictionary<
                string,
                OperatorWeapon>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public bool Active { get; private set; }

        public string UnitId { get; private set; }

        public int WeaponCount =>
            weapons.Count;

        public bool Initialize(
            string unitId)
        {
            if (Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            UnitId =
                unitId.Trim();

            weapons.Clear();

            Active = false;
            Initialized = true;

            return true;
        }

        public bool RegisterWeapon(
            string weaponId,
            float cooldown)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(weaponId))
            {
                return false;
            }

            string id =
                weaponId.Trim();

            if (weapons.ContainsKey(id))
            {
                return false;
            }

            weapons.Add(
                id,
                new OperatorWeapon(
                    id,
                    cooldown));

            return true;
        }

        public bool Activate()
        {
            if (!Initialized)
            {
                return false;
            }

            Active = true;

            return true;
        }

        public bool Deactivate()
        {
            if (!Initialized)
            {
                return false;
            }

            Active = false;

            foreach (OperatorWeapon weapon
                     in weapons.Values)
            {
                weapon.Reset();
            }

            return true;
        }

        public bool ArmWeapon(
            string weaponId,
            bool armed)
        {
            if (!Initialized ||
                !Active)
            {
                return false;
            }

            OperatorWeapon weapon =
                GetWeapon(weaponId);

            return weapon != null &&
                   weapon.SetArmed(armed);
        }

        public bool SetWeaponTarget(
            string weaponId,
            string targetId)
        {
            if (!Initialized ||
                !Active)
            {
                return false;
            }

            OperatorWeapon weapon =
                GetWeapon(weaponId);

            return weapon != null &&
                   weapon.SetTarget(targetId);
        }

        public bool FireWeapon(
            string weaponId)
        {
            if (!Initialized ||
                !Active)
            {
                return false;
            }

            OperatorWeapon weapon =
                GetWeapon(weaponId);

            return weapon != null &&
                   weapon.Fire();
        }

        public void Update(
            float deltaTime)
        {
            if (!Initialized ||
                !Active)
            {
                return;
            }

            foreach (OperatorWeapon weapon
                     in weapons.Values)
            {
                weapon.Update(deltaTime);
            }
        }

        public OperatorWeapon GetWeapon(
            string weaponId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(weaponId))
            {
                return null;
            }

            weapons.TryGetValue(
                weaponId.Trim(),
                out OperatorWeapon weapon);

            return weapon;
        }

        public IReadOnlyCollection<OperatorWeapon>
            GetWeapons()
        {
            return weapons.Values;
        }

        public void Reset()
        {
            weapons.Clear();

            Initialized = false;
            Active = false;

            UnitId =
                string.Empty;
        }
    }
}
