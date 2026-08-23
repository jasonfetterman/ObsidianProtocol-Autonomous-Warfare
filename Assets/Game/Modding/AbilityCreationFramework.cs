using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Modding
{
    public sealed class AbilityCreationDefinition
    {
        public string AbilityId { get; }

        public string AbilityName { get; }

        public string AbilityType { get; }

        public bool Enabled { get; private set; }

        public AbilityCreationDefinition(
            string abilityId,
            string abilityName,
            string abilityType)
        {
            AbilityId =
                abilityId ?? string.Empty;

            AbilityName =
                abilityName ?? string.Empty;

            AbilityType =
                abilityType ?? string.Empty;

            Enabled = true;
        }

        public bool SetEnabled(
            bool enabled)
        {
            Enabled = enabled;

            return true;
        }
    }

    public sealed class AbilityCreationFramework
    {
        private readonly Dictionary<
            string,
            AbilityCreationDefinition> definitions =
            new Dictionary<
                string,
                AbilityCreationDefinition>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int AbilityDefinitionCount =>
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

        public bool CreateAbility(
            string abilityId,
            string abilityName,
            string abilityType)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(abilityId) ||
                string.IsNullOrWhiteSpace(abilityName) ||
                string.IsNullOrWhiteSpace(abilityType))
            {
                return false;
            }

            string id =
                abilityId.Trim();

            if (definitions.ContainsKey(id))
            {
                return false;
            }

            definitions.Add(
                id,
                new AbilityCreationDefinition(
                    id,
                    abilityName.Trim(),
                    abilityType.Trim()));

            return true;
        }

        public bool RemoveAbility(
            string abilityId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(abilityId))
            {
                return false;
            }

            return definitions.Remove(
                abilityId.Trim());
        }

        public AbilityCreationDefinition GetAbility(
            string abilityId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(abilityId))
            {
                return null;
            }

            definitions.TryGetValue(
                abilityId.Trim(),
                out AbilityCreationDefinition definition);

            return definition;
        }

        public IReadOnlyCollection<
            AbilityCreationDefinition>
            GetAbilities()
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
