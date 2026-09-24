using System;
using UnityEngine;

public class UIStateSystem : MonoBehaviour
{
    public static UIStateSystem Instance { get; private set; }

    public string CurrentState { get; private set; } = "None";

    public event Action<string> OnStateChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetState(string newState)
    {
        if (string.IsNullOrEmpty(newState))
            return;

        if (CurrentState == newState)
            return;

        CurrentState = newState;
        OnStateChanged?.Invoke(CurrentState);
    }

    public bool IsState(string state)
    {
        return CurrentState == state;
    }
}
