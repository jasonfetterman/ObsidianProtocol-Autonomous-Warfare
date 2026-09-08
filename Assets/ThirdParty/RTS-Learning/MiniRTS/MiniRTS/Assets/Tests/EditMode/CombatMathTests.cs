using NUnit.Framework;

namespace MiniRTS.Tests.EditMode
{
    public sealed class CombatMathTests
    {
        [Test]
        public void ApplyDamage_SubtractsAndClampsAtZero()
        {
            Assert.That(CombatMath.ApplyDamage(45, 6), Is.EqualTo(39));
            Assert.That(CombatMath.ApplyDamage(4, 30), Is.Zero);
            Assert.That(CombatMath.ApplyDamage(40, 0), Is.EqualTo(40));
        }

        [Test]
        public void Cooldown_IsReadyAtScheduledAttackTime()
        {
            float nextAttack = CombatMath.ScheduleNextAttack(10f, 0.8f);

            Assert.That(nextAttack, Is.EqualTo(10.8f).Within(0.0001f));
            Assert.That(CombatMath.IsCooldownReady(10.79f, nextAttack), Is.False);
            Assert.That(CombatMath.IsCooldownReady(10.8f, nextAttack), Is.True);
        }

        [TestCase(-20, 100, 0f)]
        [TestCase(25, 100, 0.25f)]
        [TestCase(140, 100, 1f)]
        public void HitPointFraction_ClampsToDisplayRange(
            int current,
            int maximum,
            float expected)
        {
            Assert.That(
                CombatMath.HitPointFraction(current, maximum),
                Is.EqualTo(expected).Within(0.0001f));
        }
    }
}
