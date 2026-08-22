using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ExperimentalSystems
{
    public enum DisruptionType
    {
        Communication,
        Sensor,
        Navigation,
        Targeting,
        Command,
        Network
    }

    public sealed class DisruptionEffect
    {
        public string EffectId { get; }
        public string TargetId { get; }

        public DisruptionType Type { get; }

        public float Strength { get; private set; }
        public float Duration { get; private set; }
        public float RemainingTime { get; private set; }

        public bool Active =>
            RemainingTime > 0f &&
            Strength > 0f;

        public DisruptionEffect(
            string effectId,
            string targetId,
            DisruptionType type,
            float strength,
            float duration)
        {
            EffectId =
                effectId ?? string.Empty;

            TargetId =
                targetId ?? string.Empty;

            Type =
                type;

            Strength =
                Math.Clamp(
                    strength,
                    0f,
                    1f);

            Duration =
                Math.Max(
                    0f,
                    duration);

            RemainingTime =
                Duration;
        }

        public void Refresh(
            float strength,
            float duration)
        {
            Strength =
                Math.Clamp(
                    strength,
                    0f,
                    1f);

            Duration =
                Math.Max(
                    0f,
                    duration);

            RemainingTime =
                Duration;
        }

        public void Update(
            float deltaTime)
        {
            RemainingTime =
                Math.Max(
                    0f,
                    RemainingTime -
                    Math.Max(
                        0f,
                        deltaTime));
        }
    }

    public sealed class ElectronicDisruptionSystem
    {
        private readonly Dictionary<string, DisruptionEffect> effects =
            new Dictionary<string, DisruptionEffect>(
                StringComparer.OrdinalIgnoreCase);

        public void ApplyDisruption(
            string effectId,
            string targetId,
            DisruptionType type,
            float strength,
            float duration)
        {
            if (string.IsNullOrWhiteSpace(effectId) ||
                string.IsNullOrWhiteSpace(targetId))
            {
                return;
            }

            if (effects.TryGetValue(
                    effectId,
                    out DisruptionEffect effect))
            {
                effect.Refresh(
                    strength,
                    duration);

                return;
            }

            effects.Add(
                effectId,
                new DisruptionEffect(
                    effectId,
                    targetId,
                    type,
                    strength,
                    duration));
        }

        public void Update(
            float deltaTime)
        {
            List<string> expired =
                new List<string>();

            foreach (KeyValuePair<string, DisruptionEffect> entry
                in effects)
            {
                entry.Value.Update(
                    deltaTime);

                if (!entry.Value.Active)
                {
                    expired.Add(
                        entry.Key);
                }
            }

            foreach (string effectId in expired)
            {
                effects.Remove(
                    effectId);
            }
        }

        public float GetDisruptionStrength(
            string targetId,
            DisruptionType type)
        {
            float strongestEffect = 0f;

            foreach (DisruptionEffect effect
                in effects.Values)
            {
                if (!effect.Active ||
                    !string.Equals(
                        effect.TargetId,
                        targetId,
                        StringComparison.OrdinalIgnoreCase) ||
                    effect.Type != type)
                {
                    continue;
                }

                strongestEffect =
                    Math.Max(
                        strongestEffect,
                        effect.Strength);
            }

            return strongestEffect;
        }

        public bool IsDisrupted(
            string targetId,
            DisruptionType type)
        {
            return GetDisruptionStrength(
                       targetId,
                       type) > 0f;
        }

        public bool TryGetEffect(
            string effectId,
            out DisruptionEffect effect)
        {
            return effects.TryGetValue(
                effectId,
                out effect);
        }

        public void RemoveEffect(
            string effectId)
        {
            effects.Remove(
                effectId);
        }

        public void Clear()
        {
            effects.Clear();
        }
    }
}
