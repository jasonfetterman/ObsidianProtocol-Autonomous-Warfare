using UnityEngine;

public class HUDSelectionRing : MonoBehaviour
{
    [SerializeField] private GameObject ringVisual;

    public bool IsVisible { get; private set; }

    public void Show()
    {
        IsVisible = true;

        if (ringVisual != null)
            ringVisual.SetActive(true);
    }

    public void Hide()
    {
        IsVisible = false;

        if (ringVisual != null)
            ringVisual.SetActive(false);
    }

    public void SetVisual(GameObject visual)
    {
        ringVisual = visual;
    }
}
