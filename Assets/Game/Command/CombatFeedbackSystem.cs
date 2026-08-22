using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Command
{
    public enum CombatFeedbackType
    {
        Damage,
        Destruction,
        CriticalDamage,
        TargetAcquired,
        TargetLost,
        AttackStarted,
        AttackStopped,
        UnitDisabled,
        UnitRecovered,
        FriendlyFire,
        MissionThreat
    }

    public sealed class CombatFeedbackEvent
    {
        public string EventId { get; }
        public CombatFeedbackType Type { get; }
        public string SourceId { get; }
        public string TargetId { get; }
        public float Value { get; }
        public string Message { get; }

        public CombatFeedbackEvent(
            string eventId,
            CombatFeedbackType type,
            string sourceId,
            string targetId,
            float value,
            string message)
        {
            EventId = eventId ?? string.Empty;
            Type = type;

            SourceId = sourceId ?? string.Empty;
            TargetId = targetId ?? string.Empty;

            Value = value;
            Message = message ?? string.Empty;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(EventId);
    }

    public sealed class CombatFeedbackSystem
    {
        private readonly List<CombatFeedbackEvent> events =
            new List<CombatFeedbackEvent>();

        public int EventCount =>
            events.Count;

        public bool Register(
            CombatFeedbackEvent feedbackEvent)
        {
            if (feedbackEvent == null ||
                !feedbackEvent.Valid)
            {
                return false;
            }

            events.Add(feedbackEvent);
            return true;
        }

        public IReadOnlyList<CombatFeedbackEvent>
            GetEvents()
        {
            return events;
        }

        public void Clear()
        {
            events.Clear();
        }
    }
}
