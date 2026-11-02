using UnityEngine;
using ObsidianProtocol.Game.Research;
using ObsidianProtocol.Game.Technology;

namespace ObsidianProtocol.UI.Research
{
    public sealed class ResearchSelectedController : MonoBehaviour
    {
        [SerializeField] private TechnologyDefinition selectedTechnology;

        public TechnologyDefinition SelectedTechnology => selectedTechnology;

        public void SetSelectedTechnology(TechnologyDefinition technology)
        {
            selectedTechnology = technology;

            ResearchManager manager = ResearchManager.Instance;
            if (manager != null && technology != null)
                manager.RegisterTechnology(technology);

            Debug.Log(
                technology == null
                    ? "[RESEARCH UI] Selection cleared."
                    : $"[RESEARCH UI] Selected: {technology.DisplayName}");
        }

        public void StartSelectedResearch()
        {
            if (selectedTechnology == null)
            {
                Debug.LogWarning("[RESEARCH UI] No technology selected.");
                return;
            }

            ResearchManager manager = ResearchManager.Instance;

            if (manager == null)
            {
                Debug.LogError("[RESEARCH UI] Research Manager not found.");
                return;
            }

            bool started = manager.StartResearch(selectedTechnology);

            Debug.Log(
                $"[RESEARCH UI] Start selected research: {started}");
        }
        public void ViewSelectedTechnologyDetails()
        {
            var textObject = GameObject.Find(
                "TEXT SELECT A TECHNOLOGY NODE TO VIEW REQUIREMENTS");

            var detailsText = textObject != null
                ? textObject.GetComponent<UnityEngine.UI.Text>()
                : null;

            if (detailsText == null)
            {
                Debug.LogError("[RESEARCH UI] Technology details text not found.");
                return;
            }

            if (selectedTechnology == null)
            {
                detailsText.text = "SELECT A TECHNOLOGY NODE TO VIEW REQUIREMENTS";
                Debug.LogWarning("[RESEARCH UI] No technology selected.");
                return;
            }

            var tech = selectedTechnology;
            var details = new System.Text.StringBuilder();

            details.AppendLine(tech.DisplayName);
            details.AppendLine(tech.Description);
            details.AppendLine("CATEGORY: " + tech.Category);
            details.AppendLine("RESEARCH COST: " + tech.ResearchCost.ToString("N0"));
            details.AppendLine("RESEARCH TIME: " + tech.ResearchTimeSeconds.ToString("N0") + " SECONDS");

            details.Append("PREREQUISITES: ");
            if (tech.Prerequisites == null || tech.Prerequisites.Count == 0)
                details.AppendLine("NONE");
            else
                details.AppendLine(string.Join(", ", System.Linq.Enumerable.Select(
                    tech.Prerequisites, prerequisite => prerequisite != null
                        ? prerequisite.DisplayName : "UNKNOWN")));

            details.Append("UNLOCKS: ");
            if (tech.UnlockIds == null || tech.UnlockIds.Count == 0)
                details.Append("NONE");
            else
                details.Append(string.Join(", ", tech.UnlockIds));

            detailsText.text = details.ToString();
            Debug.Log("[RESEARCH UI] Displayed details for " + tech.DisplayName);
        }
        public void CancelSelectedResearch()
        {
            ResearchManager manager = ResearchManager.Instance;

            if (manager == null)
            {
                Debug.LogError("[RESEARCH UI] Research Manager not found.");
                return;
            }

            if (!manager.CancelResearch())
                Debug.LogWarning("[RESEARCH UI] No active research to cancel.");
        }
    }
}




