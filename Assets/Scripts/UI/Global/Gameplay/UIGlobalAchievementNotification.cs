using System;
using UnityEngine;

public class UIGlobalAchievementNotification : MonoBehaviour
{
    public static UIGlobalAchievementNotification Instance { get; private set; }

    public string CurrentAchievement { get; private set; } = string.Empty;
    public bool IsVisible { get; private set; }

    public event Action<string> OnAchievementShown;
    public event Action OnAchievementHidden;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Show(string achievement)
    {
        if (string.IsNullOrWhiteSpace(achievement))
            return;

        CurrentAchievement = achievement;
        IsVisible = true;

        OnAchievementShown?.Invoke(achievement);
    }

    public void Hide()
    {
        if (!IsVisible)
            return;

        CurrentAchievement = string.Empty;
        IsVisible = false;

        OnAchievementHidden?.Invoke();
    }
}
