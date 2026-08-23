using System;
using UnityEngine;

public class UIGlobalConnectionIndicator : MonoBehaviour
{
    public static UIGlobalConnectionIndicator Instance { get; private set; }

    public bool IsConnected { get; private set; }
    public string ConnectionStatus { get; private set; } = "Disconnected";

    public event Action<bool> OnConnectionChanged;
    public event Action<string> OnStatusChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetConnected()
    {
        SetConnectionState(true, "Connected");
    }

    public void SetDisconnected()
    {
        SetConnectionState(false, "Disconnected");
    }

    public void SetConnecting()
    {
        IsConnected = false;
        ConnectionStatus = "Connecting";

        OnStatusChanged?.Invoke(ConnectionStatus);
    }

    public void SetConnectionState(bool connected, string status)
    {
        IsConnected = connected;
        ConnectionStatus = string.IsNullOrWhiteSpace(status)
            ? (connected ? "Connected" : "Disconnected")
            : status;

        OnConnectionChanged?.Invoke(IsConnected);
        OnStatusChanged?.Invoke(ConnectionStatus);
    }
}
