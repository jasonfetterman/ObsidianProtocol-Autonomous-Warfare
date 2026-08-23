using UnityEngine;
using UnityEngine.UI;

public class HUDDefenseMarkers : MonoBehaviour
{
    [SerializeField] private GameObject markerVisual;
    [SerializeField] private Text markerText;

    public GameObject Target { get; private set; }
    public bool IsActive { get; private set; }

    private void Awake()
    {
        Hide();
    }

    public void SetTarget(GameObject target)
    {
        Target = target;

        if (markerText != null)
            markerText.text = "DEFEND";

        if (Target != null)
            Show();
        else
            Hide();
    }

    public void Show()
    {
        IsActive = true;

        if (markerVisual != null)
            markerVisual.SetActive(true);
    }

    public void Hide()
    {
        IsActive = false;

        if (markerVisual != null)
            markerVisual.SetActive(false);
    }

    public void ClearTarget()
    {
        Target = null;
        Hide();
    }
}