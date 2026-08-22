using System.Collections.Generic;

namespace ObsidianProtocol.Game.Abilities
{
    public sealed class AbilityCollection
    {
        private readonly List<AbilityDefinition> abilities = new();

        public IReadOnlyList<AbilityDefinition> Abilities => abilities;

        public bool Add(AbilityDefinition ability)
        {
            if (ability == null || abilities.Contains(ability))
            {
                return false;
            }

            abilities.Add(ability);
            return true;
        }

        public bool Remove(AbilityDefinition ability)
        {
            if (ability == null)
            {
                return false;
            }

            return abilities.Remove(ability);
        }

        public void Clear()
        {
            abilities.Clear();
        }
    }
}
