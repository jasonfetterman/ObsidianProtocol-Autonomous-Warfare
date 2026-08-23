using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.OpenWorld
{
    public enum AIRegionBehavior
    {
        Passive,
        Defensive,
        Expansionist,
        Aggressive,
        Opportunistic
    }

    public enum AIRegionState
    {
        Uncontrolled,
        Controlled,
        Contested,
        Collapsing,
        Captured
    }

    public sealed class AIControlledRegionRecord
    {
        public string RegionId { get; }

        public string FactionId { get; private set; }

        public AIRegionBehavior Behavior { get; private set; }

        public AIRegionState State { get; private set; }

        public float ControlStrength { get; private set; }

        public long LastUpdateTick { get; private set; }

        public AIControlledRegionRecord(
            string regionId,
            string factionId,
            AIRegionBehavior behavior)
        {
            RegionId =
                regionId ?? string.Empty;

            FactionId =
                factionId ?? string.Empty;

            Behavior = behavior;

            State =
                AIRegionState.Uncontrolled;

            ControlStrength = 0f;
            LastUpdateTick = 0;
        }

        public bool AssignFaction(
            string factionId)
        {
            if (string.IsNullOrWhiteSpace(factionId))
            {
                return false;
            }

            FactionId =
                factionId.Trim();

            State =
                AIRegionState.Controlled;

            return true;
        }

        public bool SetBehavior(
            AIRegionBehavior behavior)
        {
            Behavior = behavior;
            return true;
        }

        public bool SetControlStrength(
            float strength,
            long updateTick)
        {
            if (strength < 0f ||
                strength > 100f ||
                updateTick < LastUpdateTick)
            {
                return false;
            }

            ControlStrength = strength;
            LastUpdateTick = updateTick;

            if (ControlStrength <= 0f)
            {
                State =
                    AIRegionState.Uncontrolled;
            }
            else if (ControlStrength < 25f)
            {
                State =
                    AIRegionState.Collapsing;
            }
            else
            {
                State =
                    AIRegionState.Controlled;
            }

            return true;
        }

        public bool SetContested()
        {
            if (State ==
                AIRegionState.Uncontrolled)
            {
                return false;
            }

            State =
                AIRegionState.Contested;

            return true;
        }

        public bool Capture()
        {
            if (State ==
                AIRegionState.Uncontrolled)
            {
                return false;
            }

            State =
                AIRegionState.Captured;

            ControlStrength = 100f;

            return true;
        }
    }

    public sealed class AIControlledRegions
    {
        private readonly Dictionary<
            string,
            AIControlledRegionRecord> regions =
            new Dictionary<
                string,
                AIControlledRegionRecord>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int RegionCount =>
            regions.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            regions.Clear();
            Initialized = true;

            return true;
        }

        public bool RegisterRegion(
            string regionId,
            string factionId,
            AIRegionBehavior behavior)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(regionId))
            {
                return false;
            }

            string id =
                regionId.Trim();

            if (regions.ContainsKey(id))
            {
                return false;
            }

            regions.Add(
                id,
                new AIControlledRegionRecord(
                    id,
                    factionId,
                    behavior));

            return true;
        }

        public bool AssignFaction(
            string regionId,
            string factionId)
        {
            AIControlledRegionRecord record =
                GetRegion(regionId);

            return record != null &&
                   record.AssignFaction(factionId);
        }

        public bool SetBehavior(
            string regionId,
            AIRegionBehavior behavior)
        {
            AIControlledRegionRecord record =
                GetRegion(regionId);

            return record != null &&
                   record.SetBehavior(behavior);
        }

        public bool SetControlStrength(
            string regionId,
            float strength,
            long updateTick)
        {
            AIControlledRegionRecord record =
                GetRegion(regionId);

            return record != null &&
                   record.SetControlStrength(
                       strength,
                       updateTick);
        }

        public bool SetContested(
            string regionId)
        {
            AIControlledRegionRecord record =
                GetRegion(regionId);

            return record != null &&
                   record.SetContested();
        }

        public bool Capture(
            string regionId)
        {
            AIControlledRegionRecord record =
                GetRegion(regionId);

            return record != null &&
                   record.Capture();
        }

        public AIControlledRegionRecord GetRegion(
            string regionId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(regionId))
            {
                return null;
            }

            regions.TryGetValue(
                regionId.Trim(),
                out AIControlledRegionRecord record);

            return record;
        }

        public IReadOnlyCollection<
            AIControlledRegionRecord>
            GetRegions()
        {
            return regions.Values;
        }

        public void Reset()
        {
            regions.Clear();
            Initialized = false;
        }
    }
}
