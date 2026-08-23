using UnityEngine;

public class HUDFriendlyIndicators : MonoBehaviour
{
    [SerializeField] private GameObject indicatorVisual;

    public GameObject Target { get; private set; }
    public bool IsVisible { get; private set; }

    private void Awake()
    {
        Hide();
    }

    public void SetTarget(GameObject target)
    {
        Target = target;

        if (Target != null)
            Show();
        else
            Hide();
    }

    public void Show()
    {
        IsVisible = true;

        if (indicatorVisual != null)
            indicatorVisual.SetActive(true);
    }

    public void Hide()
    {
        IsVisible = false;

        if (indicatorVisual != null)
            indicatorVisual.SetActive(false);
    }

    public void ClearTarget()
    {
        Target = null;
        Hide();
    }
}