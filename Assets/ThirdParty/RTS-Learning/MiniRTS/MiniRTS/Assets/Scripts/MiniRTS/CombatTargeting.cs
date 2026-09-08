using System;
using System.Collections.Generic;

namespace MiniRTS
{
    /// <summary>
    /// Scene-independent input used by the nearest-enemy target selector.
    /// Lower priority values win only when distances are equal.
    /// </summary>
    public readonly struct CombatTargetCandidate
    {
        public int OwnerId { get; }
        public float DistanceSquared { get; }
        public bool IsAlive { get; }
        public int TieBreakPriority { get; }
        public bool IsVisibleToAttacker { get; }

        public CombatTargetCandidate(
            int ownerId,
            float distanceSquared,
            bool isAlive,
            int tieBreakPriority,
            bool isVisibleToAttacker = true)
        {
            OwnerId = ownerId;
            DistanceSquared = distanceSquared;
            IsAlive = isAlive;
            TieBreakPriority = tieBreakPriority;
            IsVisibleToAttacker = isVisibleToAttacker;
        }
    }

    /// <summary>
    /// Pure target-acquisition rules shared by Combatant and EditMode tests.
    /// </summary>
    public static class CombatTargeting
    {
        private const float DistanceTieTolerance = 0.0001f;

        public static int FindNearestEnemyIndex(
            IReadOnlyList<CombatTargetCandidate> candidates,
            int attackerOwnerId,
            float maximumDistance)
        {
            if (candidates == null)
            {
                throw new ArgumentNullException(nameof(candidates));
            }

            if (maximumDistance < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(maximumDistance));
            }

            float maximumDistanceSquared = maximumDistance * maximumDistance;
            float bestDistanceSquared = float.MaxValue;
            int bestPriority = int.MaxValue;
            int bestIndex = -1;

            for (int i = 0; i < candidates.Count; i++)
            {
                CombatTargetCandidate candidate = candidates[i];
                if (!candidate.IsAlive ||
                    !candidate.IsVisibleToAttacker ||
                    candidate.OwnerId == attackerOwnerId ||
                    float.IsNaN(candidate.DistanceSquared) ||
                    candidate.DistanceSquared < 0f ||
                    candidate.DistanceSquared > maximumDistanceSquared)
                {
                    continue;
                }

                bool isCloser =
                    candidate.DistanceSquared <
                    bestDistanceSquared - DistanceTieTolerance;
                bool winsTie =
                    Math.Abs(candidate.DistanceSquared - bestDistanceSquared) <=
                    DistanceTieTolerance &&
                    candidate.TieBreakPriority < bestPriority;
                if (!isCloser && !winsTie)
                {
                    continue;
                }

                bestIndex = i;
                bestDistanceSquared = candidate.DistanceSquared;
                bestPriority = candidate.TieBreakPriority;
            }

            return bestIndex;
        }
    }
}
