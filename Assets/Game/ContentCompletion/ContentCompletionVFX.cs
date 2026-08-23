using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ContentCompletion
{
    public sealed class ContentCompletionVFX
    {
        private readonly HashSet<string> effects =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int EffectCount =>
            effects.Count;

        public bool Complete =>
            EffectCount > 0;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            effects.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterEffect(
            string effectId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(effectId))
            {
                return false;
            }

            return effects.Add(
                effectId.Trim());
        }

        public bool ContainsEffect(
            string effectId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(effectId))
            {
                return false;
            }

            return effects.Contains(
                effectId.Trim());
        }

        public IReadOnlyCollection<string>
            GetEffects()
        {
            return effects;
        }

        public void Reset()
        {
            effects.Clear();
            Initialized = false;
        }
    }
}
