using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinanceOperationsNavigation : MonoBehaviour
{
    void Start()
    {
        GameObject hud = GameObject.Find("FINANCE HUD");
        if (hud == null) return;

        Transform buttonTransform = hud.transform.Find("FINANCE BOTTOM BAR/BUTTON_OPERATIONS");
        if (buttonTransform == null) return;

        Button button = buttonTransform.GetComponent<Button>();
        if (button == null) return;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => SceneManager.LoadScene("Operations"));
    }
}
