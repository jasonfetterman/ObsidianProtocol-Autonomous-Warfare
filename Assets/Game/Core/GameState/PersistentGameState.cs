using UnityEngine;

namespace ObsidianProtocol.Game.Core
{
    public sealed class PersistentGameState : MonoBehaviour
    {
        public static PersistentGameState Instance { get; private set; }

        [Header("Campaign")]
        [SerializeField] private string campaignId;

        [Header("Player")]
        [SerializeField] private string playerId;

        [Header("Commander")]
        [SerializeField] private string commanderName;

        [Header("Faction")]
        [SerializeField] private string factionId;

        public string CampaignId => campaignId;
        public string PlayerId => playerId;
        public string CommanderName => commanderName;
        public string FactionId => factionId;
        public int CurrentPlanetId { get; private set; } = 30;

        public bool IsInitialized =>
            !string.IsNullOrEmpty(campaignId);

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

        public void Initialize(
            string id,
            string commander,
            string faction,
            string activePlayerId = null)
        {
            campaignId = id;

            if (!string.IsNullOrWhiteSpace(activePlayerId))
                playerId = activePlayerId.Trim();
            else if (string.IsNullOrEmpty(playerId))
                playerId = System.Guid.NewGuid().ToString("N");

            commanderName =
                string.IsNullOrWhiteSpace(commander)
                    ? "COMMANDER"
                    : commander.Trim();

            factionId =
                string.IsNullOrWhiteSpace(faction)
                    ? string.Empty
                    : faction.Trim();
        }

        public void SetCurrentPlanet(int planetId)
        {
            if (planetId < 1 || planetId > 30)
                throw new System.ArgumentOutOfRangeException(nameof(planetId));
            CurrentPlanetId = planetId;
        }

        public void CopyToSaveData(SaveData data)
        {
            if (data == null)
                throw new System.ArgumentNullException(nameof(data));

            if (!IsInitialized)
                return;

            data.CampaignId = campaignId;
            data.PlayerId = playerId;
            data.CommanderName = commanderName;
            data.FactionId = factionId;
            data.CurrentPlanetId = CurrentPlanetId;
        }

        public bool RestoreFromSaveData(SaveData data)
        {
            if (data == null ||
                string.IsNullOrWhiteSpace(data.CampaignId) ||
                string.IsNullOrWhiteSpace(data.PlayerId) ||
                data.CurrentPlanetId < 1 ||
                data.CurrentPlanetId > 30)
                return false;

            Initialize(
                data.CampaignId,
                data.CommanderName,
                data.FactionId,
                data.PlayerId);

            SetCurrentPlanet(data.CurrentPlanetId);
            return IsInitialized;
        }

        public void Clear()
        {
            campaignId = string.Empty;
            playerId = string.Empty;
            commanderName = string.Empty;
            factionId = string.Empty;
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}



