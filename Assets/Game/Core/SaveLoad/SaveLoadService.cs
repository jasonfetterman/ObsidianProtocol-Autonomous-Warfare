using System.IO;
using ObsidianProtocol.Game.OnlineWorld;
using ObsidianProtocol.Game.OpenWorld;
using ObsidianProtocol.Game.Core;

using ObsidianProtocol.Game.Progression;
using ObsidianProtocol.Game.Garage;
using ObsidianProtocol.Game.Research;
using ObsidianProtocol.Game.Deployment;
using UnityEngine;

namespace ObsidianProtocol.Game.Core
{
    public static class SaveLoadService
    {
        private const string FileName = "obsidian_protocol_save.json";

        private static string SavePath =>
            Path.Combine(Application.persistentDataPath, FileName);

        public static void Save(SaveData data)
        {
            if (data == null)
                throw new System.ArgumentNullException(nameof(data));

            PersistentGameState state = PersistentGameState.Instance;
            if (state != null && state.IsInitialized)
                state.CopyToSaveData(data);

            // PROGRESSION_SNAPSHOT_SYNC: capture the active campaign progression.
            ObsidianRuntimeSystems runtime = ObsidianRuntimeSystems.Instance;
            if (runtime != null &&
                runtime.Initialized &&
                runtime.Progression != null &&
                runtime.Progression.HasData &&
                state != null &&
                state.IsInitialized &&
                runtime.Progression.CurrentData.PlayerId == state.PlayerId)
            {
                data.Progression = ProgressionSnapshot.From(
                    runtime.Progression.CurrentData);
            }

            ObsidianProtocol.Game.Logistics.LogisticsRuntime logistics =
                ObsidianProtocol.Game.Logistics.LogisticsRuntime.Instance;

            if (logistics != null && logistics.Initialized)
            {
                data.Logistics = logistics.CaptureSnapshot();
            }

            ResearchManager research = ResearchManager.Instance;
            if (research != null)
            {
                data.Research = research.CaptureSnapshot();
            }

            if (runtime != null && runtime.Initialized && runtime.FleetManagement != null)
            {
                FleetSnapshot fleetSnapshot = new FleetSnapshot();
                foreach (ObsidianProtocol.Game.Garage.FleetUnit unit in runtime.FleetManagement.GetFleet())
                {
                    if (unit == null) continue;
                    fleetSnapshot.Units.Add(new FleetUnitSnapshot
                    {
                        OwnershipId = unit.OwnershipId,
                        UnitId = unit.UnitId,
                        Status = unit.Status,
                        GarageSlotId = unit.GarageSlotId
                    });
                }
                data.Fleet = fleetSnapshot;
            }
            ObsidianProtocol.Game.Core.GameBootstrap bootstrap = ObsidianProtocol.Game.Core.GameBootstrap.Instance;
            if (bootstrap != null)
            {
                if (bootstrap.WorldSimulation != null &&
                    bootstrap.WorldSimulation.Initialized)
                {
                    data.WorldSimulation = new WorldSimulationSnapshot
                    {
                        Initialized = true,
                        State = (int)bootstrap.WorldSimulation.State,
                        CurrentTick = bootstrap.WorldSimulation.CurrentTick,
                        TickInterval = bootstrap.WorldSimulation.TickInterval
                    };
                }

                if (bootstrap.MilitaryHistory != null &&
                    bootstrap.MilitaryHistory.Initialized)
                {
                    MilitaryHistorySnapshot historySnapshot =
                        new MilitaryHistorySnapshot();

                    foreach (MatchHistoryEntry entry in
                        bootstrap.MilitaryHistory.GetHistory())
                    {
                        if (entry == null) continue;
                        historySnapshot.Entries.Add(
                            new MilitaryHistoryEntrySnapshot
                            {
                                MatchId = entry.MatchId,
                                PlayerId = entry.PlayerId,
                                Result = entry.Result,
                                RecordedAtUtc = entry.RecordedAtUtc.ToString("o")
                            });
                    }

                    data.MilitaryHistory = historySnapshot;
                }
            }
            ObsidianProtocol.Game.Economy.EconomyRuntime.Save();

            if (BattlefieldDeploymentContext.Instance != null)
                data.Operations = BattlefieldDeploymentContext.Instance.CaptureSnapshot();

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
        }

        public static bool SaveFleetSnapshot()
        {
            try
            {
                PersistentGameState state = PersistentGameState.Instance;

                if (state == null || !state.IsInitialized)
                {
                    Debug.LogWarning("[SaveLoadService] Fleet save skipped: campaign state is unavailable.");
                    return false;
                }

                ObsidianRuntimeSystems runtime = ObsidianRuntimeSystems.Instance;

                if (runtime == null ||
                    !runtime.Initialized ||
                    runtime.FleetManagement == null)
                {
                    Debug.LogWarning("[SaveLoadService] Fleet save skipped: fleet management is unavailable.");
                    return false;
                }

                if (!File.Exists(SavePath))
                {
                    Debug.LogWarning("[SaveLoadService] Fleet save skipped: campaign save file does not exist.");
                    return false;
                }

                string json = File.ReadAllText(SavePath);

                if (string.IsNullOrWhiteSpace(json))
                    return false;

                SaveData data = JsonUtility.FromJson<SaveData>(json);

                if (data == null ||
                    string.IsNullOrWhiteSpace(data.CampaignId) ||
                    string.IsNullOrWhiteSpace(data.PlayerId) ||
                    data.CampaignId != state.CampaignId ||
                    data.PlayerId != state.PlayerId)
                {
                    Debug.LogWarning("[SaveLoadService] Fleet save skipped: campaign or player identity mismatch.");
                    return false;
                }

                FleetSnapshot snapshot = new FleetSnapshot();

                foreach (FleetUnit unit in runtime.FleetManagement.GetFleet())
                {
                    if (unit == null ||
                        string.IsNullOrWhiteSpace(unit.OwnershipId) ||
                        string.IsNullOrWhiteSpace(unit.UnitId))
                    {
                        continue;
                    }

                    snapshot.Units.Add(new FleetUnitSnapshot
                    {
                        OwnershipId = unit.OwnershipId,
                        UnitId = unit.UnitId,
                        Status = unit.Status,
                        GarageSlotId = unit.GarageSlotId
                    });
                }

                data.Fleet = snapshot;

                File.WriteAllText(SavePath, JsonUtility.ToJson(data, true));

                Debug.Log("[SaveLoadService] Fleet snapshot saved. Units=" +
                    snapshot.Units.Count + " | Campaign=" + data.CampaignId);

                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError("[SaveLoadService] Fleet snapshot save failed: " + ex.Message);
                return false;
            }
        }
        public static bool SaveResearchSnapshot(ResearchSnapshot snapshot)
        {
            if (snapshot == null || !File.Exists(SavePath))
                return false;

            try
            {
                string json = File.ReadAllText(SavePath);
                SaveData data = JsonUtility.FromJson<SaveData>(json);

                if (data == null)
                    return false;

                PersistentGameState state = PersistentGameState.Instance;
                if (state != null && state.IsInitialized &&
                    (data.CampaignId != state.CampaignId ||
                     data.PlayerId != state.PlayerId))
                {
                    Debug.LogWarning(
                        "[SaveLoadService] Research save skipped: campaign or player identity mismatch.");
                    return false;
                }

                data.Research = snapshot;
                File.WriteAllText(SavePath, JsonUtility.ToJson(data, true));

                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError(
                    "[SaveLoadService] Research snapshot save failed: " + ex.Message);
                return false;
            }
        }
        public static SaveData Load()
        {
            if (!File.Exists(SavePath))
                return new SaveData();

            string json = File.ReadAllText(SavePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            if (data == null)
                return new SaveData();

            PersistentGameState state = PersistentGameState.Instance;

            if (state == null &&
                !string.IsNullOrWhiteSpace(data.CampaignId) &&
                !string.IsNullOrWhiteSpace(data.PlayerId))
            {
                GameObject stateObject = new GameObject("PersistentGameState");
                state = stateObject.AddComponent<PersistentGameState>();
            }

            if (state != null && state.IsInitialized &&
                !string.IsNullOrWhiteSpace(data.CampaignId))
            {
                ObsidianProtocol.Game.Economy.EconomyRuntime.Load(data.CampaignId);
            }
            if (state != null && !state.RestoreFromSaveData(data))
                Debug.LogWarning("[SaveLoadService] Save loaded, but campaign identity or planet data was invalid.");

            // Restore saved fleet only when a fleet snapshot exists.
            if (data.Fleet != null && data.Fleet.Units != null && state != null && state.IsInitialized && state.CampaignId == data.CampaignId && state.PlayerId == data.PlayerId && data.CurrentPlanetId >= 1 && data.CurrentPlanetId <= 30)
            {
                ObsidianRuntimeSystems fleetRuntime = ObsidianRuntimeSystems.Instance;
                if (fleetRuntime != null && fleetRuntime.Initialized && fleetRuntime.FleetManagement != null)
                {
                    fleetRuntime.FleetManagement.Clear();
                    foreach (FleetUnitSnapshot savedUnit in data.Fleet.Units)
                    {
                        if (savedUnit == null || string.IsNullOrWhiteSpace(savedUnit.OwnershipId) || string.IsNullOrWhiteSpace(savedUnit.UnitId))
                            continue;
                        FleetUnit unit = new FleetUnit(savedUnit.OwnershipId, savedUnit.UnitId);
                        unit.SetStatus(savedUnit.Status);
                        unit.AssignSlot(savedUnit.GarageSlotId);
                        fleetRuntime.FleetManagement.Register(unit);
                    }
                }
                else
                {
                    Debug.LogWarning("[SaveLoadService] Fleet snapshot found, but fleet management is not initialized; restore deferred.");
                }
            }
            // PROGRESSION_SNAPSHOT_SYNC: restore only matching campaign-player progression.
            ObsidianRuntimeSystems runtime = ObsidianRuntimeSystems.Instance;
            if (runtime != null &&
                runtime.Initialized &&
                runtime.Progression != null &&
                data.Progression != null &&
                !string.IsNullOrWhiteSpace(data.PlayerId))
            {
                ProgressionSaveData savedProgression =
                    data.Progression.ToProgressionSaveData();

                if (savedProgression != null &&
                    savedProgression.PlayerId == data.PlayerId &&
                    state != null &&
                    state.IsInitialized &&
                    state.PlayerId == savedProgression.PlayerId)
                {
                    runtime.Progression.Save(savedProgression);
                }
                else
                {
                    Debug.LogWarning("[SaveLoadService] Progression restore skipped: saved player identity did not match.");
                }
            }

            ResearchManager research = ResearchManager.Instance;
            if (data.Research != null)
            {
                if (research != null)
                {
                    research.RestoreSnapshot(data.Research);
                }
                else
                {
                    Debug.LogWarning(
                        "[SaveLoadService] Research snapshot found, but ResearchManager is not initialized.");
                }
            }

            ObsidianProtocol.Game.Logistics.LogisticsRuntime logistics =
                ObsidianProtocol.Game.Logistics.LogisticsRuntime.Instance;

            if (data.Logistics != null)
            {
                if (logistics != null && logistics.Initialized)
                {
                    logistics.RestoreSnapshot(data.Logistics);
                }
                else
                {
                    Debug.LogWarning(
                        "[SaveLoadService] Logistics snapshot found, but logistics runtime is not initialized; restore deferred.");
                }
            }

            ObsidianProtocol.Game.Core.GameBootstrap bootstrap = ObsidianProtocol.Game.Core.GameBootstrap.Instance;
            if (bootstrap != null)
            {
                if (data.WorldSimulation != null &&
                    data.WorldSimulation.Initialized &&
                    bootstrap.WorldSimulation != null &&
                    bootstrap.WorldSimulation.Initialized &&
                    data.WorldSimulation.TickInterval > 0f &&
                    data.WorldSimulation.CurrentTick >= 0 &&
                    System.Enum.IsDefined(
                        typeof(WorldSimulationState),
                        data.WorldSimulation.State))
                {
                    WorldSimulation savedWorld = new WorldSimulation();
                    if (savedWorld.Initialize(data.WorldSimulation.TickInterval))
                    {
                        savedWorld.SetTick(data.WorldSimulation.CurrentTick);
                        WorldSimulationState savedState =
                            (WorldSimulationState)data.WorldSimulation.State;

                        if (savedState == WorldSimulationState.Running)
                            savedWorld.Start();
                        else if (savedState == WorldSimulationState.Paused)
                        {
                            savedWorld.Start();
                            savedWorld.Pause();
                        }

                        bootstrap.RestoreWorldSimulation(savedWorld);
                        Debug.Log("[SaveLoadService] World simulation snapshot restored.");
                    }
                }

                if (data.MilitaryHistory != null &&
                    data.MilitaryHistory.Entries != null &&
                    bootstrap.MilitaryHistory != null &&
                    bootstrap.MilitaryHistory.Initialized)
                {
                    bootstrap.MilitaryHistory.Reset();
                    bootstrap.MilitaryHistory.Initialize();

                    foreach (MilitaryHistoryEntrySnapshot entry in
                        data.MilitaryHistory.Entries)
                    {
                        if (entry == null ||
                            !System.DateTime.TryParse(
                                entry.RecordedAtUtc,
                                null,
                                System.Globalization.DateTimeStyles.RoundtripKind,
                                out System.DateTime recordedAtUtc))
                            continue;

                        bootstrap.MilitaryHistory.RestoreEntry(
                            entry.MatchId,
                            entry.PlayerId,
                            entry.Result,
                            recordedAtUtc);
                    }

                    Debug.Log(
                        "[SaveLoadService] Military history snapshot restored. Entries=" +
                        bootstrap.MilitaryHistory.MatchCount);
                }
            }
            if (data.Operations != null)
            {
                if (BattlefieldDeploymentContext.Instance != null)
                    BattlefieldDeploymentContext.Instance.RestoreSnapshot(data.Operations);
                else
                    Debug.LogError("[SaveLoadService] Deployment context missing; operations not restored.");
            }

            return data;
        }

        public static bool Exists()
        {
            return File.Exists(SavePath);
        }

        public static void Delete()
        {
            if (File.Exists(SavePath))
                File.Delete(SavePath);
        }
    }
}






