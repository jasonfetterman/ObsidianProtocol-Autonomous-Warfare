using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ContentCompletion
{
    public sealed class ContentCompletionAbilities
    {
        private readonly HashSet<string> abilities =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int AbilityCount =>
            abilities.Count;

        public bool Complete =>
            AbilityCount > 0;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            abilities.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterAbility(
            string abilityId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(abilityId))
            {
                return false;
            }

            return abilities.Add(
                abilityId.Trim());
        }

        public bool ContainsAbility(
            string abilityId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(abilityId))
            {
                return false;
            }

            return abilities.Contains(
                abilityId.Trim());
        }

        public IReadOnlyCollection<string>
            GetAbilities()
        {
            return abilities;
        }

        public void Reset()
        {
            abilities.Clear();
            Initialized = false;
        }
    }
}
