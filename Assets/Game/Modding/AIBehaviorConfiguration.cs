using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Modding
{
    public sealed class AIBehaviorDefinition
    {
        public string BehaviorId { get; }

        public string BehaviorName { get; }

        public float Aggression { get; private set; }

        public float Caution { get; private set; }

        public float Adaptability { get; private set; }

        public bool Enabled { get; private set; }

        public AIBehaviorDefinition(
            string behaviorId,
            string behaviorName)
        {
            BehaviorId =
                behaviorId ?? string.Empty;

            BehaviorName =
                behaviorName ?? string.Empty;

            Aggression = 0.5f;
            Caution = 0.5f;
            Adaptability = 0.5f;
            Enabled = true;
        }

        public bool Configure(
            float aggression,
            float caution,
            float adaptability)
        {
            Aggression =
                Clamp(aggression);

            Caution =
                Clamp(caution);

            Adaptability =
                Clamp(adaptability);

            return true;
        }

        public bool SetEnabled(
            bool enabled)
        {
            Enabled = enabled;

            return true;
        }

        private static float Clamp(
            float value)
        {
            if (value < 0f)
            {
                return 0f;
            }

            if (value > 1f)
            {
                return 1f;
            }

            return value;
        }
    }

    public sealed class AIBehaviorConfiguration
    {
        private readonly Dictionary<
            string,
            AIBehaviorDefinition> behaviors =
            new Dictionary<
                string,
                AIBehaviorDefinition>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int BehaviorCount =>
            behaviors.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            behaviors.Clear();
            Initialized = true;

            return true;
        }

        public bool CreateBehavior(
            string behaviorId,
            string behaviorName)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(behaviorId) ||
                string.IsNullOrWhiteSpace(behaviorName))
            {
                return false;
            }

            string id =
                behaviorId.Trim();

            if (behaviors.ContainsKey(id))
            {
                return false;
            }

            behaviors.Add(
                id,
                new AIBehaviorDefinition(
                    id,
                    behaviorName.Trim()));

            return true;
        }

        public bool ConfigureBehavior(
            string behaviorId,
            float aggression,
            float caution,
            float adaptability)
        {
            AIBehaviorDefinition behavior =
                GetBehavior(behaviorId);

            return behavior != null &&
                   behavior.Configure(
                       aggression,
                       caution,
                       adaptability);
        }

        public AIBehaviorDefinition GetBehavior(
            string behaviorId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(behaviorId))
            {
                return null;
            }

            behaviors.TryGetValue(
                behaviorId.Trim(),
                out AIBehaviorDefinition behavior);

            return behavior;
        }

        public bool RemoveBehavior(
            string behaviorId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(behaviorId))
            {
                return false;
            }

            return behaviors.Remove(
                behaviorId.Trim());
        }

        public IReadOnlyCollection<
            AIBehaviorDefinition>
            GetBehaviors()
        {
            return behaviors.Values;
        }

        public void Reset()
        {
            behaviors.Clear();
            Initialized = false;
        }
    }
}
