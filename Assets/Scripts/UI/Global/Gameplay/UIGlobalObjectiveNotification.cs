using System;
using UnityEngine;

public class UIGlobalObjectiveNotification : MonoBehaviour
{
    public static UIGlobalObjectiveNotification Instance { get; private set; }

    public string CurrentObjective { get; private set; } = string.Empty;
    public bool IsVisible { get; private set; }

    public event Action<string> OnObjectiveShown;
    public event Action OnObjectiveHidden;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Show(string objective)
    {
        if (string.IsNullOrWhiteSpace(objective))
            return;

        CurrentObjective = objective;
        IsVisible = true;

        OnObjectiveShown?.Invoke(objective);
    }

    public void Hide()
    {
        if (!IsVisible)
            return;

        CurrentObjective = string.Empty;
        IsVisible = false;

        OnObjectiveHidden?.Invoke();
    }
}
