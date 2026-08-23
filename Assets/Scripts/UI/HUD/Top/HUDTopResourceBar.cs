using UnityEngine;

public class HUDTopResourceBar : MonoBehaviour
{
    public static HUDTopResourceBar Instance { get; private set; }

    [SerializeField] private GameObject resourceBar;

    public bool IsVisible { get; private set; } = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Show()
    {
        IsVisible = true;

        if (resourceBar != null)
            resourceBar.SetActive(true);
    }

    public void Hide()
    {
        IsVisible = false;

        if (resourceBar != null)
            resourceBar.SetActive(false);
    }

    public void SetVisible(bool visible)
    {
        if (visible)
            Show();
        else
            Hide();
    }
}
