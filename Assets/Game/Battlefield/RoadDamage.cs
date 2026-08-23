using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Battlefield
{
    public enum RoadDamageState
    {
        Clear,
        Damaged,
        SeverelyDamaged,
        Impassable
    }

    public sealed class RoadSegment
    {
        public string SegmentId { get; }

        public float MaximumIntegrity { get; }

        public float Integrity { get; private set; }

        public RoadDamageState State { get; private set; }

        public bool Passable =>
            State != RoadDamageState.Impassable;

        public RoadSegment(
            string segmentId,
            float maximumIntegrity)
        {
            SegmentId =
                segmentId ?? string.Empty;

            MaximumIntegrity =
                Math.Max(0f, maximumIntegrity);

            Integrity =
                MaximumIntegrity;

            State =
                MaximumIntegrity > 0f
                    ? RoadDamageState.Clear
                    : RoadDamageState.Impassable;
        }

        public bool ApplyDamage(
            float damage)
        {
            if (damage < 0f ||
                State == RoadDamageState.Impassable)
            {
                return false;
            }

            Integrity =
                Math.Max(
                    0f,
                    Integrity - damage);

            UpdateState();

            return true;
        }

        public bool Repair(
            float amount)
        {
            if (amount < 0f ||
                MaximumIntegrity <= 0f)
            {
                return false;
            }

            Integrity =
                Math.Min(
                    MaximumIntegrity,
                    Integrity + amount);

            UpdateState();

            return true;
        }

        private void UpdateState()
        {
            if (Integrity <= 0f)
            {
                State =
                    RoadDamageState.Impassable;
                return;
            }

            float ratio =
                Integrity / MaximumIntegrity;

            if (ratio <= 0.25f)
            {
                State =
                    RoadDamageState.SeverelyDamaged;
            }
            else if (ratio < 1f)
            {
                State =
                    RoadDamageState.Damaged;
            }
            else
            {
                State =
                    RoadDamageState.Clear;
            }
        }
    }

    public sealed class RoadDamage
    {
        private readonly Dictionary<
            string,
            RoadSegment> segments =
            new Dictionary<
                string,
                RoadSegment>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public int SegmentCount =>
            segments.Count;

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            segments.Clear();

            Initialized = true;

            return true;
        }

        public bool RegisterSegment(
            string segmentId,
            float maximumIntegrity)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(segmentId) ||
                maximumIntegrity <= 0f)
            {
                return false;
            }

            string id =
                segmentId.Trim();

            if (segments.ContainsKey(id))
            {
                return false;
            }

            segments.Add(
                id,
                new RoadSegment(
                    id,
                    maximumIntegrity));

            return true;
        }

        public bool ApplyDamage(
            string segmentId,
            float damage)
        {
            RoadSegment segment =
                GetSegment(segmentId);

            return segment != null &&
                   segment.ApplyDamage(damage);
        }

        public bool RepairSegment(
            string segmentId,
            float amount)
        {
            RoadSegment segment =
                GetSegment(segmentId);

            return segment != null &&
                   segment.Repair(amount);
        }

        public bool IsPassable(
            string segmentId)
        {
            RoadSegment segment =
                GetSegment(segmentId);

            return segment != null &&
                   segment.Passable;
        }

        public RoadSegment GetSegment(
            string segmentId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(segmentId))
            {
                return null;
            }

            segments.TryGetValue(
                segmentId.Trim(),
                out RoadSegment segment);

            return segment;
        }

        public IReadOnlyCollection<RoadSegment>
            GetSegments()
        {
            return segments.Values;
        }

        public void Reset()
        {
            segments.Clear();

            Initialized = false;
        }
    }
}
