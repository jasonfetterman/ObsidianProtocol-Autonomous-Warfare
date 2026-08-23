using UnityEngine;

public class UIMainMenuLogo : MonoBehaviour
{
    public static UIMainMenuLogo Instance { get; private set; }

    [SerializeField] private GameObject logoObject;

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
        if (logoObject != null)
            logoObject.SetActive(true);
    }

    public void Hide()
    {
        if (logoObject != null)
            logoObject.SetActive(false);
    }
}
