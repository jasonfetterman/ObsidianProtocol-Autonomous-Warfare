using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Intelligence
{
    public enum LostContactBehavior
    {
        HoldLastPosition,
        ContinueMission,
        SearchLastKnownPosition,
        ReturnToRelay,
        Retreat,
        RequestRecon,
        Ignore
    }

    public sealed class LostContactState
    {
        public int TargetId;
        public LostContactBehavior Behavior;
        public DateTime LostAt;
        public float LastKnownDistance;
        public float LastKnownBearing;
        public bool Active;

        public LostContactState(int targetId)
        {
            TargetId = targetId;
            Behavior = LostContactBehavior.HoldLastPosition;
            LostAt = DateTime.UtcNow;
            LastKnownDistance = 0f;
            LastKnownBearing = 0f;
            Active = false;
        }
    }

    public sealed class LostContactBehaviorSystem
    {
        private readonly Dictionary<int, LostContactState> states =
            new Dictionary<int, LostContactState>();

        public void ReportLostContact(
            int targetId,
            LostContactBehavior behavior,
            float lastKnownDistance,
            float lastKnownBearing)
        {
            if (targetId < 0)
            {
                return;
            }

            if (!states.TryGetValue(
                    targetId,
                    out LostContactState state))
            {
                state =
                    new LostContactState(targetId);

                states.Add(
                    targetId,
                    state);
            }

            state.Behavior = behavior;
            state.LostAt = DateTime.UtcNow;
            state.LastKnownDistance =
                Math.Max(0f, lastKnownDistance);
            state.LastKnownBearing =
                lastKnownBearing;
            state.Active = true;
        }

        public LostContactBehavior GetBehavior(
            int targetId)
        {
            return states.TryGetValue(
                       targetId,
                       out LostContactState state)
                ? state.Behavior
                : LostContactBehavior.Ignore;
        }

        public bool TryGetState(
            int targetId,
            out LostContactState state)
        {
            return states.TryGetValue(
                targetId,
                out state);
        }

        public void RestoreContact(int targetId)
        {
            if (states.TryGetValue(
                    targetId,
                    out LostContactState state))
            {
                state.Active = false;
            }
        }

        public void RemoveContact(int targetId)
        {
            states.Remove(targetId);
        }

        public void Clear()
        {
            states.Clear();
        }
    }
}
