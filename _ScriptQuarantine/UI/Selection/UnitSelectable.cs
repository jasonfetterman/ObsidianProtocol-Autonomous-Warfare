using UnityEngine;

public class UnitSelectable : MonoBehaviour
{
    public GameObject selectionIndicator;

    void Awake()
    {
        if (selectionIndicator != null)
            selectionIndicator.SetActive(false);
    }

    public void SetSelected(bool selected)
    {
        if (selectionIndicator != null)
            selectionIndicator.SetActive(selected);
    }
}
