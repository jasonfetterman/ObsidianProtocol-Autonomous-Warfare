using UnityEngine;
using UnityEngine.UI;

public class HUDConnectionIndicator : MonoBehaviour
{
    [SerializeField] private Text connectionText;

    public bool IsConnected { get; private set; }
    public string CurrentStatus { get; private set; } = "OFFLINE";

    private void Awake()
    {
        Refresh();
    }

    public void SetConnected()
    {
        IsConnected = true;
        CurrentStatus = "ONLINE";
        Refresh();
    }

    public void SetDisconnected()
    {
        IsConnected = false;
        CurrentStatus = "OFFLINE";
        Refresh();
    }

    public void SetStatus(string status, bool connected)
    {
        if (string.IsNullOrWhiteSpace(status))
            return;

        CurrentStatus = status.ToUpperInvariant();
        IsConnected = connected;
        Refresh();
    }

    private void Refresh()
    {
        if (connectionText != null)
            connectionText.text = CurrentStatus;
    }
}