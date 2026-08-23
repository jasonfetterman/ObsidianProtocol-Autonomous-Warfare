using UnityEngine;
using UnityEngine.UI;

public class HUDBuildingStructureIndicators : MonoBehaviour
{
    [SerializeField] private GameObject indicatorVisual;
    [SerializeField] private Text indicatorText;

    public GameObject Structure { get; private set; }
    public string StructureStatus { get; private set; }
    public bool IsActive { get; private set; }

    public void SetStructure(GameObject structure, string status)
    {
        Structure = structure;
        StructureStatus = string.IsNullOrWhiteSpace(status)
            ? "STRUCTURE"
            : status.ToUpperInvariant();

        if (indicatorText != null)
            indicatorText.text = StructureStatus;

        IsActive = Structure != null;

        if (indicatorVisual != null)
            indicatorVisual.SetActive(IsActive);
    }

    public void Clear()
    {
        Structure = null;
        StructureStatus = null;
        IsActive = false;

        if (indicatorVisual != null)
            indicatorVisual.SetActive(false);
    }
}
