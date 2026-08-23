using System;
using UnityEngine;

public class UIGlobalPurchaseNotification : MonoBehaviour
{
    public static UIGlobalPurchaseNotification Instance { get; private set; }

    public string CurrentPurchase { get; private set; } = string.Empty;
    public bool IsVisible { get; private set; }

    public event Action<string> OnPurchaseShown;
    public event Action OnPurchaseHidden;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Show(string purchase)
    {
        if (string.IsNullOrWhiteSpace(purchase))
            return;

        CurrentPurchase = purchase;
        IsVisible = true;

        OnPurchaseShown?.Invoke(purchase);
    }

    public void Hide()
    {
        if (!IsVisible)
            return;

        CurrentPurchase = string.Empty;
        IsVisible = false;

        OnPurchaseHidden?.Invoke();
    }
}
