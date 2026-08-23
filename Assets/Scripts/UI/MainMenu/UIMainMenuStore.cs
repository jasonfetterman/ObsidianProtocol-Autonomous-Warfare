using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuStore : MonoBehaviour
{
    public static UIMainMenuStore Instance { get; private set; }

    [SerializeField] private Button storeButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (storeButton == null)
            storeButton = GetComponent<Button>();
    }

    public void Store()
    {
        Debug.Log("Store selected.");
    }

    public void SetInteractable(bool interactable)
    {
        if (storeButton != null)
            storeButton.interactable = interactable;
    }
}
