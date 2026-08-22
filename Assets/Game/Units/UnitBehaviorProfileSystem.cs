using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Units
{
    public enum UnitBehaviorProfile
    {
        Aggressive,
        Defensive,
        Reconnaissance,
        Support,
        Cautious,
        Autonomous,
        Escort,
        Patrol,
        HoldPosition,
        Pursuit,
        Retreat,
        Emergency
    }

    public sealed class UnitBehaviorState
    {
        public string UnitId { get; }

        public UnitBehaviorProfile Profile { get; private set; }

        public float Aggression { get; private set; }
        public float Caution { get; private set; }
        public float Autonomy { get; private set; }
        public float Discipline { get; private set; }

        public UnitBehaviorState(string unitId)
        {
            UnitId = unitId ?? string.Empty;
            Profile = UnitBehaviorProfile.Autonomous;

            Aggression = 0.5f;
            Caution = 0.5f;
            Autonomy = 1f;
            Discipline = 1f;
        }

        public void Configure(
            UnitBehaviorProfile profile,
            float aggression,
            float caution,
            float autonomy,
            float discipline)
        {
            Profile = profile;

            Aggression =
                Math.Clamp(aggression, 0f, 1f);

            Caution =
                Math.Clamp(caution, 0f, 1f);

            Autonomy =
                Math.Clamp(autonomy, 0f, 1f);

            Discipline =
                Math.Clamp(discipline, 0f, 1f);
        }
    }

    public sealed class UnitBehaviorProfileSystem
    {
        private readonly Dictionary<string, UnitBehaviorState> states =
            new Dictionary<string, UnitBehaviorState>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterUnit(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!states.ContainsKey(unitId))
            {
                states.Add(
                    unitId,
                    new UnitBehaviorState(unitId));
            }
        }

        public void ConfigureUnit(
            string unitId,
            UnitBehaviorProfile profile,
            float aggression,
            float caution,
            float autonomy,
            float discipline)
        {
            RegisterUnit(unitId);

            states[unitId].Configure(
                profile,
                aggression,
                caution,
                autonomy,
                discipline);
        }

        public bool TryGetBehavior(
            string unitId,
            out UnitBehaviorState state)
        {
            return states.TryGetValue(
                unitId,
                out state);
        }

        public void SetProfile(
            string unitId,
            UnitBehaviorProfile profile)
        {
            RegisterUnit(unitId);

            UnitBehaviorState state =
                states[unitId];

            state.Configure(
                profile,
                state.Aggression,
                state.Caution,
                state.Autonomy,
                state.Discipline);
        }

        public void RemoveUnit(string unitId)
        {
            states.Remove(unitId);
        }

        public void Clear()
        {
            states.Clear();
        }
    }
}
