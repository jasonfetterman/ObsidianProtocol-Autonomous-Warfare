using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.VerticalSlice
{
    public enum VerticalSliceCombatState
    {
        Idle,
        Engaged,
        Victory,
        Defeat
    }

    public sealed class VerticalSliceCombatEngagement
    {
        public string EngagementId { get; }

        public string AttackerId { get; }

        public string TargetId { get; }

        public int Damage { get; }

        public bool Resolved { get; private set; }

        public VerticalSliceCombatEngagement(
            string engagementId,
            string attackerId,
            string targetId,
            int damage)
        {
            EngagementId =
                engagementId ?? string.Empty;

            AttackerId =
                attackerId ?? string.Empty;

            TargetId =
                targetId ?? string.Empty;

            Damage =
                Math.Max(0, damage);

            Resolved = false;
        }

        public bool Resolve()
        {
            if (Resolved)
            {
                return false;
            }

            Resolved = true;

            return true;
        }
    }

    public sealed class VerticalSliceCombat
    {
        private readonly Dictionary<
            string,
            VerticalSliceCombatEngagement> engagements =
            new Dictionary<
                string,
                VerticalSliceCombatEngagement>(
                StringComparer.OrdinalIgnoreCase);

        public bool Initialized { get; private set; }

        public VerticalSliceCombatState State
        {
            get;
            private set;
        }

        public int EngagementCount =>
            engagements.Count;

        public int ResolvedEngagementCount
        {
            get
            {
                int count = 0;

                foreach (VerticalSliceCombatEngagement engagement
                         in engagements.Values)
                {
                    if (engagement.Resolved)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public bool Initialize()
        {
            if (Initialized)
            {
                return false;
            }

            engagements.Clear();

            State =
                VerticalSliceCombatState.Idle;

            Initialized = true;

            return true;
        }

        public bool BeginEngagement(
            string engagementId,
            string attackerId,
            string targetId,
            int damage)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(engagementId) ||
                string.IsNullOrWhiteSpace(attackerId) ||
                string.IsNullOrWhiteSpace(targetId) ||
                damage < 0)
            {
                return false;
            }

            string id =
                engagementId.Trim();

            if (engagements.ContainsKey(id))
            {
                return false;
            }

            engagements.Add(
                id,
                new VerticalSliceCombatEngagement(
                    id,
                    attackerId.Trim(),
                    targetId.Trim(),
                    damage));

            State =
                VerticalSliceCombatState.Engaged;

            return true;
        }

        public bool ResolveEngagement(
            string engagementId)
        {
            VerticalSliceCombatEngagement engagement =
                GetEngagement(engagementId);

            if (engagement == null)
            {
                return false;
            }

            bool resolved =
                engagement.Resolve();

            if (resolved &&
                ResolvedEngagementCount ==
                EngagementCount)
            {
                State =
                    VerticalSliceCombatState.Idle;
            }

            return resolved;
        }

        public bool SetVictory()
        {
            if (!Initialized)
            {
                return false;
            }

            State =
                VerticalSliceCombatState.Victory;

            return true;
        }

        public bool SetDefeat()
        {
            if (!Initialized)
            {
                return false;
            }

            State =
                VerticalSliceCombatState.Defeat;

            return true;
        }

        public VerticalSliceCombatEngagement GetEngagement(
            string engagementId)
        {
            if (!Initialized ||
                string.IsNullOrWhiteSpace(engagementId))
            {
                return null;
            }

            engagements.TryGetValue(
                engagementId.Trim(),
                out VerticalSliceCombatEngagement engagement);

            return engagement;
        }

        public IReadOnlyCollection<
            VerticalSliceCombatEngagement>
            GetEngagements()
        {
            return engagements.Values;
        }

        public void Reset()
        {
            engagements.Clear();

            State =
                VerticalSliceCombatState.Idle;

            Initialized = false;
        }
    }
}
