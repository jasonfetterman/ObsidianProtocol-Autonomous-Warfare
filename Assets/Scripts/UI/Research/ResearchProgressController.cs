using UnityEngine;
using TMPro;
using ObsidianProtocol.Game.Research;
using ObsidianProtocol.Game.Technology;

namespace ObsidianProtocol.UI.Research
{
    public sealed class ResearchProgressController : MonoBehaviour
    {
        [SerializeField] private RectTransform progressValue;
        [SerializeField] private TMP_Text technologyName;
        [SerializeField] private TMP_Text progressPercent;
        [SerializeField] private TMP_Text completionTime;

        private float fullWidth;
        private bool wasResearching;
        private bool researchCompleted;
        private ResearchManager subscribedManager;

        private void Awake()
        {
            if (progressValue != null)
                fullWidth = progressValue.rect.width;
        }

        private void OnEnable()
        {
            SubscribeToManager();
        }

        private void OnDisable()
        {
            UnsubscribeFromManager();
        }

        private void SubscribeToManager()
        {
            ResearchManager manager = ResearchManager.Instance;

            if (manager == null || manager == subscribedManager)
                return;

            UnsubscribeFromManager();

            subscribedManager = manager;
            subscribedManager.ResearchStarted += OnResearchStarted;
            subscribedManager.ResearchCompleted += OnResearchCompleted;
        }

        private void UnsubscribeFromManager()
        {
            if (subscribedManager == null)
                return;

            subscribedManager.ResearchStarted -= OnResearchStarted;
            subscribedManager.ResearchCompleted -= OnResearchCompleted;
            subscribedManager = null;
        }

        private void OnResearchStarted(TechnologyDefinition technology)
        {
            researchCompleted = false;
        }

        private void OnResearchCompleted(TechnologyDefinition technology)
        {
            researchCompleted = true;
        }

        private void Update()
        {
            SubscribeToManager();

            ResearchManager manager = ResearchManager.Instance;

            if (manager == null)
                return;

            if (manager.IsResearching && manager.CurrentTechnology != null)
            {
                wasResearching = true;
                researchCompleted = false;

                float progress = manager.ResearchProgress;

                if (progressValue != null)
                {
                    if (fullWidth <= 0f)
                        fullWidth = progressValue.rect.width;

                    progressValue.SetSizeWithCurrentAnchors(
                        RectTransform.Axis.Horizontal,
                        fullWidth * progress);
                }

                if (technologyName != null)
                    technologyName.text =
                        manager.CurrentTechnology.DisplayName;

                if (progressPercent != null)
                    progressPercent.text =
                        $"{Mathf.RoundToInt(progress * 100f)}%";

                if (completionTime != null)
                {
                    float remaining = Mathf.Max(
                        0f,
                        manager.CurrentTechnology.ResearchTimeSeconds -
                        manager.ResearchElapsed);

                    completionTime.text =
                        $"{Mathf.CeilToInt(remaining)}s";
                }
            }
            else if (wasResearching)
            {
                wasResearching = false;

                if (progressValue != null)
                {
                    if (fullWidth <= 0f)
                        fullWidth = progressValue.rect.width;

                    progressValue.SetSizeWithCurrentAnchors(
                        RectTransform.Axis.Horizontal,
                        researchCompleted ? fullWidth : 0f);
                }

                if (progressPercent != null)
                    progressPercent.text =
                        researchCompleted ? "100%" : "0%";

                if (completionTime != null)
                    completionTime.text =
                        researchCompleted ? "COMPLETE" : "CANCELLED";

                researchCompleted = false;
            }
        }
    }
}
