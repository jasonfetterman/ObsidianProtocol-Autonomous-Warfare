using UnityEngine;

public class HUDUnitCommandPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    public bool IsOpen => panel != null && panel.activeSelf;

    public void Open()
    {
        if (panel != null)
            panel.SetActive(true);
    }

    public void Close()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public void Toggle()
    {
        if (panel != null)
            panel.SetActive(!panel.activeSelf);
    }
}
