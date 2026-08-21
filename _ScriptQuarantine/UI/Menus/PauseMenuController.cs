using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenuPanel;

    [SerializeField] private PauseMenuAnimation pauseMenuAnimation;

    private bool isOpen;
    private bool isAnimating;

    private void Awake()
    {
        isOpen = false;
        isAnimating = false;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    public void TogglePauseMenu()
    {
        if (isAnimating)
            return;

        if (isOpen)
            ClosePauseMenu();
        else
            OpenPauseMenu();
    }

    public void OpenPauseMenu()
    {
        if (isOpen || isAnimating)
            return;

        if (pauseMenuPanel == null)
        {
            Debug.LogError(
                "PauseMenuController: Pause Menu Panel is not assigned."
            );

            return;
        }

        if (pauseMenuAnimation == null)
        {
            Debug.LogError(
                "PauseMenuController: Pause Menu Animation is not assigned."
            );

            return;
        }

        isOpen = true;
        isAnimating = true;

        pauseMenuPanel.SetActive(true);

        pauseMenuAnimation.PlayOpenAnimation();

        Invoke(
            nameof(FinishAnimation),
            1.25f
        );
    }

    public void ClosePauseMenu()
    {
        if (!isOpen || isAnimating)
            return;

        if (pauseMenuAnimation == null)
        {
            pauseMenuPanel.SetActive(false);
            isOpen = false;
            return;
        }

        isAnimating = true;

        pauseMenuAnimation.PlayCloseAnimation();

        Invoke(
            nameof(FinishCloseAnimation),
            1.25f
        );
    }

    private void FinishAnimation()
    {
        isAnimating = false;
    }

    private void FinishCloseAnimation()
    {
        isOpen = false;
        isAnimating = false;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    public void ResumeGame()
    {
        ClosePauseMenu();
    }

    public bool IsPauseMenuOpen()
    {
        return isOpen;
    }
}