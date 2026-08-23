using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Polish
{
    public sealed class UnitAnimationPolish
    {
        private readonly Dictionary<
            string,
            float> animationSpeeds =
            new Dictionary<
                string,
                float>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int AnimationCount =>
            animationSpeeds.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            animationSpeeds.Clear();

            SetDefault("Idle", 1f);
            SetDefault("Move", 1f);
            SetDefault("Attack", 1f);
            SetDefault("Damage", 1f);
            SetDefault("Destroyed", 1f);
            SetDefault("Deploy", 1f);
            SetDefault("Repair", 1f);

            Initialized = true;

            return true;
        }

        public bool SetAnimationSpeed(
            string animationId,
            float speed)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(animationId))
            {
                return false;
            }

            animationSpeeds[animationId.Trim()] =
                Math.Max(0f, speed);

            return true;
        }

        public float GetAnimationSpeed(
            string animationId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(animationId))
            {
                return 0f;
            }

            animationSpeeds.TryGetValue(
                animationId.Trim(),
                out float speed);

            return speed;
        }

        private void SetDefault(
            string key,
            float speed)
        {
            animationSpeeds[key] = speed;
        }

        public void Reset()
        {
            animationSpeeds.Clear();
            Initialized = false;
        }
    }
}
