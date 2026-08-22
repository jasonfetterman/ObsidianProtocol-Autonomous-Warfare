using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.Progression
{
    public sealed class ProgressionSaveData
    {
        public string PlayerId { get; set; }

        public int PlayerLevel { get; set; }
        public long PlayerExperience { get; set; }

        public int FleetLevel { get; set; }
        public long FleetExperience { get; set; }

        public int FabricationLevel { get; set; }
        public long FabricationExperience { get; set; }

        public int AILevel { get; set; }
        public long AIExperience { get; set; }

        public Dictionary<
            string,
            int> TechnologyLevels { get; set; }

        public Dictionary<
            string,
            int> FacilityLevels { get; set; }

        public Dictionary<
            string,
            bool> UnitUnlocks { get; set; }

        public Dictionary<
            string,
            bool> EquipmentUnlocks { get; set; }

        public Dictionary<
            string,
            bool> ExperimentalUnlocks { get; set; }

        public Dictionary<
            string,
            int> Reputation { get; set; }

        public List<string> Achievements { get; set; }

        public ProgressionSaveData()
        {
            PlayerId = string.Empty;

            TechnologyLevels =
                new Dictionary<string, int>(
                    StringComparer.OrdinalIgnoreCase);

            FacilityLevels =
                new Dictionary<string, int>(
                    StringComparer.OrdinalIgnoreCase);

            UnitUnlocks =
                new Dictionary<string, bool>(
                    StringComparer.OrdinalIgnoreCase);

            EquipmentUnlocks =
                new Dictionary<string, bool>(
                    StringComparer.OrdinalIgnoreCase);

            ExperimentalUnlocks =
                new Dictionary<string, bool>(
                    StringComparer.OrdinalIgnoreCase);

            Reputation =
                new Dictionary<string, int>(
                    StringComparer.OrdinalIgnoreCase);

            Achievements =
                new List<string>();
        }
    }

    public sealed class ProgressionPersistence
    {
        private ProgressionSaveData data;

        public ProgressionSaveData CurrentData =>
            data;

        public bool HasData =>
            data != null &&
            !string.IsNullOrWhiteSpace(
                data.PlayerId);

        public ProgressionPersistence()
        {
            data = null;
        }

        public void Create(
            string playerId)
        {
            data =
                new ProgressionSaveData();

            data.PlayerId =
                playerId ?? string.Empty;
        }

        public bool Save(
            ProgressionSaveData saveData)
        {
            if (saveData == null ||
                string.IsNullOrWhiteSpace(
                    saveData.PlayerId))
            {
                return false;
            }

            data = saveData;
            return true;
        }

        public bool Load(
            out ProgressionSaveData saveData)
        {
            saveData = data;

            return HasData;
        }

        public bool Clear()
        {
            if (!HasData)
                return false;

            data = null;
            return true;
        }

        public bool Validate()
        {
            if (!HasData)
                return false;

            if (data.PlayerLevel < 1 ||
                data.PlayerExperience < 0)
            {
                return false;
            }

            if (data.FleetLevel < 1 ||
                data.FleetExperience < 0)
            {
                return false;
            }

            if (data.FabricationLevel < 1 ||
                data.FabricationExperience < 0)
            {
                return false;
            }

            if (data.AILevel < 1 ||
                data.AIExperience < 0)
            {
                return false;
            }

            return true;
        }

        public void SetPlayerProgression(
            int level,
            long experience)
        {
            if (!HasData)
                return;

            data.PlayerLevel =
                Math.Max(1, level);

            data.PlayerExperience =
                Math.Max(0L, experience);
        }

        public void SetFleetProgression(
            int level,
            long experience)
        {
            if (!HasData)
                return;

            data.FleetLevel =
                Math.Max(1, level);

            data.FleetExperience =
                Math.Max(0L, experience);
        }

        public void SetFabricationProgression(
            int level,
            long experience)
        {
            if (!HasData)
                return;

            data.FabricationLevel =
                Math.Max(1, level);

            data.FabricationExperience =
                Math.Max(0L, experience);
        }

        public void SetAIProgression(
            int level,
            long experience)
        {
            if (!HasData)
                return;

            data.AILevel =
                Math.Max(1, level);

            data.AIExperience =
                Math.Max(0L, experience);
        }

        public void SetTechnologyLevel(
            string technologyId,
            int level)
        {
            if (!HasData ||
                string.IsNullOrWhiteSpace(
                    technologyId))
            {
                return;
            }

            data.TechnologyLevels[
                technologyId] =
                Math.Max(1, level);
        }

        public void SetFacilityLevel(
            string facilityId,
            int level)
        {
            if (!HasData ||
                string.IsNullOrWhiteSpace(
                    facilityId))
            {
                return;
            }

            data.FacilityLevels[
                facilityId] =
                Math.Max(1, level);
        }

        public void SetUnitUnlocked(
            string unitId,
            bool unlocked)
        {
            if (!HasData ||
                string.IsNullOrWhiteSpace(
                    unitId))
            {
                return;
            }

            data.UnitUnlocks[
                unitId] =
                unlocked;
        }

        public void SetEquipmentUnlocked(
            string equipmentId,
            bool unlocked)
        {
            if (!HasData ||
                string.IsNullOrWhiteSpace(
                    equipmentId))
            {
                return;
            }

            data.EquipmentUnlocks[
                equipmentId] =
                unlocked;
        }

        public void SetExperimentalUnlocked(
            string unitId,
            bool unlocked)
        {
            if (!HasData ||
                string.IsNullOrWhiteSpace(
                    unitId))
            {
                return;
            }

            data.ExperimentalUnlocks[
                unitId] =
                unlocked;
        }

        public void SetReputation(
            string category,
            int value)
        {
            if (!HasData ||
                string.IsNullOrWhiteSpace(
                    category))
            {
                return;
            }

            data.Reputation[
                category] =
                Math.Max(0, value);
        }

        public void RecordAchievement(
            string achievementId)
        {
            if (!HasData ||
                string.IsNullOrWhiteSpace(
                    achievementId))
            {
                return;
            }

            foreach (string existing
                in data.Achievements)
            {
                if (string.Equals(
                        existing,
                        achievementId,
                        StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }
            }

            data.Achievements.Add(
                achievementId);
        }
    }
}
