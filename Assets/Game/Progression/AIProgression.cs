using System;

namespace ObsidianProtocol.Game.Progression
{
    public sealed class AIProgression
    {
        public string PlayerId { get; }

        public int AILevel
        {
            get;
            private set;
        }

        public long AIExperience
        {
            get;
            private set;
        }

        public long ExperienceForNextLevel
        {
            get;
            private set;
        }

        public int AutonomyLevel
        {
            get;
            private set;
        }

        public int CoordinationLevel
        {
            get;
            private set;
        }

        public int PerceptionLevel
        {
            get;
            private set;
        }

        public int DecisionLevel
        {
            get;
            private set;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(PlayerId) &&
            AILevel > 0 &&
            AIExperience >= 0 &&
            ExperienceForNextLevel > 0;

        public AIProgression(
            string playerId)
        {
            PlayerId =
                playerId ?? string.Empty;

            AILevel = 1;
            AIExperience = 0;
            ExperienceForNextLevel = 100;

            AutonomyLevel = 1;
            CoordinationLevel = 1;
            PerceptionLevel = 1;
            DecisionLevel = 1;
        }

        public double ProgressToNextLevel
        {
            get
            {
                if (ExperienceForNextLevel <= 0)
                    return 0.0;

                return Math.Min(
                    1.0,
                    (double)AIExperience /
                    ExperienceForNextLevel);
            }
        }

        public bool AddExperience(
            long amount)
        {
            if (amount <= 0)
                return false;

            if (AIExperience >
                long.MaxValue - amount)
            {
                AIExperience =
                    long.MaxValue;
            }
            else
            {
                AIExperience += amount;
            }

            return true;
        }

        public bool CanLevelUp()
        {
            return AIExperience >=
                   ExperienceForNextLevel;
        }

        public bool TryLevelUp()
        {
            if (!CanLevelUp())
                return false;

            while (AIExperience >=
                   ExperienceForNextLevel)
            {
                AIExperience -=
                    ExperienceForNextLevel;

                if (AILevel < int.MaxValue)
                    AILevel++;

                if (AutonomyLevel < AILevel)
                    AutonomyLevel = AILevel;

                if (CoordinationLevel < AILevel)
                    CoordinationLevel = AILevel;

                if (PerceptionLevel < AILevel)
                    PerceptionLevel = AILevel;

                if (DecisionLevel < AILevel)
                    DecisionLevel = AILevel;

                ExperienceForNextLevel =
                    CalculateNextLevelExperience(
                        AILevel);
            }

            return true;
        }

        public void SetProgression(
            int level,
            long experience,
            long experienceForNextLevel)
        {
            AILevel =
                Math.Max(1, level);

            AIExperience =
                Math.Max(0L, experience);

            ExperienceForNextLevel =
                Math.Max(
                    1L,
                    experienceForNextLevel);

            AutonomyLevel =
                Math.Min(
                    AILevel,
                    Math.Max(1, AutonomyLevel));

            CoordinationLevel =
                Math.Min(
                    AILevel,
                    Math.Max(1, CoordinationLevel));

            PerceptionLevel =
                Math.Min(
                    AILevel,
                    Math.Max(1, PerceptionLevel));

            DecisionLevel =
                Math.Min(
                    AILevel,
                    Math.Max(1, DecisionLevel));
        }

        public void SetCapabilityLevels(
            int autonomy,
            int coordination,
            int perception,
            int decision)
        {
            AutonomyLevel =
                ClampCapability(autonomy);

            CoordinationLevel =
                ClampCapability(coordination);

            PerceptionLevel =
                ClampCapability(perception);

            DecisionLevel =
                ClampCapability(decision);
        }

        public bool HasAutonomyCapability(
            int requiredLevel)
        {
            return requiredLevel > 0 &&
                   AutonomyLevel >= requiredLevel;
        }

        public bool HasCoordinationCapability(
            int requiredLevel)
        {
            return requiredLevel > 0 &&
                   CoordinationLevel >= requiredLevel;
        }

        public bool HasPerceptionCapability(
            int requiredLevel)
        {
            return requiredLevel > 0 &&
                   PerceptionLevel >= requiredLevel;
        }

        public bool HasDecisionCapability(
            int requiredLevel)
        {
            return requiredLevel > 0 &&
                   DecisionLevel >= requiredLevel;
        }

        public void Reset()
        {
            AILevel = 1;
            AIExperience = 0;
            ExperienceForNextLevel = 100;

            AutonomyLevel = 1;
            CoordinationLevel = 1;
            PerceptionLevel = 1;
            DecisionLevel = 1;
        }

        private int ClampCapability(
            int value)
        {
            return Math.Max(
                1,
                Math.Min(
                    AILevel,
                    value));
        }

        private static long
            CalculateNextLevelExperience(
                int level)
        {
            if (level <= 1)
                return 100;

            long value =
                100L +
                ((long)(level - 1) * 125L);

            return Math.Max(1L, value);
        }
    }
}
