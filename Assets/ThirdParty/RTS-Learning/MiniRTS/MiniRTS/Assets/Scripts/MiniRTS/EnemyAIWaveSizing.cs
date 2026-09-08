using System;

namespace MiniRTS
{
    /// <summary>
    /// Pure wave-growth math. Wave indices are zero-based, so the first wave is four.
    /// </summary>
    public static class EnemyAIWaveSizing
    {
        public static int CalculateDesiredUnitCount(int waveIndex)
        {
            if (waveIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(waveIndex));
            }

            long desired =
                BalanceConfig.EnemyAIInitialWaveSize +
                (long)BalanceConfig.EnemyAIWaveSizeIncrease * waveIndex;
            return (int)Math.Min(BalanceConfig.MaximumSupply, desired);
        }

        public static int CalculateCommittedUnitCount(
            int waveIndex,
            int availableMilitaryCount)
        {
            if (availableMilitaryCount < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(availableMilitaryCount));
            }

            return Math.Min(
                CalculateDesiredUnitCount(waveIndex),
                availableMilitaryCount);
        }
    }
}
