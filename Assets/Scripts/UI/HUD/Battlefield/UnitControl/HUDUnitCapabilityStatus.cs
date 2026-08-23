using UnityEngine;
using UnityEngine.UI;

public class HUDUnitCapabilityStatus : MonoBehaviour
{
    [SerializeField] private Text capabilityText;

    public string CurrentCapabilityStatus { get; private set; } = "READY";

    private void Awake()
    {
        Refresh();
    }

    public void SetCapabilityStatus(string status)
    {
        if (string.IsNullOrWhiteSpace(status))
            return;

        CurrentCapabilityStatus = status.ToUpperInvariant();
        Refresh();
    }

    public void SetReady()
    {
        SetCapabilityStatus("READY");
    }

    public void SetUnavailable()
    {
        SetCapabilityStatus("UNAVAILABLE");
    }

    public void SetDisabled()
    {
        SetCapabilityStatus("DISABLED");
    }

    private void Refresh()
    {
        if (capabilityText != null)
            capabilityText.text = CurrentCapabilityStatus;
    }
}
