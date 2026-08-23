using System;
using UnityEngine;

public class UIGlobalProgressIndicator : MonoBehaviour
{
    public static UIGlobalProgressIndicator Instance { get; private set; }

    public float Progress { get; private set; }
    public bool IsActive { get; private set; }

    public event Action<float> OnProgressChanged;
    public event Action OnProgressStarted;
    public event Action OnProgressCompleted;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartProgress()
    {
        Progress = 0f;
        IsActive = true;

        OnProgressStarted?.Invoke();
        OnProgressChanged?.Invoke(Progress);
    }

    public void SetProgress(float value)
    {
        if (!IsActive)
            return;

        Progress = Mathf.Clamp01(value);
        OnProgressChanged?.Invoke(Progress);

        if (Progress >= 1f)
            CompleteProgress();
    }

    public void CompleteProgress()
    {
        if (!IsActive)
            return;

        Progress = 1f;
        IsActive = false;

        OnProgressChanged?.Invoke(Progress);
        OnProgressCompleted?.Invoke();
    }

    public void CancelProgress()
    {
        Progress = 0f;
        IsActive = false;
        OnProgressChanged?.Invoke(Progress);
    }
}
