using UnityEngine;
using UnityEngine.UI;

public class HUDWaypointMarkers : MonoBehaviour
{
    [SerializeField] private GameObject markerVisual;
    [SerializeField] private Text waypointText;

    public string WaypointID { get; private set; }
    public Vector3 WorldPosition { get; private set; }
    public bool IsActive { get; private set; }

    private void Awake()
    {
        Hide();
    }

    public void SetWaypoint(string id, Vector3 position)
    {
        WaypointID = id;
        WorldPosition = position;

        if (waypointText != null)
            waypointText.text = "WAYPOINT";

        Show();
    }

    public void SetPosition(Vector3 position)
    {
        WorldPosition = position;
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

    public void ClearWaypoint()
    {
        WaypointID = null;
        Hide();
    }
}