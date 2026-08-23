using UnityEngine;
using UnityEngine.UI;

public class HUDThreatMarkers : MonoBehaviour
{
    [SerializeField] private GameObject markerVisual;
    [SerializeField] private Text markerText;

    public GameObject Target { get; private set; }
    public bool IsActive { get; private set; }

    public void SetTarget(GameObject target)
    {
        Target = target;

        if (markerText != null) markerText.text = "THREAT";

        if (Target != null) Show();
        else Hide();
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

    public void ClearTarget()
    {
        Target = null;
        Hide();
    }
}
