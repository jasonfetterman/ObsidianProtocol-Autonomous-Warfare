using UnityEngine;

public class UIMainMenuBackground : MonoBehaviour
{
    public static UIMainMenuBackground Instance { get; private set; }

    [SerializeField] private GameObject backgroundObject;

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
        if (backgroundObject != null)
            backgroundObject.SetActive(true);
    }

    public void Hide()
    {
        if (backgroundObject != null)
            backgroundObject.SetActive(false);
    }
}
