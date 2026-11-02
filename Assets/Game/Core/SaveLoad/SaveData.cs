using System;
using System.Collections.Generic;
using ObsidianProtocol.Game.Progression;
using ObsidianProtocol.Game.Garage;
using ObsidianProtocol.Game.Deployment;

namespace ObsidianProtocol.Game.Core
{
    [Serializable]
    public sealed class SaveData
    {
        public int Version = 1;
        public string CampaignId;
        public string CommanderName;
        public string PlayerId;
        public string FactionId;
        public int CurrentPlanetId = 30;
        public int Funding;
        public ProgressionSnapshot Progression;
        public LogisticsSnapshot Logistics;
        public ResearchSnapshot Research;
        public FleetSnapshot Fleet;
        public WorldSimulationSnapshot WorldSimulation;
        public MilitaryHistorySnapshot MilitaryHistory;
        public OperationsSnapshot Operations;
    }

    [Serializable]
    public sealed class WorldSimulationSnapshot
    {
        public bool Initialized;
        public int State;
        public long CurrentTick;
        public float TickInterval;
    }

    [Serializable]
    public sealed class MilitaryHistorySnapshot
    {
        public List<MilitaryHistoryEntrySnapshot> Entries = new List<MilitaryHistoryEntrySnapshot>();
    }

    [Serializable]
    public sealed class MilitaryHistoryEntrySnapshot
    {
        public string MatchId;
        public string PlayerId;
        public string Result;
        public string RecordedAtUtc;
    }

    [Serializable]
    public sealed class OperationsSnapshot
    {
        public string OperationId;
        public string SelectedServiceRecordId;
        public List<DeploymentEntrySnapshot> Entries = new List<DeploymentEntrySnapshot>();
    }

    [Serializable]
    public sealed class DeploymentEntrySnapshot
    {
        public string DeploymentId;
        public string ServiceRecordId;
        public string UnitId;
        public int DeploymentPoints;
        public int State;
    }
    public sealed class FleetSnapshot
    {
        public List<FleetUnitSnapshot> Units = new List<FleetUnitSnapshot>();
    }

    [Serializable]
    public sealed class FleetUnitSnapshot
    {
        public string OwnershipId;
        public string UnitId;
        public FleetUnitStatus Status;
        public string GarageSlotId;
    }
    [Serializable]
    public sealed class ResearchSnapshot
    {
        public List<string> UnlockedTechnologyIds = new List<string>();
        public string CurrentTechnologyId;
        public float ResearchElapsed;
        public bool IsResearching;
    }

    [Serializable]
    public sealed class LogisticsSnapshot
    {
        public List<ResourceAmountEntry> GlobalResources =
            new List<ResourceAmountEntry>();

        public List<StorageInventorySnapshot> Storages =
            new List<StorageInventorySnapshot>();

        public List<DepotInventorySnapshot> Depots =
            new List<DepotInventorySnapshot>();
    }

    [Serializable]
    public sealed class ResourceAmountEntry
    {
        public string Key;
        public int Amount;
    }

    [Serializable]
    public sealed class StorageInventorySnapshot
    {
        public string StorageId;
        public List<ResourceAmountEntry> Amounts =
            new List<ResourceAmountEntry>();
    }

    [Serializable]
    public sealed class SupplyAmountEntry
    {
        public string SupplyType;
        public float Amount;
    }

    [Serializable]
    public sealed class DepotInventorySnapshot
    {
        public string DepotId;
        public List<SupplyAmountEntry> Amounts =
            new List<SupplyAmountEntry>();
    }

    [Serializable]
    public sealed class ProgressionIntEntry
    {
        public string Key;
        public int Value;
    }

    [Serializable]
    public sealed class ProgressionBoolEntry
    {
        public string Key;
        public bool Value;
    }

    [Serializable]
    public sealed class ProgressionSnapshot
    {
        public string PlayerId;
        public int PlayerLevel = 1;
        public long PlayerExperience;
        public int FleetLevel = 1;
        public long FleetExperience;
        public int FabricationLevel = 1;
        public long FabricationExperience;
        public int AILevel = 1;
        public long AIExperience;

        public List<ProgressionIntEntry> TechnologyLevels = new List<ProgressionIntEntry>();
        public List<ProgressionIntEntry> FacilityLevels = new List<ProgressionIntEntry>();
        public List<ProgressionBoolEntry> UnitUnlocks = new List<ProgressionBoolEntry>();
        public List<ProgressionBoolEntry> EquipmentUnlocks = new List<ProgressionBoolEntry>();
        public List<ProgressionBoolEntry> ExperimentalUnlocks = new List<ProgressionBoolEntry>();
        public List<ProgressionIntEntry> Reputation = new List<ProgressionIntEntry>();
        public List<string> Achievements = new List<string>();

        public static ProgressionSnapshot From(ProgressionSaveData source)
        {
            if (source == null)
                return null;

            var snapshot = new ProgressionSnapshot
            {
                PlayerId = source.PlayerId,
                PlayerLevel = source.PlayerLevel,
                PlayerExperience = source.PlayerExperience,
                FleetLevel = source.FleetLevel,
                FleetExperience = source.FleetExperience,
                FabricationLevel = source.FabricationLevel,
                FabricationExperience = source.FabricationExperience,
                AILevel = source.AILevel,
                AIExperience = source.AIExperience
            };

            if (source.TechnologyLevels != null)
                foreach (var pair in source.TechnologyLevels)
                    snapshot.TechnologyLevels.Add(new ProgressionIntEntry { Key = pair.Key, Value = pair.Value });

            if (source.FacilityLevels != null)
                foreach (var pair in source.FacilityLevels)
                    snapshot.FacilityLevels.Add(new ProgressionIntEntry { Key = pair.Key, Value = pair.Value });

            if (source.UnitUnlocks != null)
                foreach (var pair in source.UnitUnlocks)
                    snapshot.UnitUnlocks.Add(new ProgressionBoolEntry { Key = pair.Key, Value = pair.Value });

            if (source.EquipmentUnlocks != null)
                foreach (var pair in source.EquipmentUnlocks)
                    snapshot.EquipmentUnlocks.Add(new ProgressionBoolEntry { Key = pair.Key, Value = pair.Value });

            if (source.ExperimentalUnlocks != null)
                foreach (var pair in source.ExperimentalUnlocks)
                    snapshot.ExperimentalUnlocks.Add(new ProgressionBoolEntry { Key = pair.Key, Value = pair.Value });

            if (source.Reputation != null)
                foreach (var pair in source.Reputation)
                    snapshot.Reputation.Add(new ProgressionIntEntry { Key = pair.Key, Value = pair.Value });

            if (source.Achievements != null)
                snapshot.Achievements.AddRange(source.Achievements);

            return snapshot;
        }

        public ProgressionSaveData ToProgressionSaveData()
        {
            var result = new ProgressionSaveData
            {
                PlayerId = PlayerId ?? string.Empty,
                PlayerLevel = PlayerLevel,
                PlayerExperience = PlayerExperience,
                FleetLevel = FleetLevel,
                FleetExperience = FleetExperience,
                FabricationLevel = FabricationLevel,
                FabricationExperience = FabricationExperience,
                AILevel = AILevel,
                AIExperience = AIExperience
            };

            if (TechnologyLevels != null)
                foreach (var entry in TechnologyLevels)
                    if (entry != null && !string.IsNullOrWhiteSpace(entry.Key))
                        result.TechnologyLevels[entry.Key] = entry.Value;

            if (FacilityLevels != null)
                foreach (var entry in FacilityLevels)
                    if (entry != null && !string.IsNullOrWhiteSpace(entry.Key))
                        result.FacilityLevels[entry.Key] = entry.Value;

            if (UnitUnlocks != null)
                foreach (var entry in UnitUnlocks)
                    if (entry != null && !string.IsNullOrWhiteSpace(entry.Key))
                        result.UnitUnlocks[entry.Key] = entry.Value;

            if (EquipmentUnlocks != null)
                foreach (var entry in EquipmentUnlocks)
                    if (entry != null && !string.IsNullOrWhiteSpace(entry.Key))
                        result.EquipmentUnlocks[entry.Key] = entry.Value;

            if (ExperimentalUnlocks != null)
                foreach (var entry in ExperimentalUnlocks)
                    if (entry != null && !string.IsNullOrWhiteSpace(entry.Key))
                        result.ExperimentalUnlocks[entry.Key] = entry.Value;

            if (Reputation != null)
                foreach (var entry in Reputation)
                    if (entry != null && !string.IsNullOrWhiteSpace(entry.Key))
                        result.Reputation[entry.Key] = entry.Value;

            if (Achievements != null)
                result.Achievements.AddRange(Achievements);

            return result;
        }
    }
}
