using System.Collections.Generic;

namespace ObsidianProtocol.Game.StatusEffects
{
    public sealed class StatusEffectCollection
    {
        private readonly HashSet<StatusEffectDefinition> activeEffects = new();

        public IReadOnlyCollection<StatusEffectDefinition> ActiveEffects => activeEffects;

        public bool Add(StatusEffectDefinition effect)
        {
            if (effect == null)
            {
                return false;
            }

            return activeEffects.Add(effect);
        }

        public bool Remove(StatusEffectDefinition effect)
        {
            if (effect == null)
            {
                return false;
            }

            return activeEffects.Remove(effect);
        }

        public bool Contains(StatusEffectDefinition effect)
        {
            return effect != null && activeEffects.Contains(effect);
        }

        public void Clear()
        {
            activeEffects.Clear();
        }
    }
}
