using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuMultiplayer : MonoBehaviour
{
    public static UIMainMenuMultiplayer Instance { get; private set; }

    [SerializeField] private Button multiplayerButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (multiplayerButton == null)
            multiplayerButton = GetComponent<Button>();
    }

    public void Multiplayer()
    {
        Debug.Log("Multiplayer selected.");
    }

    public void SetInteractable(bool interactable)
    {
        if (multiplayerButton != null)
            multiplayerButton.interactable = interactable;
    }
}
