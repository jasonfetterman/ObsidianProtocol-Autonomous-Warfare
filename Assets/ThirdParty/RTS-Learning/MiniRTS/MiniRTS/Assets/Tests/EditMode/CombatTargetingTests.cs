using System.Collections.Generic;
using NUnit.Framework;

namespace MiniRTS.Tests.EditMode
{
    public sealed class CombatTargetingTests
    {
        [Test]
        public void FindNearestEnemy_IgnoresFriendlyDeadAndOutOfRangeTargets()
        {
            List<CombatTargetCandidate> candidates =
                new List<CombatTargetCandidate>
                {
                    new CombatTargetCandidate(0, 1f, true, 0),
                    new CombatTargetCandidate(1, 4f, false, 0),
                    new CombatTargetCandidate(1, 100f, true, 0),
                    new CombatTargetCandidate(1, 9f, true, 1),
                    new CombatTargetCandidate(1, 6.25f, true, 0)
                };

            int result = CombatTargeting.FindNearestEnemyIndex(
                candidates,
                0,
                5f);

            Assert.That(result, Is.EqualTo(4));
        }

        [Test]
        public void FindNearestEnemy_PrefersUnitPriorityOnExactDistanceTie()
        {
            List<CombatTargetCandidate> candidates =
                new List<CombatTargetCandidate>
                {
                    new CombatTargetCandidate(1, 16f, true, 1),
                    new CombatTargetCandidate(1, 16f, true, 0)
                };

            int result = CombatTargeting.FindNearestEnemyIndex(
                candidates,
                0,
                8f);

            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void FindNearestEnemy_ReturnsNoneWhenNoEnemyQualifies()
        {
            List<CombatTargetCandidate> candidates =
                new List<CombatTargetCandidate>
                {
                    new CombatTargetCandidate(0, 1f, true, 0),
                    new CombatTargetCandidate(1, 4f, false, 0)
                };

            Assert.That(
                CombatTargeting.FindNearestEnemyIndex(candidates, 0, 5f),
                Is.EqualTo(-1));
        }

        [Test]
        public void FindNearestEnemy_IgnoresCloserFogHiddenEnemy()
        {
            List<CombatTargetCandidate> candidates =
                new List<CombatTargetCandidate>
                {
                    new CombatTargetCandidate(1, 1f, true, 0, false),
                    new CombatTargetCandidate(1, 9f, true, 0, true)
                };

            Assert.That(
                CombatTargeting.FindNearestEnemyIndex(candidates, 0, 5f),
                Is.EqualTo(1));
        }
    }
}
