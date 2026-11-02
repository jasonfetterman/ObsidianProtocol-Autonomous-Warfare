using UnityEngine;
using ObsidianProtocol.Game.Core;
using ObsidianProtocol.Game.Inventory;
using ObsidianProtocol.Game.Economy;

namespace ObsidianProtocol.Game.Campaign
{
    public class CampaignState : MonoBehaviour
    {
        public static CampaignState Instance { get; private set; }

        public bool CampaignCreated { get; private set; }
        public string CampaignId { get; private set; }
        public string CommanderName { get; private set; }
        public string FactionId { get; private set; }

        public CampaignResources Resources { get; private set; }
        public PlayerProfile Profile { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

                public bool CreateNewCampaign(
            string commanderName,
            string factionId = "")
        {
            if (CampaignCreated)
            {
                Debug.Log("Campaign already created; skipping duplicate initialization.");
                return true;
            }

            CampaignCreated = false;

            CampaignId =
                System.Guid.NewGuid().ToString("N");

            if (string.IsNullOrWhiteSpace(commanderName))
                commanderName = "COMMANDER";

            CommanderName =
                commanderName.Trim();

            FactionId =
                factionId ?? string.Empty;

            PersistentGameState state =
                PersistentGameState.Instance;

            if (state == null)
            {
                GameObject stateObject =
                    new GameObject("[SYSTEM] PERSISTENT GAME STATE");

                state =
                    stateObject.AddComponent<PersistentGameState>();
            }

            state.Initialize(
                CampaignId,
                CommanderName,
                FactionId,
                state.PlayerId);

            state.SetCurrentPlanet(30);
            Profile = new PlayerProfile();
            Profile.Initialize(
                state.PlayerId,
                state.CommanderName,
                state.FactionId);

            if (!state.IsInitialized ||
                Profile == null ||
                !Profile.IsValid)
            {
                Debug.LogError("[CampaignState] Campaign identity/profile validation failed.");
                return false;
            }

            ObsidianRuntimeSystems runtime =
                ObsidianRuntimeSystems.Instance;

            if (runtime == null || runtime.Progression == null)
            {
                Debug.LogError(
                    "[CampaignState] Runtime progression system unavailable.");
                return false;
            }

            runtime.Progression.Create(state.PlayerId);
            runtime.Progression.SetPlayerProgression(1, 0);
            runtime.Progression.SetFleetProgression(1, 0);
            runtime.Progression.SetFabricationProgression(1, 0);
            runtime.Progression.SetAIProgression(1, 0);

            if (!runtime.Progression.HasData ||
                runtime.Progression.CurrentData == null ||
                runtime.Progression.CurrentData.PlayerId != state.PlayerId ||
                !runtime.Progression.Validate())
            {
                Debug.LogError("[CampaignState] Progression validation failed or player ID does not match.");
                return false;
            }

            Debug.Log(
                "[CampaignState] STARTING PROGRESSION INITIALIZED | " +
                "PLAYER=" + state.PlayerId +
                " | PLAYER_LEVEL=1 | FLEET_LEVEL=1" +
                " | FABRICATION_LEVEL=1 | AI_LEVEL=1");

            InventoryRuntime.StartNewCampaign(
                CampaignId,
                state.PlayerId);

            EconomyRuntime.StartNewCampaign(
                CampaignId,
                10000,
                state.PlayerId);

            InitializeStartingResources();

            Debug.Log(
                $"[CampaignState] NEW CAMPAIGN CREATED | " +
                $"ID={CampaignId} | " +
                $"COMMANDER={CommanderName} | " +
                $"FACTION={FactionId} | " +
                $"INVENTORY=EMPTY | " +
                $"CREDITS=10000 | " +
                $"RESOURCES=0"
            );

                        // Save initial campaign identity and starting funding.
            try
            {
                SaveData initialSave = new SaveData
                {
                    CampaignId = CampaignId,
                    CommanderName = CommanderName,
                    PlayerId = state.PlayerId,
                    FactionId = state.FactionId,
                    CurrentPlanetId = state.CurrentPlanetId,
                    Funding = 10000,
            Progression = ProgressionSnapshot.From(runtime.Progression.CurrentData),
                };

                SaveLoadService.Save(initialSave);

                Debug.Log(
                    "[CampaignState] INITIAL CAMPAIGN SAVE WRITTEN | " +
                    "CAMPAIGN=" + CampaignId +
                    " | PLAYER=" + state.PlayerId);
            }
            catch (System.Exception ex)
            {
                Debug.LogError(
                    "[CampaignState] Initial campaign save failed: " +
                    ex.Message);
            }

            CampaignCreated = true;
            return true;
        }

        private void InitializeStartingResources()
        {
            Resources =
                new CampaignResources();

            Resources.Initialize();

            Resources.RegisterResource("Meat", 0);
            Resources.RegisterResource("Wood", 0);
            Resources.RegisterResource("Coal", 0);
            Resources.RegisterResource("Iron", 0);
            Resources.RegisterResource("Alloy", 0);
            Resources.RegisterResource("Electronics", 0);
            Resources.RegisterResource("Fuel", 0);
            Resources.RegisterResource("Energy", 0);
        }

        public bool HasCampaign()
        {
            return CampaignCreated;
        }
    }
}








