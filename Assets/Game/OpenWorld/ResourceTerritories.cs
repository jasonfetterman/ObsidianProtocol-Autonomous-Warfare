using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OpenWorld
{
    public enum ResourceTerritoryType
    {
        Meat,
        Wood,
        Coal,
        Iron,
        Alloy,
        Electronics,
        Fuel,
        Energy
    }

    public sealed class ResourceTerritoryRecord
    {
        public string TerritoryId { get; }

        public string RegionId { get; }

        public ResourceTerritoryType ResourceType { get; }

        public float ProductionRate { get; private set; }

        public bool Active { get; private set; }

        public ResourceTerritoryRecord(
            string territoryId,
            string regionId,
            ResourceTerritoryType resourceType,
            float productionRate)
        {
            TerritoryId =
                territoryId ?? string.Empty;

            RegionId =
                regionId ?? string.Empty;

            ResourceType = resourceType;

            ProductionRate =
                productionRate >= 0f
                    ? productionRate
                    : 0f;

            Active = true;
        }

        public bool SetProductionRate(
            float productionRate)
        {
            if (productionRate < 0f)
            {
                return false;
            }

            ProductionRate =
                productionRate;

            return true;
        }

        public void SetActive(
            bool active)
        {
            Active = active;
        }
    }

    public sealed class ResourceTerritories
    {
        private readonly Dictionary<
            string,
            ResourceTerritoryRecord> territories =
            new Dictionary<
                string,
                ResourceTerritoryRecord>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int TerritoryCount =>
            territories.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            territories.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterTerritory(
            string territoryId,
            string regionId,
            ResourceTerritoryType resourceType,
            float productionRate)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(territoryId) ||
                string.IsNullOrWhiteSpace(regionId) ||
                productionRate < 0f)
            {
                return false;
            }

            string id =
                territoryId.Trim();

            if (territories.ContainsKey(id))
            {
                return false;
            }

            territories.Add(
                id,
                new ResourceTerritoryRecord(
                    id,
                    regionId.Trim(),
                    resourceType,
                    productionRate));

            return true;
        }

        public bool SetProductionRate(
            string territoryId,
            float productionRate)
        {
            ResourceTerritoryRecord record =
                GetTerritory(territoryId);

            return record != null &&
                   record.SetProductionRate(
                       productionRate);
        }

        public bool SetActive(
            string territoryId,
            bool active)
        {
            ResourceTerritoryRecord record =
                GetTerritory(territoryId);

            if (record == null)
            {
                return false;
            }

            record.SetActive(active);

            return true;
        }

        public ResourceTerritoryRecord GetTerritory(
            string territoryId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(territoryId))
            {
                return null;
            }

            territories.TryGetValue(
                territoryId.Trim(),
                out ResourceTerritoryRecord record);

            return record;
        }

        public IReadOnlyCollection<
            ResourceTerritoryRecord>
            GetTerritories()
        {
            return territories.Values;
        }

        public void Reset()
        {
            territories.Clear();
            Initialized = false;
        }
    }
}
