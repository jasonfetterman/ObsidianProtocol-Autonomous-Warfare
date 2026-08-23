using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuConnectionStatus : MonoBehaviour
{
    public static UIMainMenuConnectionStatus Instance { get; private set; }

    [SerializeField] private Text statusText;

    public bool IsConnected { get; private set; }
    public string Status { get; private set; } = "OFFLINE";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Refresh();
    }

    public void SetConnected()
    {
        IsConnected = true;
        Status = "ONLINE";
        Refresh();
    }

    public void SetDisconnected()
    {
        IsConnected = false;
        Status = "OFFLINE";
        Refresh();
    }

    public void SetStatus(string status, bool connected)
    {
        if (string.IsNullOrWhiteSpace(status))
            return;

        Status = status;
        IsConnected = connected;
        Refresh();
    }

    private void Refresh()
    {
        if (statusText != null)
            statusText.text = Status;
    }
}
