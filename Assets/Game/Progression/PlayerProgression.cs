using System;

namespace ObsidianProtocol.Game.Progression
{
    public sealed class PlayerProgression
    {
        public string PlayerId { get; }

        public int Level
        {
            get;
            private set;
        }

        public long Experience
        {
            get;
            private set;
        }

        public long ExperienceForNextLevel
        {
            get;
            private set;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(PlayerId) &&
            Level > 0 &&
            Experience >= 0 &&
            ExperienceForNextLevel > 0;

        public PlayerProgression(
            string playerId,
            int startingLevel = 1,
            long startingExperience = 0,
            long experienceForNextLevel = 100)
        {
            PlayerId =
                playerId ?? string.Empty;

            Level =
                Math.Max(1, startingLevel);

            Experience =
                Math.Max(0L, startingExperience);

            ExperienceForNextLevel =
                Math.Max(
                    1L,
                    experienceForNextLevel);
        }

        public double ProgressToNextLevel
        {
            get
            {
                if (ExperienceForNextLevel <= 0)
                    return 0.0;

                return Math.Min(
                    1.0,
                    (double)Experience /
                    ExperienceForNextLevel);
            }
        }

        public bool AddExperience(
            long amount)
        {
            if (amount <= 0)
                return false;

            if (Experience >
                long.MaxValue - amount)
            {
                Experience =
                    long.MaxValue;
            }
            else
            {
                Experience += amount;
            }

            return true;
        }

        public bool CanLevelUp()
        {
            return Experience >=
                   ExperienceForNextLevel;
        }

        public bool TryLevelUp()
        {
            if (!CanLevelUp())
                return false;

            while (Experience >=
                   ExperienceForNextLevel)
            {
                Experience -=
                    ExperienceForNextLevel;

                if (Level < int.MaxValue)
                    Level++;

                ExperienceForNextLevel =
                    CalculateNextLevelExperience(
                        Level);
            }

            return true;
        }

        public void SetProgression(
            int level,
            long experience,
            long experienceForNextLevel)
        {
            Level =
                Math.Max(1, level);

            Experience =
                Math.Max(0L, experience);

            ExperienceForNextLevel =
                Math.Max(
                    1L,
                    experienceForNextLevel);
        }

        public void Reset()
        {
            Level = 1;
            Experience = 0;
            ExperienceForNextLevel = 100;
        }

        private static long
            CalculateNextLevelExperience(
                int level)
        {
            if (level <= 1)
                return 100;

            long value =
                100L +
                ((long)(level - 1) * 50L);

            return Math.Max(1L, value);
        }
    }
}
