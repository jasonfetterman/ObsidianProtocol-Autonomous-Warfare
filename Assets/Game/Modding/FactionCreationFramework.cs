using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Modding
{
    public sealed class FactionCreationDefinition
    {
        public string FactionId { get; }

        public string FactionName { get; }

        public string Description { get; }

        public bool Enabled { get; private set; }

        public FactionCreationDefinition(
            string factionId,
            string factionName,
            string description)
        {
            FactionId =
                factionId ?? string.Empty;

            FactionName =
                factionName ?? string.Empty;

            Description =
                description ?? string.Empty;

            Enabled = true;
        }

        public bool SetEnabled(
            bool enabled)
        {
            Enabled = enabled;

            return true;
        }
    }

    public sealed class FactionCreationFramework
    {
        private readonly Dictionary<
            string,
            FactionCreationDefinition> definitions =
            new Dictionary<
                string,
                FactionCreationDefinition>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int FactionDefinitionCount =>
            definitions.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            definitions.Clear();
            Initialized = true;

            return true;
        }

        public bool CreateFaction(
            string factionId,
            string factionName,
            string description)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(factionId) ||
                string.IsNullOrWhiteSpace(factionName))
            {
                return false;
            }

            string id =
                factionId.Trim();

            if (definitions.ContainsKey(id))
            {
                return false;
            }

            definitions.Add(
                id,
                new FactionCreationDefinition(
                    id,
                    factionName.Trim(),
                    description ?? string.Empty));

            return true;
        }

        public bool RemoveFaction(
            string factionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(factionId))
            {
                return false;
            }

            return definitions.Remove(
                factionId.Trim());
        }

        public FactionCreationDefinition GetFaction(
            string factionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(factionId))
            {
                return null;
            }

            definitions.TryGetValue(
                factionId.Trim(),
                out FactionCreationDefinition definition);

            return definition;
        }

        public IReadOnlyCollection<
            FactionCreationDefinition>
            GetFactions()
        {
            return definitions.Values;
        }

        public void Reset()
        {
            definitions.Clear();
            Initialized = false;
        }
    }
}
