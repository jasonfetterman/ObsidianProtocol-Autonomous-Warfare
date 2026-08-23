using UnityEngine;
using System;

public class UIGlobalNotificationSystem : MonoBehaviour
{
    public static UIGlobalNotificationSystem Instance { get; private set; }

    public event Action<string> OnNotification;

    public string CurrentMessage { get; private set; } = string.Empty;

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
        if (string.IsNullOrEmpty(message))
            return;

        CurrentMessage = message;
        OnNotification?.Invoke(message);
    }

    public void Clear()
    {
        CurrentMessage = string.Empty;
    }
}
