using NUnit.Framework;

namespace MiniRTS.Tests.EditMode
{
    public sealed class GameOutcomeLogicTests
    {
        [TestCase(1, 1, GameOutcome.InProgress)]
        [TestCase(3, 0, GameOutcome.Victory)]
        [TestCase(0, 4, GameOutcome.Defeat)]
        [TestCase(0, 0, GameOutcome.Defeat)]
        public void Determine_UsesAllBuildingsEliminationRule(
            int playerBuildings,
            int enemyBuildings,
            GameOutcome expected)
        {
            Assert.That(
                GameOutcomeLogic.Determine(
                    playerBuildings,
                    enemyBuildings),
                Is.EqualTo(expected));
        }
    }
}
