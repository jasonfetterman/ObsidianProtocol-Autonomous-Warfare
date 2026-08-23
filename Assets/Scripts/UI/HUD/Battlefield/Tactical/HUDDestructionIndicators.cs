using UnityEngine;
using UnityEngine.UI;

public class HUDDestructionIndicators : MonoBehaviour
{
    [SerializeField] private GameObject indicatorVisual;
    [SerializeField] private Text indicatorText;

    public GameObject DestroyedObject { get; private set; }
    public bool IsActive { get; private set; }

    public void ShowDestruction(GameObject target)
    {
        DestroyedObject = target;
        IsActive = true;

        if (indicatorText != null)
            indicatorText.text = "DESTROYED";

        if (indicatorVisual != null)
            indicatorVisual.SetActive(true);
    }

    public void Hide()
    {
        DestroyedObject = null;
        IsActive = false;

        if (indicatorVisual != null)
            indicatorVisual.SetActive(false);
    }
}
