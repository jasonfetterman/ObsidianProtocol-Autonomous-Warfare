using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Modding
{
    public sealed class TechnologyConfigurationDefinition
    {
        public string TechnologyId { get; }

        public string TechnologyName { get; }

        public int ResearchCost { get; private set; }

        public bool Enabled { get; private set; }

        public TechnologyConfigurationDefinition(
            string technologyId,
            string technologyName,
            int researchCost)
        {
            TechnologyId =
                technologyId ?? string.Empty;

            TechnologyName =
                technologyName ?? string.Empty;

            ResearchCost =
                Math.Max(0, researchCost);

            Enabled = true;
        }

        public bool SetResearchCost(
            int researchCost)
        {
            ResearchCost =
                Math.Max(0, researchCost);

            return true;
        }

        public bool SetEnabled(
            bool enabled)
        {
            Enabled = enabled;

            return true;
        }
    }

    public sealed class TechnologyConfigurationTools
    {
        private readonly Dictionary<
            string,
            TechnologyConfigurationDefinition> technologies =
            new Dictionary<
                string,
                TechnologyConfigurationDefinition>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int TechnologyCount =>
            technologies.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            technologies.Clear();
            Initialized = true;

            return true;
        }

        public bool CreateTechnology(
            string technologyId,
            string technologyName,
            int researchCost)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(technologyId) ||
                string.IsNullOrWhiteSpace(technologyName))
            {
                return false;
            }

            string id =
                technologyId.Trim();

            if (technologies.ContainsKey(id))
            {
                return false;
            }

            technologies.Add(
                id,
                new TechnologyConfigurationDefinition(
                    id,
                    technologyName.Trim(),
                    researchCost));

            return true;
        }

        public bool ConfigureTechnology(
            string technologyId,
            int researchCost)
        {
            TechnologyConfigurationDefinition technology =
                GetTechnology(technologyId);

            return technology != null &&
                   technology.SetResearchCost(
                       researchCost);
        }

        public bool RemoveTechnology(
            string technologyId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(technologyId))
            {
                return false;
            }

            return technologies.Remove(
                technologyId.Trim());
        }

        public TechnologyConfigurationDefinition GetTechnology(
            string technologyId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(technologyId))
            {
                return null;
            }

            technologies.TryGetValue(
                technologyId.Trim(),
                out TechnologyConfigurationDefinition technology);

            return technology;
        }

        public IReadOnlyCollection<
            TechnologyConfigurationDefinition>
            GetTechnologies()
        {
            return technologies.Values;
        }

        public void Reset()
        {
            technologies.Clear();
            Initialized = false;
        }
    }
}
