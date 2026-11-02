using UnityEngine;
using TMPro;

public class UnitRowDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI unitNameText;
    [SerializeField] private TextMeshProUGUI unitStatusText;

    public void SetUnit(UnitStatsComponent stats)
    {
        if (stats == null)
        {
            return;
        }

        unitNameText.text = stats.gameObject.name;

        string status = stats.CurrentHealth <= 0f
            ? "Destroyed"
            : stats.CurrentHealth < stats.MaxHealth
                ? "Damaged"
                : "Ready";

        SetStatusTextAndColor(status);
    }

    public void SetSampleData(string name, string status)
    {
        unitNameText.text = name;
        SetStatusTextAndColor(status);
    }

    private void SetStatusTextAndColor(string status)
    {
        unitStatusText.text = status;

        switch (status)
        {
            case "Ready":
                unitStatusText.color = new Color(0.3f, 1f, 0.5f);
                break;
            case "Damaged":
                unitStatusText.color = new Color(1f, 0.65f, 0.2f);
                break;
            case "Destroyed":
                unitStatusText.color = new Color(1f, 0.3f, 0.3f);
                break;
            default:
                unitStatusText.color = Color.white;
                break;
        }
    }
}