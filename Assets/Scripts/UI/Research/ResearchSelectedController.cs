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
    }
}
