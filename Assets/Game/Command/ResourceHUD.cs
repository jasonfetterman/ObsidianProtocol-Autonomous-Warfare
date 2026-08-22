using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Command
{
    public sealed class ResourceHUD
    {
        private readonly Dictionary<string, int> resources =
            new Dictionary<string, int>(
                StringComparer.OrdinalIgnoreCase);

        public bool Visible { get; private set; }

        public void Show()
        {
            Visible = true;
        }

        public void Hide()
        {
            Visible = false;
        }

        public void SetResource(
            string resourceId,
            int amount)
        {
            if (string.IsNullOrWhiteSpace(resourceId))
                return;

            resources[resourceId] =
                Math.Max(0, amount);
        }

        public bool TryGetResource(
            string resourceId,
            out int amount)
        {
            return resources.TryGetValue(
                resourceId,
                out amount);
        }

        public bool RemoveResource(
            string resourceId)
        {
            if (string.IsNullOrWhiteSpace(resourceId))
                return false;

            return resources.Remove(resourceId);
        }

        public IReadOnlyDictionary<string, int>
            GetResources()
        {
            return resources;
        }

        public void Clear()
        {
            resources.Clear();
        }

        public void Reset()
        {
            Visible = false;
            resources.Clear();
        }
    }
}
