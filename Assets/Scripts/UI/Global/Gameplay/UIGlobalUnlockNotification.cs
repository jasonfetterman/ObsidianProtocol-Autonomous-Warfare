using System;
using UnityEngine;

public class UIGlobalUnlockNotification : MonoBehaviour
{
    public static UIGlobalUnlockNotification Instance { get; private set; }

    public string CurrentUnlock { get; private set; } = string.Empty;
    public bool IsVisible { get; private set; }

    public event Action<string> OnUnlockShown;
    public event Action OnUnlockHidden;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Show(string unlock)
    {
        if (string.IsNullOrWhiteSpace(unlock))
            return;

        CurrentUnlock = unlock;
        IsVisible = true;

        OnUnlockShown?.Invoke(unlock);
    }

    public void Hide()
    {
        if (!IsVisible)
            return;

        CurrentUnlock = string.Empty;
        IsVisible = false;

        OnUnlockHidden?.Invoke();
    }
}
