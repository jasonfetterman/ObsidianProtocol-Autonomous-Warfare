using UnityEngine;

public class UITooltipSystem : MonoBehaviour
{
    public static UITooltipSystem Instance { get; private set; }

    public string CurrentTooltip { get; private set; } = string.Empty;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ShowTooltip(string text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        CurrentTooltip = text;
    }

    public void HideTooltip()
    {
        CurrentTooltip = string.Empty;
    }

    public bool IsVisible()
    {
        return !string.IsNullOrEmpty(CurrentTooltip);
    }
}
