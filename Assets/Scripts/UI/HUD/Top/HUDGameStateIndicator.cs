using UnityEngine;
using UnityEngine.UI;

public class HUDGameStateIndicator : MonoBehaviour
{
    [SerializeField] private Text stateText;

    public string CurrentState { get; private set; } = "READY";

    private void Awake()
    {
        Refresh();
    }

    public void SetState(string state)
    {
        if (string.IsNullOrWhiteSpace(state))
            return;

        CurrentState = state.ToUpperInvariant();
        Refresh();
    }

    public void SetReady()
    {
        SetState("READY");
    }

    public void SetActive()
    {
        SetState("ACTIVE");
    }

    public void SetPaused()
    {
        SetState("PAUSED");
    }

    public void SetComplete()
    {
        SetState("COMPLETE");
    }

    private void Refresh()
    {
        if (stateText != null)
            stateText.text = CurrentState;
    }
}