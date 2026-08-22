using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Progression
{
    public sealed class FacilityProgress
    {
        public string FacilityId { get; }

        public int Level
        {
            get;
            private set;
        }

        public long DevelopmentExperience
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
            !string.IsNullOrWhiteSpace(FacilityId) &&
            Level > 0 &&
            DevelopmentExperience >= 0 &&
            ExperienceForNextLevel > 0;

        public FacilityProgress(
            string facilityId)
        {
            FacilityId =
                facilityId ?? string.Empty;

            Level = 1;
            DevelopmentExperience = 0;
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
                    (double)DevelopmentExperience /
                    ExperienceForNextLevel);
            }
        }

        public bool AddExperience(
            long amount)
        {
            if (amount <= 0)
                return false;

            if (DevelopmentExperience >
                long.MaxValue - amount)
            {
                DevelopmentExperience =
                    long.MaxValue;
            }
            else
            {
                DevelopmentExperience += amount;
            }

            return true;
        }

        public bool CanLevelUp()
        {
            return DevelopmentExperience >=
                   ExperienceForNextLevel;
        }

        public bool TryLevelUp()
        {
            if (!CanLevelUp())
                return false;

            while (DevelopmentExperience >=
                   ExperienceForNextLevel)
            {
                DevelopmentExperience -=
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
            long developmentExperience,
            long experienceForNextLevel)
        {
            Level =
                Math.Max(1, level);

            DevelopmentExperience =
                Math.Max(
                    0L,
                    developmentExperience);

            ExperienceForNextLevel =
                Math.Max(
                    1L,
                    experienceForNextLevel);
        }

        public void Reset()
        {
            Level = 1;
            DevelopmentExperience = 0;
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
                ((long)(level - 1) * 125L);

            return Math.Max(1L, value);
        }
    }

    public sealed class FacilityProgression
    {
        private readonly Dictionary<
            string,
            FacilityProgress> facilities =
            new Dictionary<
                string,
                FacilityProgress>(
                StringComparer.OrdinalIgnoreCase);

        public bool Register(
            FacilityProgress facility)
        {
            if (facility == null ||
                !facility.Valid ||
                facilities.ContainsKey(
                    facility.FacilityId))
            {
                return false;
            }

            facilities.Add(
                facility.FacilityId,
                facility);

            return true;
        }

        public bool Remove(
            string facilityId)
        {
            if (string.IsNullOrWhiteSpace(
                    facilityId))
            {
                return false;
            }

            return facilities.Remove(
                facilityId);
        }

        public bool TryGet(
            string facilityId,
            out FacilityProgress facility)
        {
            return facilities.TryGetValue(
                facilityId,
                out facility);
        }

        public bool AddExperience(
            string facilityId,
            long amount)
        {
            if (!facilities.TryGetValue(
                    facilityId,
                    out FacilityProgress facility))
            {
                return false;
            }

            return facility.AddExperience(
                amount);
        }

        public bool TryLevelUp(
            string facilityId)
        {
            if (!facilities.TryGetValue(
                    facilityId,
                    out FacilityProgress facility))
            {
                return false;
            }

            return facility.TryLevelUp();
        }

        public IReadOnlyCollection<
            FacilityProgress>
            GetFacilities()
        {
            return facilities.Values;
        }

        public void Clear()
        {
            facilities.Clear();
        }
    }
}
