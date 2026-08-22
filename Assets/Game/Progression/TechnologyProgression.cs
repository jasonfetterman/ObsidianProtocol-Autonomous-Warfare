using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Progression
{
    public sealed class TechnologyProgress
    {
        public string TechnologyId { get; }

        public int Level
        {
            get;
            private set;
        }

        public long ResearchExperience
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
            !string.IsNullOrWhiteSpace(TechnologyId) &&
            Level > 0 &&
            ResearchExperience >= 0 &&
            ExperienceForNextLevel > 0;

        public TechnologyProgress(
            string technologyId)
        {
            TechnologyId =
                technologyId ?? string.Empty;

            Level = 1;
            ResearchExperience = 0;
            ExperienceForNextLevel = 100;
        }

        public double ProgressToNextLevel
        {
            get
            {
                if (ExperienceForNextLevel <= 0)
                    return 0.0;

                return Math.Min(
                    1.0,
                    (double)ResearchExperience /
                    ExperienceForNextLevel);
            }
        }

        public bool AddResearchExperience(
            long amount)
        {
            if (amount <= 0)
                return false;

            if (ResearchExperience >
                long.MaxValue - amount)
            {
                ResearchExperience =
                    long.MaxValue;
            }
            else
            {
                ResearchExperience += amount;
            }

            return true;
        }

        public bool CanLevelUp()
        {
            return ResearchExperience >=
                   ExperienceForNextLevel;
        }

        public bool TryLevelUp()
        {
            if (!CanLevelUp())
                return false;

            while (ResearchExperience >=
                   ExperienceForNextLevel)
            {
                ResearchExperience -=
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
            long researchExperience,
            long experienceForNextLevel)
        {
            Level =
                Math.Max(1, level);

            ResearchExperience =
                Math.Max(
                    0L,
                    researchExperience);

            ExperienceForNextLevel =
                Math.Max(
                    1L,
                    experienceForNextLevel);
        }

        public void Reset()
        {
            Level = 1;
            ResearchExperience = 0;
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
                ((long)(level - 1) * 100L);

            return Math.Max(1L, value);
        }
    }

    public sealed class TechnologyProgression
    {
        private readonly Dictionary<
            string,
            TechnologyProgress> technologies =
            new Dictionary<
                string,
                TechnologyProgress>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            TechnologyProgress technology)
        {
            if (technology == null ||
                !technology.Valid ||
                technologies.ContainsKey(
                    technology.TechnologyId))
            {
                return false;
            }

            technologies.Add(
                technology.TechnologyId,
                technology);

            return true;
        }

        public bool Remove(
            string technologyId)
        {
            if (string.IsNullOrWhiteSpace(
                    technologyId))
            {
                return false;
            }

            return technologies.Remove(
                technologyId);
        }

        public bool TryGet(
            string technologyId,
            out TechnologyProgress technology)
        {
            return technologies.TryGetValue(
                technologyId,
                out technology);
        }

        public bool AddResearchExperience(
            string technologyId,
            long amount)
        {
            if (!technologies.TryGetValue(
                    technologyId,
                    out TechnologyProgress technology))
            {
                return false;
            }

            return technology.AddResearchExperience(
                amount);
        }

        public bool TryLevelUp(
            string technologyId)
        {
            if (!technologies.TryGetValue(
                    technologyId,
                    out TechnologyProgress technology))
            {
                return false;
            }

            return technology.TryLevelUp();
        }

        public IReadOnlyCollection<
            TechnologyProgress>
            GetTechnologies()
        {
            return technologies.Values;
        }

        public void Clear()
        {
            technologies.Clear();
        }
    }
}
