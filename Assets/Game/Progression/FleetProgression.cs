using System;

namespace ObsidianProtocol.Game.Progression
{
    public sealed class FleetProgression
    {
        public string PlayerId { get; }

        public int FleetLevel
        {
            get;
            private set;
        }

        public long FleetExperience
        {
            get;
            private set;
        }

        public long ExperienceForNextLevel
        {
            get;
            private set;
        }

        public int UnitsAcquired
        {
            get;
            private set;
        }

        public int UnitsOperational
        {
            get;
            private set;
        }

        public bool Valid =>
            !string.IsNullOrWhiteSpace(PlayerId) &&
            FleetLevel > 0 &&
            FleetExperience >= 0 &&
            ExperienceForNextLevel > 0 &&
            UnitsAcquired >= 0 &&
            UnitsOperational >= 0;

        public FleetProgression(
            string playerId)
        {
            PlayerId =
                playerId ?? string.Empty;

            FleetLevel = 1;
            FleetExperience = 0;
            ExperienceForNextLevel = 100;

            UnitsAcquired = 0;
            UnitsOperational = 0;
        }

        public double ProgressToNextLevel
        {
            get
            {
                if (ExperienceForNextLevel <= 0)
                    return 0.0;

                return Math.Min(
                    1.0,
                    (double)FleetExperience /
                    ExperienceForNextLevel);
            }
        }

        public void RegisterUnitAcquired()
        {
            if (UnitsAcquired < int.MaxValue)
                UnitsAcquired++;
        }

        public void RegisterUnitOperational()
        {
            if (UnitsOperational < int.MaxValue)
                UnitsOperational++;
        }

        public void RegisterUnitDecommissioned()
        {
            if (UnitsOperational > 0)
                UnitsOperational--;
        }

        public bool AddExperience(
            long amount)
        {
            if (amount <= 0)
                return false;

            if (FleetExperience >
                long.MaxValue - amount)
            {
                FleetExperience =
                    long.MaxValue;
            }
            else
            {
                FleetExperience += amount;
            }

            return true;
        }

        public bool CanLevelUp()
        {
            return FleetExperience >=
                   ExperienceForNextLevel;
        }

        public bool TryLevelUp()
        {
            if (!CanLevelUp())
                return false;

            while (FleetExperience >=
                   ExperienceForNextLevel)
            {
                FleetExperience -=
                    ExperienceForNextLevel;

                if (FleetLevel < int.MaxValue)
                    FleetLevel++;

                ExperienceForNextLevel =
                    CalculateNextLevelExperience(
                        FleetLevel);
            }

            return true;
        }

        public void SetProgression(
            int fleetLevel,
            long fleetExperience,
            long experienceForNextLevel)
        {
            FleetLevel =
                Math.Max(1, fleetLevel);

            FleetExperience =
                Math.Max(0L, fleetExperience);

            ExperienceForNextLevel =
                Math.Max(
                    1L,
                    experienceForNextLevel);
        }

        public void SetFleetCounts(
            int unitsAcquired,
            int unitsOperational)
        {
            UnitsAcquired =
                Math.Max(0, unitsAcquired);

            UnitsOperational =
                Math.Max(
                    0,
                    Math.Min(
                        unitsOperational,
                        UnitsAcquired));
        }

        public void Reset()
        {
            FleetLevel = 1;
            FleetExperience = 0;
            ExperienceForNextLevel = 100;

            UnitsAcquired = 0;
            UnitsOperational = 0;
        }

        private static long
            CalculateNextLevelExperience(
                int level)
        {
            if (level <= 1)
                return 100;

            long value =
                100L +
                ((long)(level - 1) * 75L);

            return Math.Max(1L, value);
        }
    }
}
