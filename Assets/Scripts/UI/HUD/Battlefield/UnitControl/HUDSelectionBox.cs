using UnityEngine;

public class HUDSelectionBox : MonoBehaviour
{
    [SerializeField] private RectTransform selectionBox;

    public bool IsSelecting { get; private set; }

    private void Awake()
    {
        Hide();
    }

    public void BeginSelection()
    {
        IsSelecting = true;

        if (selectionBox != null)
            selectionBox.gameObject.SetActive(true);
    }

    public void EndSelection()
    {
        IsSelecting = false;
        Hide();
    }

    public void SetBox(RectTransform box)
    {
        selectionBox = box;
    }

    public void Show()
    {
        if (selectionBox != null)
            selectionBox.gameObject.SetActive(true);
    }

    public void Hide()
    {
        if (selectionBox != null)
            selectionBox.gameObject.SetActive(false);
    }
}
