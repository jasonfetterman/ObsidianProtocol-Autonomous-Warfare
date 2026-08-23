using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Battlefield
{
    public enum BuildingDamageState
    {
        Operational,
        Damaged,
        Critical,
        Collapsed
    }

    public sealed class BuildingDamageProfile
    {
        public string BuildingId { get; }

        public float MaximumIntegrity { get; }

        public float Integrity { get; private set; }

        public BuildingDamageState State { get; private set; }

        public bool SupportsStructuralDamage { get; }

        public BuildingDamageProfile(
            string buildingId,
            float maximumIntegrity,
            bool supportsStructuralDamage)
        {
            BuildingId =
                buildingId ?? string.Empty;

            MaximumIntegrity =
                Math.Max(0f, maximumIntegrity);

            Integrity =
                MaximumIntegrity;

            SupportsStructuralDamage =
                supportsStructuralDamage;

            State =
                MaximumIntegrity > 0f
                    ? BuildingDamageState.Operational
                    : BuildingDamageState.Collapsed;
        }

        public bool ApplyDamage(
            float damage)
        {
            if (damage < 0f ||
                State == BuildingDamageState.Collapsed)
            {
                return false;
            }

            Integrity =
                Math.Max(
                    0f,
                    Integrity - damage);

            UpdateState();

            return true;
        }

        public bool Repair(
            float amount)
        {
            if (amount < 0f ||
                State == BuildingDamageState.Collapsed ||
                MaximumIntegrity <= 0f)
            {
                return false;
            }

            Integrity =
                Math.Min(
                    MaximumIntegrity,
                    Integrity + amount);

            UpdateState();

            return true;
        }

        private void UpdateState()
        {
            if (Integrity <= 0f)
            {
                State =
                    BuildingDamageState.Collapsed;
            }
            else
            {
                float ratio =
                    Integrity / MaximumIntegrity;

                if (ratio <= 0.25f)
                {
                    State =
                        BuildingDamageState.Critical;
                }
                else if (ratio < 1f)
                {
                    State =
                        BuildingDamageState.Damaged;
                }
                else
                {
                    State =
                        BuildingDamageState.Operational;
                }
            }
        }
    }

    public sealed class BuildingDamage
    {
        private readonly Dictionary<
            string,
            BuildingDamageProfile> buildings =
            new Dictionary<
                string,
                BuildingDamageProfile>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int BuildingCount =>
            buildings.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            buildings.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterBuilding(
            string buildingId,
            float maximumIntegrity,
            bool supportsStructuralDamage)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(buildingId) ||
                maximumIntegrity <= 0f)
            {
                return false;
            }

            string id =
                buildingId.Trim();

            if (buildings.ContainsKey(id))
            {
                return false;
            }

            buildings.Add(
                id,
                new BuildingDamageProfile(
                    id,
                    maximumIntegrity,
                    supportsStructuralDamage));

            return true;
        }

        public bool ApplyDamage(
            string buildingId,
            float damage)
        {
            BuildingDamageProfile building =
                GetBuilding(buildingId);

            return building != null &&
                   building.ApplyDamage(damage);
        }

        public bool RepairBuilding(
            string buildingId,
            float amount)
        {
            BuildingDamageProfile building =
                GetBuilding(buildingId);

            return building != null &&
                   building.Repair(amount);
        }

        public BuildingDamageProfile GetBuilding(
            string buildingId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(buildingId))
            {
                return null;
            }

            buildings.TryGetValue(
                buildingId.Trim(),
                out BuildingDamageProfile building);

            return building;
        }

        public IReadOnlyCollection<
            BuildingDamageProfile>
            GetBuildings()
        {
            return buildings.Values;
        }

        public void Reset()
        {
            buildings.Clear();

            Initialized = false;
        }
    }
}
