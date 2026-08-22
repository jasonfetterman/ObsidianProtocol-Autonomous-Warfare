using System;

namespace ObsidianProtocol.Game.Progression
{
    public sealed class FabricationProgression
    {
        public string PlayerId { get; }

        public int FabricationLevel
        {
            get;
            private set;
        }

        public long FabricationExperience
        {
            get;
            private set;
        }

        public long ExperienceForNextLevel
        {
            get;
            private set;
        }

        public int ActiveFabricationCapacity
        {
            get;
            private set;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(PlayerId) &&
            FabricationLevel > 0 &&
            FabricationExperience >= 0 &&
            ExperienceForNextLevel > 0 &&
            ActiveFabricationCapacity >= 0;

        public FabricationProgression(
            string playerId)
        {
            PlayerId =
                playerId ?? string.Empty;

            FabricationLevel = 1;
            FabricationExperience = 0;
            ExperienceForNextLevel = 100;
            ActiveFabricationCapacity = 1;
        }

        public double ProgressToNextLevel
        {
            get
            {
                if (ExperienceForNextLevel <= 0)
                    return 0.0;

                return Math.Min(
                    1.0,
                    (double)FabricationExperience /
                    ExperienceForNextLevel);
            }
        }

        public bool AddExperience(
            long amount)
        {
            if (amount <= 0)
                return false;

            if (FabricationExperience >
                long.MaxValue - amount)
            {
                FabricationExperience =
                    long.MaxValue;
            }
            else
            {
                FabricationExperience += amount;
            }

            return true;
        }

        public bool CanLevelUp()
        {
            return FabricationExperience >=
                   ExperienceForNextLevel;
        }

        public bool TryLevelUp()
        {
            if (!CanLevelUp())
                return false;

            while (FabricationExperience >=
                   ExperienceForNextLevel)
            {
                FabricationExperience -=
                    ExperienceForNextLevel;

                if (FabricationLevel < int.MaxValue)
                    FabricationLevel++;

                if (ActiveFabricationCapacity <
                    int.MaxValue)
                {
                    ActiveFabricationCapacity++;
                }

                ExperienceForNextLevel =
                    CalculateNextLevelExperience(
                        FabricationLevel);
            }

            return true;
        }

        public void SetProgression(
            int level,
            long experience,
            long experienceForNextLevel)
        {
            FabricationLevel =
                Math.Max(1, level);

            FabricationExperience =
                Math.Max(0L, experience);

            ExperienceForNextLevel =
                Math.Max(
                    1L,
                    experienceForNextLevel);

            ActiveFabricationCapacity =
                CalculateFabricationCapacity(
                    FabricationLevel);
        }

        public void SetFabricationCapacity(
            int capacity)
        {
            ActiveFabricationCapacity =
                Math.Max(0, capacity);
        }

        public bool CanFabricate(
            int requiredCapacity)
        {
            if (requiredCapacity <= 0)
                return false;

            return requiredCapacity <=
                   ActiveFabricationCapacity;
        }

        public void Reset()
        {
            FabricationLevel = 1;
            FabricationExperience = 0;
            ExperienceForNextLevel = 100;
            ActiveFabricationCapacity = 1;
        }

        private static long
            CalculateNextLevelExperience(
                int level)
        {
            if (level <= 1)
                return 100;

            long value =
                100L +
                ((long)(level - 1) * 150L);

            return Math.Max(1L, value);
        }

        private static int
            CalculateFabricationCapacity(
                int level)
        {
            if (level <= 0)
                return 0;

            return Math.Max(
                1,
                level);
        }
    }
}
