using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinanceCommandCenterNavigation : MonoBehaviour
{
    private const string ScenePath =
        "Assets/Scenes/SCN-02  COMMAND CENTER/[HUD] COMMAND CENTER HUD/Command_Center.unity";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Install()
    {
        GameObject hud = GameObject.Find("FINANCE HUD");
        if (hud == null) return;

        var controller = hud.GetComponent<FinanceCommandCenterNavigation>();
        if (controller == null)
            controller = hud.AddComponent<FinanceCommandCenterNavigation>();

        controller.Setup();
    }

    private void Setup()
    {
        Transform t = transform.Find(
            "FINANCE BOTTOM BAR/BUTTON_COMMAND_CENTER");

        if (t == null)
        {
            Debug.LogWarning("[FINANCE] COMMAND CENTER button not found.");
            return;
        }

        Button button = t.GetComponent<Button>();
        if (button == null)
        {
            Debug.LogWarning("[FINANCE] COMMAND CENTER Button component not found.");
            return;
        }

        button.onClick.RemoveListener(OpenCommandCenter);
        button.onClick.AddListener(OpenCommandCenter);

        Debug.Log("[FINANCE] COMMAND CENTER navigation connected.");
    }

    private void OpenCommandCenter()
    {
        Debug.Log("[FINANCE] Loading COMMAND CENTER.");

        if (!Application.isPlaying)
            return;

        SceneManager.LoadScene("Command_Center");
    }
}

