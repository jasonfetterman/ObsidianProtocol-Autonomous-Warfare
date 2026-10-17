using UnityEngine;
using UnityEngine.UI;

public class FinanceController : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod]
    private static void Install()
    {
        GameObject hud = GameObject.Find("FINANCE HUD");
        if (hud == null) return;

        FinanceController controller = hud.GetComponent<FinanceController>();
        if (controller == null)
            controller = hud.AddComponent<FinanceController>();

        Transform button = hud.transform.Find("CONTROL ADJUST BUDGET");
        if (button == null) return;

        Button b = button.GetComponent<Button>();
        if (b == null) return;

        b.onClick.RemoveListener(controller.OpenBudget);
        b.onClick.AddListener(controller.OpenBudget);
    }

    public void OpenBudget()
    {
        GameObject panel = GameObject.Find("BUDGET ALLOCATION PLATFORM");
        if (panel != null) panel.SetActive(true);
    }

    public void CloseBudget()
    {
        GameObject panel = GameObject.Find("BUDGET ALLOCATION PLATFORM");
        if (panel != null) panel.SetActive(false);
    }
}
