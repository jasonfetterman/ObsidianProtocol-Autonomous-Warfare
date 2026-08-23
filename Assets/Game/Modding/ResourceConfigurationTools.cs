using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Modding
{
    public sealed class ResourceConfigurationDefinition
    {
        public string ResourceId { get; }

        public string ResourceName { get; }

        public int BaseValue { get; private set; }

        public bool Enabled { get; private set; }

        public ResourceConfigurationDefinition(
            string resourceId,
            string resourceName,
            int baseValue)
        {
            ResourceId =
                resourceId ?? string.Empty;

            ResourceName =
                resourceName ?? string.Empty;

            BaseValue =
                Math.Max(0, baseValue);

            Enabled = true;
        }

        public bool SetBaseValue(
            int baseValue)
        {
            BaseValue =
                Math.Max(0, baseValue);

            return true;
        }

        public bool SetEnabled(
            bool enabled)
        {
            Enabled = enabled;

            return true;
        }
    }

    public sealed class ResourceConfigurationTools
    {
        private readonly Dictionary<
            string,
            ResourceConfigurationDefinition> resources =
            new Dictionary<
                string,
                ResourceConfigurationDefinition>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int ResourceCount =>
            resources.Count;

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

        public bool CreateResource(
            string resourceId,
            string resourceName,
            int baseValue)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(resourceId) ||
                string.IsNullOrWhiteSpace(resourceName))
            {
                return false;
            }

            string id =
                resourceId.Trim();

            if (resources.ContainsKey(id))
            {
                return false;
            }

            resources.Add(
                id,
                new ResourceConfigurationDefinition(
                    id,
                    resourceName.Trim(),
                    baseValue));

            return true;
        }

        public bool ConfigureResource(
            string resourceId,
            int baseValue)
        {
            ResourceConfigurationDefinition resource =
                GetResource(resourceId);

            return resource != null &&
                   resource.SetBaseValue(
                       baseValue);
        }

        public bool RemoveResource(
            string resourceId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(resourceId))
            {
                return false;
            }

            return resources.Remove(
                resourceId.Trim());
        }

        public ResourceConfigurationDefinition GetResource(
            string resourceId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(resourceId))
            {
                return null;
            }

            resources.TryGetValue(
                resourceId.Trim(),
                out ResourceConfigurationDefinition resource);

            return resource;
        }

        public IReadOnlyCollection<
            ResourceConfigurationDefinition>
            GetResources()
        {
            return resources.Values;
        }

        public void Reset()
        {
            resources.Clear();
            Initialized = false;
        }
    }
}
