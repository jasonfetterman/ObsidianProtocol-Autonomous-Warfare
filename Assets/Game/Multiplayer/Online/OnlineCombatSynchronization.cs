using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Multiplayer.Online
{
    public enum OnlineCombatEventType
    {
        None,
        Attack,
        Hit,
        Damage,
        Destroyed,
        Retreated
    }

    public sealed class OnlineCombatEvent
    {
        public string EventId { get; }

        public string SourceUnitId { get; }

        public string TargetUnitId { get; }

        public OnlineCombatEventType Type { get; }

        public float Damage { get; }

        public long SimulationTick { get; }

        public OnlineCombatEvent(
            string eventId,
            string sourceUnitId,
            string targetUnitId,
            OnlineCombatEventType type,
            float damage,
            long simulationTick)
        {
            EventId =
                eventId ?? string.Empty;

            SourceUnitId =
                sourceUnitId ?? string.Empty;

            TargetUnitId =
                targetUnitId ?? string.Empty;

            Type = type;

            Damage =
                Math.Max(0f, damage);

            SimulationTick =
                simulationTick;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(EventId) &&
            !string.IsNullOrWhiteSpace(SourceUnitId) &&
            !string.IsNullOrWhiteSpace(TargetUnitId) &&
            Type != OnlineCombatEventType.None;
    }

    public sealed class OnlineCombatSynchronization
    {
        private readonly Queue<OnlineCombatEvent> pendingEvents =
            new Queue<OnlineCombatEvent>();

        private readonly HashSet<string> processedEventIds =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int PendingEventCount =>
            pendingEvents.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            pendingEvents.Clear();
            processedEventIds.Clear();

            Initialized = true;

            return true;
        }

        public bool SubmitEvent(
            OnlineCombatEvent combatEvent)
        {
            if (!Initialized ||
                combatEvent == null ||
                !combatEvent.Valid)
            {
                return false;
            }

            if (processedEventIds.Contains(
                    combatEvent.EventId))
            {
                return false;
            }

            pendingEvents.Enqueue(
                combatEvent);

            return true;
        }

        public OnlineCombatEvent
            DequeueEvent()
        {
            if (!Initialized ||
                pendingEvents.Count == 0)
            {
                return null;
            }

            OnlineCombatEvent combatEvent =
                pendingEvents.Dequeue();

            processedEventIds.Add(
                combatEvent.EventId);

            return combatEvent;
        }

        public bool HasProcessedEvent(
            string eventId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(eventId))
            {
                return false;
            }

            return processedEventIds.Contains(
                eventId.Trim());
        }

        public void ClearPendingEvents()
        {
            pendingEvents.Clear();
        }

        public void Reset()
        {
            pendingEvents.Clear();
            processedEventIds.Clear();

            Initialized = false;
        }
    }
}
