using System;
using UnityEngine;

public class UIGlobalSystemMessage : MonoBehaviour
{
    public static UIGlobalSystemMessage Instance { get; private set; }

    public string CurrentMessage { get; private set; } = string.Empty;
    public bool IsVisible { get; private set; }

    public event Action<string> OnMessageShown;
    public event Action OnMessageHidden;

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
        IsVisible = true;

        OnMessageShown?.Invoke(message);
    }

    public void Hide()
    {
        if (!IsVisible)
            return;

        CurrentMessage = string.Empty;
        IsVisible = false;

        OnMessageHidden?.Invoke();
    }
}
