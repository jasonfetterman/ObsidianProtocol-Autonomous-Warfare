using TMPro;
using UnityEngine;
using ObsidianProtocol.Game.Economy;

namespace ObsidianProtocol.UI.Research
{
    public sealed class ResearchFundingController : MonoBehaviour
    {
        private void Update()
        {
            FundingManager funding = FundingManager.Instance;

            if (funding == null)
                return;

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
