using UnityEngine;

public class HUDSelectionIndicator : MonoBehaviour
{
    [SerializeField] private GameObject selectionVisual;

    public GameObject SelectedObject { get; private set; }

    public bool HasSelection => SelectedObject != null;

    private void Awake()
    {
        Hide();
    }

    public void Select(GameObject target)
    {
        SelectedObject = target;

        if (selectionVisual != null)
            selectionVisual.SetActive(SelectedObject != null);
    }

    public void ClearSelection()
    {
        SelectedObject = null;
        Hide();
    }

    public void Show()
    {
        if (selectionVisual != null)
            selectionVisual.SetActive(true);
    }

    public void Hide()
    {
        if (selectionVisual != null)
            selectionVisual.SetActive(false);
    }
}