using System;

namespace ObsidianProtocol.Game.Progression
{
    public sealed class ProgressionBalance
    {
        public long PlayerBaseExperience { get; set; }
        public long PlayerExperiencePerLevel { get; set; }

        public long FleetBaseExperience { get; set; }
        public long FleetExperiencePerLevel { get; set; }

        public long TechnologyBaseExperience { get; set; }
        public long TechnologyExperiencePerLevel { get; set; }

        public long FacilityBaseExperience { get; set; }
        public long FacilityExperiencePerLevel { get; set; }

        public long FabricationBaseExperience { get; set; }
        public long FabricationExperiencePerLevel { get; set; }

        public long AIBaseExperience { get; set; }
        public long AIExperiencePerLevel { get; set; }

        public ProgressionBalance()
        {
            PlayerBaseExperience = 100;
            PlayerExperiencePerLevel = 50;

            FleetBaseExperience = 100;
            FleetExperiencePerLevel = 75;

            TechnologyBaseExperience = 100;
            TechnologyExperiencePerLevel = 100;

            FacilityBaseExperience = 100;
            FacilityExperiencePerLevel = 125;

            FabricationBaseExperience = 100;
            FabricationExperiencePerLevel = 150;

            AIBaseExperience = 100;
            AIExperiencePerLevel = 125;
        }

        public long GetPlayerExperience(
            int level)
        {
            return Calculate(
                PlayerBaseExperience,
                PlayerExperiencePerLevel,
                level);
        }

        public long GetFleetExperience(
            int level)
        {
            return Calculate(
                FleetBaseExperience,
                FleetExperiencePerLevel,
                level);
        }

        public long GetTechnologyExperience(
            int level)
        {
            return Calculate(
                TechnologyBaseExperience,
                TechnologyExperiencePerLevel,
                level);
        }

        public long GetFacilityExperience(
            int level)
        {
            return Calculate(
                FacilityBaseExperience,
                FacilityExperiencePerLevel,
                level);
        }

        public long GetFabricationExperience(
            int level)
        {
            return Calculate(
                FabricationBaseExperience,
                FabricationExperiencePerLevel,
                level);
        }

        public long GetAIExperience(
            int level)
        {
            return Calculate(
                AIBaseExperience,
                AIExperiencePerLevel,
                level);
        }

        public bool IsLevelValid(
            int level)
        {
            return level >= 1;
        }

        public bool IsProgressionValid(
            int level,
            long experience)
        {
            return level >= 1 &&
                   experience >= 0;
        }

        public int GetMaximumUnlockLevel(
            int playerLevel,
            int fleetLevel,
            int technologyLevel)
        {
            return Math.Max(
                1,
                Math.Min(
                    playerLevel,
                    Math.Min(
                        fleetLevel,
                        technologyLevel)));
        }

        public bool CanUnlockStandardContent(
            int playerLevel,
            int fleetLevel,
            int technologyLevel,
            int requiredPlayerLevel,
            int requiredFleetLevel,
            int requiredTechnologyLevel)
        {
            return playerLevel >=
                       Math.Max(
                           1,
                           requiredPlayerLevel) &&
                   fleetLevel >=
                       Math.Max(
                           1,
                           requiredFleetLevel) &&
                   technologyLevel >=
                       Math.Max(
                           0,
                           requiredTechnologyLevel);
        }

        public bool CanUnlockExperimentalContent(
            int playerLevel,
            int fleetLevel,
            int technologyLevel,
            int researchLevel,
            int requiredPlayerLevel,
            int requiredFleetLevel,
            int requiredTechnologyLevel,
            int requiredResearchLevel)
        {
            return playerLevel >=
                       Math.Max(
                           1,
                           requiredPlayerLevel) &&
                   fleetLevel >=
                       Math.Max(
                           1,
                           requiredFleetLevel) &&
                   technologyLevel >=
                       Math.Max(
                           1,
                           requiredTechnologyLevel) &&
                   researchLevel >=
                       Math.Max(
                           1,
                           requiredResearchLevel);
        }

        public bool IsCompetitiveAdvantage(
            bool changesDeploymentBudget,
            bool changesBattleBudget,
            bool directlyIncreasesCombatPower)
        {
            return
                changesDeploymentBudget ||
                changesBattleBudget ||
                directlyIncreasesCombatPower;
        }

        private static long Calculate(
            long baseExperience,
            long experiencePerLevel,
            int level)
        {
            if (level <= 1)
                return Math.Max(
                    1L,
                    baseExperience);

            long safeBase =
                Math.Max(
                    1L,
                    baseExperience);

            long safePerLevel =
                Math.Max(
                    0L,
                    experiencePerLevel);

            long levelOffset =
                level - 1L;

            if (safePerLevel > 0 &&
                levelOffset >
                (long.MaxValue - safeBase) /
                safePerLevel)
            {
                return long.MaxValue;
            }

            return safeBase +
                   (levelOffset *
                    safePerLevel);
        }
    }
}
