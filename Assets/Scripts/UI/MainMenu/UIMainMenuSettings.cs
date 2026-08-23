using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuSettings : MonoBehaviour
{
    public static UIMainMenuSettings Instance { get; private set; }

    [SerializeField] private Button settingsButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (settingsButton == null)
            settingsButton = GetComponent<Button>();
    }

    public void Settings()
    {
        Debug.Log("Settings selected.");
    }

    public void SetInteractable(bool interactable)
    {
        if (settingsButton != null)
            settingsButton.interactable = interactable;
    }
}
