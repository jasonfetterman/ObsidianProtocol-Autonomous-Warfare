using System;
using UnityEngine;

public class UIGlobalAlertSystem : MonoBehaviour
{
    public static UIGlobalAlertSystem Instance { get; private set; }

    public event Action<string> OnAlert;

    public string CurrentAlert { get; private set; } = string.Empty;
    public bool HasAlert => !string.IsNullOrEmpty(CurrentAlert);

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

        CurrentAlert = message;
        OnAlert?.Invoke(message);
    }

    public void Clear()
    {
        CurrentAlert = string.Empty;
    }
}
