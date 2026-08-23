using System;
using UnityEngine;

public class UIGlobalTooltip : MonoBehaviour
{
    public static UIGlobalTooltip Instance { get; private set; }

    public event Action<string> OnTooltipShown;
    public event Action OnTooltipHidden;

    public string CurrentTooltip { get; private set; } = string.Empty;
    public bool IsVisible => !string.IsNullOrEmpty(CurrentTooltip);

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

        CurrentTooltip = message;
        OnTooltipShown?.Invoke(message);
    }

    public void Hide()
    {
        if (!IsVisible)
            return;

        CurrentTooltip = string.Empty;
        OnTooltipHidden?.Invoke();
    }
}
