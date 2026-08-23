using System;
using UnityEngine;

public class UIGlobalResourceNotification : MonoBehaviour
{
    public static UIGlobalResourceNotification Instance { get; private set; }

    public string ResourceName { get; private set; } = string.Empty;
    public int Amount { get; private set; }
    public bool IsVisible { get; private set; }

    public event Action<string, int> OnResourceShown;
    public event Action OnResourceHidden;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Show(string resourceName, int amount)
    {
        if (string.IsNullOrWhiteSpace(resourceName))
            return;

        ResourceName = resourceName;
        Amount = amount;
        IsVisible = true;

        OnResourceShown?.Invoke(ResourceName, Amount);
    }

    public void Hide()
    {
        if (!IsVisible)
            return;

        ResourceName = string.Empty;
        Amount = 0;
        IsVisible = false;

        OnResourceHidden?.Invoke();
    }
}
