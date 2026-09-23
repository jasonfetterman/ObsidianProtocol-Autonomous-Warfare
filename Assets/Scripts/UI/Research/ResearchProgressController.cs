using UnityEngine;
using TMPro;
using ObsidianProtocol.Game.Research;

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

        private void Awake()
        {
            if (progressValue != null)
                fullWidth = progressValue.rect.width;
        }

        private void Update()
        {
            ResearchManager manager = ResearchManager.Instance;

            if (manager == null)
                return;

            if (manager.IsResearching && manager.CurrentTechnology != null)
            {
                wasResearching = true;

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
                    float remaining =
                        Mathf.Max(
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
                        fullWidth);
                }

                if (progressPercent != null)
                    progressPercent.text = "100%";

                if (completionTime != null)
                    completionTime.text = "COMPLETE";
            }
        }
    }
}
