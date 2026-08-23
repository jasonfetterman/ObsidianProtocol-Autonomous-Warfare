using UnityEngine;
using UnityEngine.UI;

public class HUDTacticalIntentMarkers : MonoBehaviour
{
    [SerializeField] private GameObject markerVisual;
    [SerializeField] private Text markerText;

    public string Intent { get; private set; }
    public bool IsActive { get; private set; }

    public void SetIntent(string intent)
    {
        if (string.IsNullOrWhiteSpace(intent)) return;

        Intent = intent.ToUpperInvariant();
        if (markerText != null) markerText.text = Intent;
        Show();
    }

    public void Show()
    {
        IsActive = true;
        if (markerVisual != null) markerVisual.SetActive(true);
    }

    public void Hide()
    {
        IsActive = false;
        if (markerVisual != null) markerVisual.SetActive(false);
    }

    public void ClearIntent()
    {
        Intent = null;
        Hide();
    }
}
