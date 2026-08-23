using System;
using UnityEngine;

public class UIGlobalErrorPopup : MonoBehaviour
{
    public static UIGlobalErrorPopup Instance { get; private set; }

    public bool IsOpen { get; private set; }
    public string CurrentMessage { get; private set; } = string.Empty;

    public event Action<string> OnErrorShown;
    public event Action OnErrorClosed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Show(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return;

        CurrentMessage = message;
        IsOpen = true;

        OnErrorShown?.Invoke(message);
    }

    public void Close()
    {
        if (!IsOpen)
            return;

        IsOpen = false;
        CurrentMessage = string.Empty;

        OnErrorClosed?.Invoke();
    }
}
