using System;
using UnityEngine;
using ObsidianProtocol.Game.Technology;

namespace ObsidianProtocol.Game.Research
{
    public sealed class ResearchManager : MonoBehaviour
    {
        public static ResearchManager Instance { get; private set; }

        [SerializeField] private TechnologyInventory technologyInventory =
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

            currentTechnology = technology;
            researchElapsed = 0f;
            researching = true;

            ResearchStarted?.Invoke(
                currentTechnology);

            Debug.Log(
                $"[RESEARCH] Started: {technology.DisplayName}");

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

            ResearchCompleted?.Invoke(
                completed);

            Debug.Log(
                $"[RESEARCH] Completed: {completed.DisplayName}");
        }
    }
}
