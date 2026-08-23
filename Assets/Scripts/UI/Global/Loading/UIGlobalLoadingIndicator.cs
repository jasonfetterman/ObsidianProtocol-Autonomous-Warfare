using System;
using UnityEngine;

public class UIGlobalLoadingIndicator : MonoBehaviour
{
    public static UIGlobalLoadingIndicator Instance { get; private set; }

    public bool IsLoading { get; private set; }
    public float Progress { get; private set; }

    public event Action OnLoadingStarted;
    public event Action<float> OnProgressChanged;
    public event Action OnLoadingCompleted;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void StartLoading()
    {
        IsLoading = true;
        Progress = 0f;

        OnLoadingStarted?.Invoke();
        OnProgressChanged?.Invoke(Progress);
    }

    public void SetProgress(float value)
    {
        if (!IsLoading)
            return;

        Progress = Mathf.Clamp01(value);
        OnProgressChanged?.Invoke(Progress);

        if (Progress >= 1f)
            CompleteLoading();
    }

    public void CompleteLoading()
    {
        if (!IsLoading)
            return;

        Progress = 1f;
        IsLoading = false;

        OnProgressChanged?.Invoke(Progress);
        OnLoadingCompleted?.Invoke();
    }
}
