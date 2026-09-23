using UnityEngine;
using TMPro;
using ObsidianProtocol.Game.Economy;
using ObsidianProtocol.Game.Research;

namespace ObsidianProtocol.UI.Research
{
    public sealed class ResearchFundingController : MonoBehaviour
    {
        private int activeResearchCost;
        private bool wasResearching;

        private void Update()
        {
            FundingManager funding = FundingManager.Instance;
            ResearchManager research = ResearchManager.Instance;

            if (funding == null || research == null)
                return;

            if (research.IsResearching &&
                research.CurrentTechnology != null)
            {
                if (!wasResearching)
                {
                    wasResearching = true;
                    activeResearchCost =
                        research.CurrentTechnology.ResearchCost;
                }
            }
            else if (wasResearching)
            {
                wasResearching = false;

                if (funding.TrySpend(activeResearchCost))
                {
                    Debug.Log(
                        $"[RESEARCH FUNDING] Spent {activeResearchCost:N0}. " +
                        $"Remaining: {funding.CurrentFunding:N0}");
                }
                else
                {
                    Debug.LogWarning(
                        "[RESEARCH FUNDING] Not enough funding.");
                }
            }

            UpdateAllFundingDisplays(funding.CurrentFunding);
        }

        private void UpdateAllFundingDisplays(int amount)
        {
            TMP_Text[] allTexts =
                FindObjectsByType<TMP_Text>(
                    FindObjectsInactive.Include,
                    FindObjectsSortMode.None);

            foreach (TMP_Text text in allTexts)
            {
                string objectName =
                    text.gameObject.name.ToUpperInvariant();

                string currentText =
                    text.text.ToUpperInvariant();

                if (objectName.Contains("FUNDING") ||
                    currentText.Contains("FUNDING"))
                {
                    text.text = $"FUNDING {amount:N0}";
                }
            }
        }
    }
}
