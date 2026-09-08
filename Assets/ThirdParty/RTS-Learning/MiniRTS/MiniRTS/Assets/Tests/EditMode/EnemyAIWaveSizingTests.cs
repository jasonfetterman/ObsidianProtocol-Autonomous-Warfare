using NUnit.Framework;

namespace MiniRTS.Tests.EditMode
{
    public sealed class EnemyAIWaveSizingTests
    {
        [TestCase(0, 4)]
        [TestCase(1, 6)]
        [TestCase(5, 14)]
        public void DesiredUnitCount_GrowsByTwoPerWave(
            int waveIndex,
            int expected)
        {
            Assert.That(
                EnemyAIWaveSizing.CalculateDesiredUnitCount(waveIndex),
                Is.EqualTo(expected));
        }

        [Test]
        public void CommittedUnitCount_IsCappedByAvailableMilitary()
        {
            Assert.That(
                EnemyAIWaveSizing.CalculateCommittedUnitCount(3, 5),
                Is.EqualTo(5));
            Assert.That(
                EnemyAIWaveSizing.CalculateCommittedUnitCount(3, 20),
                Is.EqualTo(10));
        }

        [Test]
        public void DesiredUnitCount_DoesNotExceedMaximumSupply()
        {
            Assert.That(
                EnemyAIWaveSizing.CalculateDesiredUnitCount(1000),
                Is.EqualTo(BalanceConfig.MaximumSupply));
        }
    }
}
