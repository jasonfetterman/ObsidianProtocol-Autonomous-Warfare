using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuContinue : MonoBehaviour
{
    public static UIMainMenuContinue Instance { get; private set; }

    [SerializeField] private Button continueButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (continueButton == null)
            continueButton = GetComponent<Button>();
    }

    public void SetInteractable(bool interactable)
    {
        if (continueButton != null)
            continueButton.interactable = interactable;
    }

    public void ContinueGame()
    {
        Debug.Log("Continue selected.");
    }
}
