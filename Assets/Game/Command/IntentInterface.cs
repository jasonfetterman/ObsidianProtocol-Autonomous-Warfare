using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Command
{
    public sealed class CommandIntent
    {
        public string IntentId { get; }
        public IntentType Type { get; }
        public string TargetId { get; }
        public float Priority { get; }

        public CommandIntent(
            string intentId,
            IntentType type,
            string targetId,
            float priority)
        {
            IntentId = intentId ?? string.Empty;
            Type = type;
            TargetId = targetId ?? string.Empty;
            Priority = Math.Max(0f, priority);
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(IntentId);
    }

    public sealed class IntentInterface
    {
        private readonly Dictionary<string, CommandIntent> intents =
            new Dictionary<string, CommandIntent>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(CommandIntent intent)
        {
            if (intent == null ||
                !intent.Valid ||
                intents.ContainsKey(intent.IntentId))
            {
                return false;
            }

            intents.Add(
                intent.IntentId,
                intent);

            return true;
        }

        public bool Remove(string intentId)
        {
            if (string.IsNullOrWhiteSpace(intentId))
                return false;

            return intents.Remove(intentId);
        }

        public bool TryGet(
            string intentId,
            out CommandIntent intent)
        {
            return intents.TryGetValue(
                intentId,
                out intent);
        }

        public IReadOnlyCollection<CommandIntent>
            GetIntents()
        {
            return intents.Values;
        }

        public void Clear()
        {
            intents.Clear();
        }
    }
}
