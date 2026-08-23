using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuCredits : MonoBehaviour
{
    public static UIMainMenuCredits Instance { get; private set; }

    [SerializeField] private Button creditsButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (creditsButton == null)
            creditsButton = GetComponent<Button>();
    }

    public void Credits()
    {
        Debug.Log("Credits selected.");
    }

    public void SetInteractable(bool interactable)
    {
        if (creditsButton != null)
            creditsButton.interactable = interactable;
    }
}
