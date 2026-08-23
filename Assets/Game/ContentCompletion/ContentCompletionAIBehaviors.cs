using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.ContentCompletion
{
    public sealed class ContentCompletionAIBehaviors
    {
        private readonly HashSet<string> behaviors =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int BehaviorCount =>
            behaviors.Count;

        public bool Complete =>
            BehaviorCount > 0;

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

        public bool RegisterBehavior(
            string behaviorId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(behaviorId))
            {
                return false;
            }

            return behaviors.Add(
                behaviorId.Trim());
        }

        public bool ContainsBehavior(
            string behaviorId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(behaviorId))
            {
                return false;
            }

            return behaviors.Contains(
                behaviorId.Trim());
        }

        public IReadOnlyCollection<string>
            GetBehaviors()
        {
            return behaviors;
        }

        public void Reset()
        {
            behaviors.Clear();
            Initialized = false;
        }
    }
}
