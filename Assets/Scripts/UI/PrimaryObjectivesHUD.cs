
using System.Text;
using TMPro;
using UnityEngine;
using ObsidianProtocol.Game.Campaign;

public class PrimaryObjectivesHUD : MonoBehaviour
{
    private TMP_Text objectivesText;

    private void Awake()
    {
        objectivesText = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        CampaignObjectives.ObjectivesChanged += RefreshObjectives;
        RefreshObjectives();
    }

    private void OnDisable()
    {
        CampaignObjectives.ObjectivesChanged -= RefreshObjectives;
    }

    private void RefreshObjectives()
    {
        if (objectivesText == null)
            return;

        CampaignObjectives campaign =
            CampaignObjectives.ActiveInstance;

        if (campaign == null || !campaign.Initialized)
        {
            objectivesText.text = "NO ACTIVE OBJECTIVES";
            return;
        }

        StringBuilder display = new StringBuilder();

        foreach (CampaignObjective objective in campaign.GetObjectives())
        {
            if (display.Length > 0)
                display.AppendLine();

            display.Append(objective.Completed ? "[COMPLETE] " : "[ACTIVE] ");
            display.Append(objective.Description);
        }

        objectivesText.text = display.Length > 0
            ? display.ToString()
            : "NO ACTIVE OBJECTIVES";
    }
}
