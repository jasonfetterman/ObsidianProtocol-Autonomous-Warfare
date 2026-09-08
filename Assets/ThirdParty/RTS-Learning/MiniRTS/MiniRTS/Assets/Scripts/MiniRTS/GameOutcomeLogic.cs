using System;

namespace MiniRTS
{
    public enum GameOutcome
    {
        InProgress,
        Victory,
        Defeat
    }

    /// <summary>
    /// Pure elimination rule: a faction remains alive while it owns any building.
    /// Player defeat wins simultaneous-elimination ties.
    /// </summary>
    public static class GameOutcomeLogic
    {
        public static GameOutcome Determine(
            int playerBuildingCount,
            int enemyBuildingCount)
        {
            if (playerBuildingCount < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(playerBuildingCount));
            }

            if (enemyBuildingCount < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(enemyBuildingCount));
            }

            if (playerBuildingCount == 0)
            {
                return GameOutcome.Defeat;
            }

            return enemyBuildingCount == 0
                ? GameOutcome.Victory
                : GameOutcome.InProgress;
        }
    }
}
