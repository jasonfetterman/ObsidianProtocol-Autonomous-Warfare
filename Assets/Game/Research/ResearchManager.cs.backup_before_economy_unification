using System;
using UnityEngine;
using ObsidianProtocol.Game.Technology;
using ObsidianProtocol.Game.Economy;
using ObsidianProtocol.Game.Core;

namespace ObsidianProtocol.Game.Research
{
    public sealed class ResearchManager : MonoBehaviour
    {
        public static ResearchManager Instance { get; private set; }

        private TechnologyInventory technologyInventory =
            new TechnologyInventory();

        private TechnologyDefinition currentTechnology;
        private float researchElapsed;
        private bool researching;

        public TechnologyDefinition CurrentTechnology => currentTechnology;
        public float ResearchElapsed => researchElapsed;
        public bool IsResearching => researching;

        public float ResearchProgress
        {
            get
            {
                if (currentTechnology == null)
                    return 0f;

                if (currentTechnology.ResearchTimeSeconds <= 0f)
                    return 0f;

                return Mathf.Clamp01(
                    researchElapsed /
                    currentTechnology.ResearchTimeSeconds);
            }
        }

        public event Action<TechnologyDefinition> ResearchStarted;
        public event Action<TechnologyDefinition> ResearchCompleted;

        private string pendingTechnologyId;
        private float pendingResearchElapsed;
        private bool pendingResearchActive;
        private float saveCheckpointElapsed;
        private const float SaveCheckpointInterval = 10f;

        private void SaveResearchCheckpoint()
        {
            if (PersistentGameState.Instance == null ||
                !PersistentGameState.Instance.IsInitialized)
                return;

            SaveLoadService.SaveResearchSnapshot(CaptureSnapshot());
        }

        public ResearchSnapshot CaptureSnapshot()
        {
            return new ResearchSnapshot
            {
                UnlockedTechnologyIds = technologyInventory.GetUnlockedIds(),
                CurrentTechnologyId = currentTechnology != null
                    ? currentTechnology.TechnologyId
                    : pendingTechnologyId,
                ResearchElapsed = currentTechnology != null
                    ? researchElapsed
                    : pendingResearchElapsed,
                IsResearching = researching || pendingResearchActive
            };
        }

        public void RestoreSnapshot(ResearchSnapshot snapshot)
        {
            if (snapshot == null)
                return;

            technologyInventory.RestoreUnlockedIds(
                snapshot.UnlockedTechnologyIds);

            currentTechnology = null;
            researchElapsed = 0f;
            researching = false;

            pendingTechnologyId = snapshot.CurrentTechnologyId;
            pendingResearchElapsed = Mathf.Max(0f, snapshot.ResearchElapsed);
            pendingResearchActive = snapshot.IsResearching &&
                !string.IsNullOrWhiteSpace(snapshot.CurrentTechnologyId);

            Debug.Log("[RESEARCH] Saved research snapshot restored.");

            if (pendingResearchActive)
                TryResumePendingResearch();
        }

        private void TryResumePendingResearch()
        {
            if (!pendingResearchActive ||
                string.IsNullOrWhiteSpace(pendingTechnologyId))
                return;

            TechnologyDefinition[] technologies =
                UnityEngine.Resources.LoadAll<TechnologyDefinition>("Technology");

            foreach (TechnologyDefinition technology in technologies)
            {
                if (technology != null &&
                    technology.TechnologyId == pendingTechnologyId)
                {
                    RegisterTechnology(technology);
                    return;
                }
            }

            Debug.LogWarning(
                $"[RESEARCH] Saved technology '{pendingTechnologyId}' was not found in Resources/Technology.");
        }

        public void RegisterTechnology(TechnologyDefinition technology)
        {
            if (technology == null || !technology.IsValid())
                return;

            if (!pendingResearchActive ||
                technology.TechnologyId != pendingTechnologyId)
                return;

            currentTechnology = technology;
            researchElapsed = Mathf.Clamp(
                pendingResearchElapsed,
                0f,
                technology.ResearchTimeSeconds);
            researching = true;

            pendingTechnologyId = null;
            pendingResearchElapsed = 0f;
            pendingResearchActive = false;

            Debug.Log($"[RESEARCH] Resumed saved research: {technology.DisplayName}");
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Update()
        {
            if (!researching || currentTechnology == null)
                return;

            researchElapsed += Time.deltaTime;
            saveCheckpointElapsed += Time.deltaTime;

            if (saveCheckpointElapsed >= SaveCheckpointInterval)
            {
                saveCheckpointElapsed = 0f;
                SaveResearchCheckpoint();
            }

            if (researchElapsed >=
                currentTechnology.ResearchTimeSeconds)
            {
                CompleteResearch();
            }
        }

        public bool IsUnlocked(
            TechnologyDefinition technology)
        {
            if (technology == null)
                return false;

            return technologyInventory.IsUnlocked(
                technology.TechnologyId);
        }

        public bool StartResearch(
            TechnologyDefinition technology)
        {
            if (technology == null ||
                !technology.IsValid() ||
                researching)
            {
                return false;
            }

            if (IsUnlocked(technology))
                return false;

            if (!PrerequisitesComplete(technology))
                return false;

            FundingManager funding = FundingManager.Instance;

            if (funding == null ||
                !funding.TrySpend(technology.ResearchCost))
            {
                Debug.LogWarning(
                    "[RESEARCH] Cannot start research: funding unavailable or insufficient.");
                return false;
            }

            Debug.Log(
                $"[RESEARCH] Funding paid: {technology.ResearchCost:N0}.");

            currentTechnology = technology;
            researchElapsed = 0f;
            researching = true;

            saveCheckpointElapsed = 0f;
            SaveResearchCheckpoint();

            ResearchStarted?.Invoke(
                currentTechnology);

            Debug.Log(
                $"[RESEARCH] Started: {technology.DisplayName}");

            return true;
        }

        public bool CancelResearch()
        {
            if (!researching && !pendingResearchActive)
                return false;

            currentTechnology = null;
            researchElapsed = 0f;
            researching = false;

            pendingTechnologyId = null;
            pendingResearchElapsed = 0f;
            pendingResearchActive = false;
            saveCheckpointElapsed = 0f;

            SaveResearchCheckpoint();

            Debug.Log("[RESEARCH] Research cancelled.");
            return true;
        }
        public bool PrerequisitesComplete(
            TechnologyDefinition technology)
        {
            if (technology == null)
                return false;

            foreach (
                TechnologyDefinition prerequisite
                in technology.Prerequisites)
            {
                if (prerequisite == null)
                    continue;

                if (!technologyInventory.IsUnlocked(
                        prerequisite.TechnologyId))
                {
                    return false;
                }
            }

            return true;
        }

        private void CompleteResearch()
        {
            TechnologyDefinition completed =
                currentTechnology;

            technologyInventory.Unlock(
                completed.TechnologyId);

            currentTechnology = null;
            researchElapsed = 0f;
            researching = false;

            SaveResearchCheckpoint();

            ResearchCompleted?.Invoke(
                completed);

            Debug.Log(
                $"[RESEARCH] Completed: {completed.DisplayName}");
        }
    }
}







