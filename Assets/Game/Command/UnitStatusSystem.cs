using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Command
{
    public sealed class UnitStatus
    {
        public string UnitId { get; }

        public float Health { get; private set; }
        public float MaxHealth { get; private set; }

        public float Energy { get; private set; }
        public float MaxEnergy { get; private set; }

        public bool Operational { get; private set; }
        public bool Selected { get; private set; }

        public string CurrentOrder { get; private set; }
        public string CurrentIntent { get; private set; }

        public UnitStatus(
            string unitId,
            float maxHealth,
            float maxEnergy)
        {
            UnitId = unitId ?? string.Empty;

            MaxHealth = Math.Max(0f, maxHealth);
            Health = MaxHealth;

            MaxEnergy = Math.Max(0f, maxEnergy);
            Energy = MaxEnergy;

            Operational = true;
            Selected = false;

            CurrentOrder = string.Empty;
            CurrentIntent = string.Empty;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(UnitId);

        public void SetHealth(float value)
        {
            Health =
                Math.Max(
                    0f,
                    Math.Min(value, MaxHealth));

            Operational = Health > 0f;
        }

        public void SetEnergy(float value)
        {
            Energy =
                Math.Max(
                    0f,
                    Math.Min(value, MaxEnergy));
        }

        public void SetSelected(bool selected)
        {
            Selected = selected;
        }

        public void SetOrder(string order)
        {
            CurrentOrder =
                order ?? string.Empty;
        }

        public void SetIntent(string intent)
        {
            CurrentIntent =
                intent ?? string.Empty;
        }

        public void SetOperational(bool operational)
        {
            Operational = operational;
        }

        public void Reset()
        {
            Health = MaxHealth;
            Energy = MaxEnergy;

            Operational = true;
            Selected = false;

            CurrentOrder = string.Empty;
            CurrentIntent = string.Empty;
        }
    }

    public sealed class UnitStatusSystem
    {
        private readonly Dictionary<string, UnitStatus> units =
            new Dictionary<string, UnitStatus>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(UnitStatus unit)
        {
            if (unit == null ||
                !unit.Valid ||
                units.ContainsKey(unit.UnitId))
            {
                return false;
            }

            units.Add(unit.UnitId, unit);
            return true;
        }

        public bool Remove(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
                return false;

            return units.Remove(unitId);
        }

        public bool TryGet(
            string unitId,
            out UnitStatus unit)
        {
            return units.TryGetValue(
                unitId,
                out unit);
        }

        public bool SetSelected(
            string unitId,
            bool selected)
        {
            if (!units.TryGetValue(
                    unitId,
                    out UnitStatus unit))
            {
                return false;
            }

            unit.SetSelected(selected);
            return true;
        }

        public IReadOnlyCollection<UnitStatus>
            GetUnits()
        {
            return units.Values;
        }

        public void Clear()
        {
            units.Clear();
        }
    }
}
