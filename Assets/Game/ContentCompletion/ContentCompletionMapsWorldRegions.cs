using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ContentCompletion
{
    public sealed class ContentCompletionMapsWorldRegions
    {
        private readonly HashSet<string> regions =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int RegionCount =>
            regions.Count;

        public bool Complete =>
            RegionCount > 0;

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
            string regionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(regionId))
            {
                return false;
            }

            return regions.Add(
                regionId.Trim());
        }

        public bool ContainsRegion(
            string regionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(regionId))
            {
                return false;
            }

            return regions.Contains(
                regionId.Trim());
        }

        public IReadOnlyCollection<string>
            GetRegions()
        {
            return regions;
        }

        public void Reset()
        {
            regions.Clear();
            Initialized = false;
        }
    }
}
