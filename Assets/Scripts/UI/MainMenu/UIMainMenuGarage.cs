using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuGarage : MonoBehaviour
{
    public static UIMainMenuGarage Instance { get; private set; }

    [SerializeField] private Button garageButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (garageButton == null)
            garageButton = GetComponent<Button>();
    }

    public void Garage()
    {
        Debug.Log("Garage selected.");
    }

    public void SetInteractable(bool interactable)
    {
        if (garageButton != null)
            garageButton.interactable = interactable;
    }
}
