using UnityEngine;
using UnityEngine.UI;

public class HUDUnitStatusIndicators : MonoBehaviour
{
    [SerializeField] private Text statusText;

    public string CurrentStatus { get; private set; } = "NORMAL";

    private void Awake()
    {
        Refresh();
    }

    public void SetStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return;

        CurrentStatus = status.ToUpperInvariant();
        Refresh();
    }

    public void SetNormal()
    {
        SetStatus("NORMAL");
    }

    public void SetDamaged()
    {
        SetStatus("DAMAGED");
    }

    public void SetDisabled()
    {
        SetStatus("DISABLED");
    }

    public void SetRetreating()
    {
        SetStatus("RETREATING");
    }

    public void SetDestroyed()
    {
        SetStatus("DESTROYED");
    }

    private void Refresh()
    {
        if (statusText != null)
            statusText.text = CurrentStatus;
    }
}