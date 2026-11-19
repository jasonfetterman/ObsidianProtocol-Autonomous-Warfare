using UnityEngine;
using TMPro;
using ObsidianProtocol.Game.Garage;

public class UnitRowDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI unitNameText;
    [SerializeField] private TextMeshProUGUI unitStatusText;

    public void SetFleetData(
        string unitName,
        FleetUnitStatus status)
    {
        if (unitNameText != null)
            unitNameText.text = unitName;

        SetFleetStatus(status);
    }

    private void SetFleetStatus(FleetUnitStatus status)
    {
        if (unitStatusText == null)
            return;

        unitStatusText.text = status.ToString().ToUpperInvariant();

        switch (status)
        {
            case FleetUnitStatus.Operational:
                unitStatusText.color = new Color(0.3f, 1f, 0.5f);
                break;

            case FleetUnitStatus.Stored:
                unitStatusText.color = Color.white;
                break;

            case FleetUnitStatus.Repairing:
            case FleetUnitStatus.Upgrading:
            case FleetUnitStatus.Fabricating:
                unitStatusText.color = new Color(1f, 0.75f, 0.25f);
                break;

            case FleetUnitStatus.Deployed:
                unitStatusText.color = new Color(0.3f, 0.75f, 1f);
                break;

            case FleetUnitStatus.Salvaging:
            case FleetUnitStatus.Disabled:
                unitStatusText.color = new Color(1f, 0.35f, 0.35f);
                break;

            default:
                unitStatusText.color = Color.white;
                break;
        }
    }
}