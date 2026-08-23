using UnityEngine;
using UnityEngine.UI;

public class HUDReconMarkers : MonoBehaviour
{
    [SerializeField] private GameObject markerVisual;
    [SerializeField] private Text markerText;

    public Vector3 WorldPosition { get; private set; }
    public bool IsActive { get; private set; }

    public void SetPosition(Vector3 position)
    {
        WorldPosition = position;
        if (markerText != null) markerText.text = "RECON";
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
}
