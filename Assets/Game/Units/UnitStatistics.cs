using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Units
{
    public sealed class UnitStatistics
    {
        public string UnitId { get; }

        public float MaxHealth { get; private set; }
        public float Armor { get; private set; }
        public float Mobility { get; private set; }
        public float SensorRange { get; private set; }
        public float Detection { get; private set; }
        public float Accuracy { get; private set; }
        public float Damage { get; private set; }
        public float FireRate { get; private set; }
        public float CommandCapacity { get; private set; }
        public float EnergyCapacity { get; private set; }

        public UnitStatistics(string unitId)
        {
            UnitId = unitId ?? string.Empty;
        }

        public void Configure(
            float maxHealth,
            float armor,
            float mobility,
            float sensorRange,
            float detection,
            float accuracy,
            float damage,
            float fireRate,
            float commandCapacity,
            float energyCapacity)
        {
            MaxHealth = Math.Max(0f, maxHealth);
            Armor = Math.Max(0f, armor);
            Mobility = Math.Max(0f, mobility);
            SensorRange = Math.Max(0f, sensorRange);
            Detection = Math.Max(0f, detection);
            Accuracy = Math.Max(0f, accuracy);
            Damage = Math.Max(0f, damage);
            FireRate = Math.Max(0f, fireRate);
            CommandCapacity = Math.Max(0f, commandCapacity);
            EnergyCapacity = Math.Max(0f, energyCapacity);
        }

        public float GetHealthRatio(float currentHealth)
        {
            if (MaxHealth <= 0f)
            {
                return 0f;
            }

            return Math.Clamp(
                currentHealth / MaxHealth,
                0f,
                1f);
        }
    }

    public sealed class UnitStatisticsSystem
    {
        private readonly Dictionary<string, UnitStatistics> statistics =
            new Dictionary<string, UnitStatistics>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!statistics.ContainsKey(unitId))
            {
                statistics.Add(
                    unitId,
                    new UnitStatistics(unitId));
            }
        }

        public void ConfigureUnit(
            string unitId,
            float maxHealth,
            float armor,
            float mobility,
            float sensorRange,
            float detection,
            float accuracy,
            float damage,
            float fireRate,
            float commandCapacity,
            float energyCapacity)
        {
            RegisterUnit(unitId);

            statistics[unitId].Configure(
                maxHealth,
                armor,
                mobility,
                sensorRange,
                detection,
                accuracy,
                damage,
                fireRate,
                commandCapacity,
                energyCapacity);
        }

        public bool TryGetStatistics(
            string unitId,
            out UnitStatistics unitStatistics)
        {
            return statistics.TryGetValue(
                unitId,
                out unitStatistics);
        }

        public void RemoveUnit(string unitId)
        {
            statistics.Remove(unitId);
        }

        public void Clear()
        {
            statistics.Clear();
        }
    }
}
