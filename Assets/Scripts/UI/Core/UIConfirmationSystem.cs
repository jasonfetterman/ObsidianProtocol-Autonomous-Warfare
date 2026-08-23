using System;
using UnityEngine;

public class UIConfirmationSystem : MonoBehaviour
{
    public static UIConfirmationSystem Instance { get; private set; }

    public bool IsWaitingForConfirmation { get; private set; }

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

    public void RequestConfirmation(Action onConfirm, Action onCancel = null)
    {
        confirmAction = onConfirm;
        cancelAction = onCancel;
        IsWaitingForConfirmation = true;
    }

    public void Confirm()
    {
        if (!IsWaitingForConfirmation)
            return;

        IsWaitingForConfirmation = false;

        Action action = confirmAction;
        ClearCallbacks();

        action?.Invoke();
    }

    public void Cancel()
    {
        if (!IsWaitingForConfirmation)
            return;

        IsWaitingForConfirmation = false;

        Action action = cancelAction;
        ClearCallbacks();

        action?.Invoke();
    }

    private void ClearCallbacks()
    {
        confirmAction = null;
        cancelAction = null;
    }
}
