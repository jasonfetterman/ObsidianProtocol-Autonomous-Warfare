using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OpenWorld
{
    public enum StrategicRegionType
    {
        Unknown,
        Industrial,
        Agricultural,
        Energy,
        Military,
        Urban,
        Coastal,
        Frontier
    }

    public sealed class StrategicRegionRecord
    {
        public string RegionId { get; }

        public StrategicRegionType Type { get; private set; }

        public float StrategicValue { get; private set; }

        public bool Active { get; private set; }

        public StrategicRegionRecord(
            string regionId,
            StrategicRegionType type,
            float strategicValue)
        {
            RegionId =
                regionId ?? string.Empty;

            Type = type;
            StrategicValue = strategicValue;
            Active = true;
        }

        public bool Update(
            StrategicRegionType type,
            float strategicValue)
        {
            if (string.IsNullOrWhiteSpace(RegionId) ||
                strategicValue < 0f)
            {
                return false;
            }

            Type = type;
            StrategicValue = strategicValue;

            return true;
        }

        public void SetActive(
            bool active)
        {
            Active = active;
        }
    }

    public sealed class StrategicRegions
    {
        private readonly Dictionary<
            string,
            StrategicRegionRecord> regions =
            new Dictionary<
                string,
                StrategicRegionRecord>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int RegionCount =>
            regions.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            regions.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterRegion(
            string regionId,
            StrategicRegionType type,
            float strategicValue)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(regionId) ||
                strategicValue < 0f)
            {
                return false;
            }

            string id =
                regionId.Trim();

            if (regions.ContainsKey(id))
            {
                return false;
            }

            regions.Add(
                id,
                new StrategicRegionRecord(
                    id,
                    type,
                    strategicValue));

            return true;
        }

        public bool UpdateRegion(
            string regionId,
            StrategicRegionType type,
            float strategicValue)
        {
            StrategicRegionRecord region =
                GetRegion(regionId);

            return region != null &&
                   region.Update(
                       type,
                       strategicValue);
        }

        public bool SetRegionActive(
            string regionId,
            bool active)
        {
            StrategicRegionRecord region =
                GetRegion(regionId);

            if (region == null)
            {
                return false;
            }

            region.SetActive(active);

            return true;
        }

        public StrategicRegionRecord GetRegion(
            string regionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(regionId))
            {
                return null;
            }

            regions.TryGetValue(
                regionId.Trim(),
                out StrategicRegionRecord region);

            return region;
        }

        public IReadOnlyCollection<
            StrategicRegionRecord>
            GetRegions()
        {
            return regions.Values;
        }

        public void Reset()
        {
            regions.Clear();
            Initialized = false;
        }
    }
}
