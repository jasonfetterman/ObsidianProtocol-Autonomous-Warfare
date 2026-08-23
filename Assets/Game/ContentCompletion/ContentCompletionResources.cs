using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ContentCompletion
{
    public sealed class ContentCompletionResources
    {
        private readonly HashSet<string> resources =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ResourceCount =>
            resources.Count;

        public bool Complete =>
            ResourceCount > 0;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            resources.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterResource(
            string resourceId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(resourceId))
            {
                return false;
            }

            return resources.Add(
                resourceId.Trim());
        }

        public bool ContainsResource(
            string resourceId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(resourceId))
            {
                return false;
            }

            return resources.Contains(
                resourceId.Trim());
        }

        public IReadOnlyCollection<string>
            GetResources()
        {
            return resources;
        }

        public void Reset()
        {
            resources.Clear();
            Initialized = false;
        }
    }
}
