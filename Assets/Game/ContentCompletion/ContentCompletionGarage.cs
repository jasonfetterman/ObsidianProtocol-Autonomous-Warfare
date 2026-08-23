using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ContentCompletion
{
    public sealed class ContentCompletionGarage
    {
        private readonly HashSet<string> areas =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int AreaCount =>
            areas.Count;

        public bool Complete =>
            AreaCount > 0;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            areas.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterArea(
            string areaId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(areaId))
            {
                return false;
            }

            return areas.Add(
                areaId.Trim());
        }

        public bool ContainsArea(
            string areaId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(areaId))
            {
                return false;
            }

            return areas.Contains(
                areaId.Trim());
        }

        public IReadOnlyCollection<string>
            GetAreas()
        {
            return areas;
        }

        public void Reset()
        {
            areas.Clear();
            Initialized = false;
        }
    }
}
