using UnityEngine;
using UnityEngine.UI;

public class UIMainMenuExit : MonoBehaviour
{
    public static UIMainMenuExit Instance { get; private set; }

    [SerializeField] private Button exitButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (exitButton == null)
            exitButton = GetComponent<Button>();
    }

    public void Exit()
    {
        Debug.Log("Exit selected.");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SetInteractable(bool interactable)
    {
        if (exitButton != null)
            exitButton.interactable = interactable;
    }
}
