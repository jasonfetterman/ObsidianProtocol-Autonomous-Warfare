using System;
using UnityEngine;

public class UIGlobalConfirmationPopup : MonoBehaviour
{
    public static UIGlobalConfirmationPopup Instance { get; private set; }

    public bool IsOpen { get; private set; }
    public string CurrentMessage { get; private set; } = string.Empty;

    private Action confirmAction;
    private Action cancelAction;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Show(
        string message,
        Action onConfirm = null,
        Action onCancel = null)
    {
        if (string.IsNullOrWhiteSpace(message))
            return;

        CurrentMessage = message;
        confirmAction = onConfirm;
        cancelAction = onCancel;
        IsOpen = true;
    }

    public void Confirm()
    {
        if (!IsOpen)
            return;

        Action action = confirmAction;
        Close();

        action?.Invoke();
    }

    public void Cancel()
    {
        if (!IsOpen)
            return;

        Action action = cancelAction;
        Close();

        action?.Invoke();
    }

    public void Close()
    {
        IsOpen = false;
        CurrentMessage = string.Empty;
        confirmAction = null;
        cancelAction = null;
    }
}
