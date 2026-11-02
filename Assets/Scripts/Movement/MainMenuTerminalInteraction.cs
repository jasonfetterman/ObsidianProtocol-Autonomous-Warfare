using UnityEngine;

public class MainMenuTerminalInteraction : MonoBehaviour
{
    public Transform player;
    public GameObject menu;
    public float interactionDistance = 2.5f;

    private void Update()
    {
        if (player == null || menu == null) return;

        bool nearby = Vector3.Distance(player.position, transform.position)
                      <= interactionDistance;

        if (nearby && Input.GetKeyDown(KeyCode.E))
        {
            bool opening = !menu.activeSelf;
            menu.SetActive(opening);
            Cursor.lockState = opening
                ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = opening;
        }
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
