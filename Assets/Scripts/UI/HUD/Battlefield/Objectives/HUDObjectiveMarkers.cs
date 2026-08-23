using UnityEngine;
using UnityEngine.UI;

public class HUDObjectiveMarkers : MonoBehaviour
{
    [SerializeField] private GameObject markerVisual;
    [SerializeField] private Text objectiveText;

    public string ObjectiveID { get; private set; }
    public string ObjectiveName { get; private set; }
    public bool IsActive { get; private set; }

    private void Awake()
    {
        Hide();
    }

    public void SetObjective(string id, string objectiveName)
    {
        ObjectiveID = id;
        ObjectiveName = objectiveName;

        if (objectiveText != null)
            objectiveText.text = objectiveName;

        Show();
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

    public void ClearObjective()
    {
        ObjectiveID = null;
        ObjectiveName = null;
        Hide();
    }
}