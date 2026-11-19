using TMPro;
using UnityEngine;
using ObsidianProtocol.Game.Economy;

namespace ObsidianProtocol.UI.Research
{
    public sealed class ResearchFundingController : MonoBehaviour
    {
        private TMP_Text[] fundingDisplays;
        private int lastDisplayedCredits = -1;

        private void Awake()
        {
            RefreshFundingDisplays();
        }

        private void Update()
        {
            if (!EconomyRuntime.CampaignActive)
                return;

            int credits = EconomyRuntime.Credits;

            if (credits == lastDisplayedCredits)
                return;

            lastDisplayedCredits = credits;
            UpdateFundingDisplays(credits);
        }

        private void RefreshFundingDisplays()
        {
            fundingDisplays = FindObjectsByType<TMP_Text>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

            lastDisplayedCredits = -1;
        }

        private void UpdateFundingDisplays(int amount)
        {
            if (fundingDisplays == null)
                RefreshFundingDisplays();

            foreach (TMP_Text text in fundingDisplays)
            {
                if (text == null)
                    continue;

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