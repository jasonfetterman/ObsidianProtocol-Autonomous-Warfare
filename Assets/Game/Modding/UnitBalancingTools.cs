using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Modding
{
    public sealed class UnitBalanceDefinition
    {
        public string UnitId { get; }

        public float Health { get; private set; }

        public float Damage { get; private set; }

        public float Speed { get; private set; }

        public float DeploymentCost { get; private set; }

        public UnitBalanceDefinition(
            string unitId)
        {
            UnitId =
                unitId ?? string.Empty;

            Health = 100f;
            Damage = 10f;
            Speed = 1f;
            DeploymentCost = 1f;
        }

        public bool Configure(
            float health,
            float damage,
            float speed,
            float deploymentCost)
        {
            Health =
                Math.Max(0f, health);

            Damage =
                Math.Max(0f, damage);

            Speed =
                Math.Max(0f, speed);

            DeploymentCost =
                Math.Max(0f, deploymentCost);

            return true;
        }
    }

    public sealed class UnitBalancingTools
    {
        private readonly Dictionary<
            string,
            UnitBalanceDefinition> units =
            new Dictionary<
                string,
                UnitBalanceDefinition>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int UnitCount =>
            units.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            units.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            string id =
                unitId.Trim();

            if (units.ContainsKey(id))
            {
                return false;
            }

            units.Add(
                id,
                new UnitBalanceDefinition(id));

            return true;
        }

        public bool ConfigureUnit(
            string unitId,
            float health,
            float damage,
            float speed,
            float deploymentCost)
        {
            UnitBalanceDefinition unit =
                GetUnit(unitId);

            return unit != null &&
                   unit.Configure(
                       health,
                       damage,
                       speed,
                       deploymentCost);
        }

        public UnitBalanceDefinition GetUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return null;
            }

            units.TryGetValue(
                unitId.Trim(),
                out UnitBalanceDefinition unit);

            return unit;
        }

        public bool RemoveUnit(
            string unitId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return units.Remove(
                unitId.Trim());
        }

        public IReadOnlyCollection<
            UnitBalanceDefinition>
            GetUnits()
        {
            return units.Values;
        }

        public void Reset()
        {
            units.Clear();
            Initialized = false;
        }
    }
}
