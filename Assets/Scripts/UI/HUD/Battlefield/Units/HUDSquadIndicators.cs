using UnityEngine;
using UnityEngine.UI;

public class HUDSquadIndicators : MonoBehaviour
{
    [SerializeField] private Text squadText;

    public string SquadID { get; private set; } = "SQUAD";
    public int UnitCount { get; private set; }

    private void Awake()
    {
        Refresh();
    }

    public void SetSquad(string squadID, int unitCount)
    {
        if (!string.IsNullOrWhiteSpace(squadID))
            SquadID = squadID.ToUpperInvariant();

        UnitCount = Mathf.Max(0, unitCount);
        Refresh();
    }

    public void SetSquadID(string squadID)
    {
        if (string.IsNullOrWhiteSpace(squadID))
            return;

        SquadID = squadID.ToUpperInvariant();
        Refresh();
    }

    public void SetUnitCount(int unitCount)
    {
        UnitCount = Mathf.Max(0, unitCount);
        Refresh();
    }

    private void Refresh()
    {
        if (squadText != null)
            squadText.text = $"{SquadID}  {UnitCount}";
    }
}