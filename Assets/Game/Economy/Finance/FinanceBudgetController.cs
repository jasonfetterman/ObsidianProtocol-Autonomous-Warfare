using UnityEngine;
using UnityEngine.UI;

public class FinanceBudgetController : MonoBehaviour
{
    private GameObject budgetPanel;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Install()
    {
        GameObject hud = GameObject.Find("FINANCE HUD");
        if (hud == null) return;

        FinanceBudgetController controller =
            hud.GetComponent<FinanceBudgetController>();

        if (controller == null)
            controller = hud.AddComponent<FinanceBudgetController>();

        controller.Setup();
    }

    private void Setup()
    {
        Transform buttonTransform =
            transform.Find("FINANCE CONTROL PANEL/BUTTON_ADJUST_BUDGET");

        if (buttonTransform == null)
        {
            Debug.LogWarning("[FINANCE] ADJUST BUDGET button not found.");
            return;
        }

        Button button = buttonTransform.GetComponent<Button>();

        if (button == null) return;

        button.onClick.RemoveListener(OpenBudget);
        button.onClick.AddListener(OpenBudget);

        CreateBudgetPanel();
    }

    private void CreateBudgetPanel()
    {
        if (budgetPanel != null) return;

        budgetPanel = new GameObject(
            "BUDGET ALLOCATION PLATFORM",
            typeof(RectTransform),
            typeof(Image)
        );

        budgetPanel.transform.SetParent(transform, false);

        RectTransform rt = budgetPanel.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(850, 620);

        Image bg = budgetPanel.GetComponent<Image>();
        bg.color = new Color32(12, 18, 26, 250);

        CreateText(
            budgetPanel.transform,
            "BUDGET ALLOCATION",
            new Vector2(30, -25),
            new Vector2(790, 45),
            28,
            FontStyle.Bold
        );

        CreateText(
            budgetPanel.transform,
            "COMMAND ECONOMY DISTRIBUTION",
            new Vector2(30, -70),
            new Vector2(790, 30),
            14,
            FontStyle.Normal
        );

        CreateBudgetRow("OPERATIONS", "38%", 150);
        CreateBudgetRow("LOGISTICS", "24%", 215);
        CreateBudgetRow("RESEARCH", "18%", 280);
        CreateBudgetRow("FLEET MAINTENANCE", "12%", 345);
        CreateBudgetRow("EXPERIMENTAL", "8%", 410);

        CreateButton(
            budgetPanel.transform,
            "APPLY",
            new Vector2(510, -535),
            new Vector2(140, 50),
            ApplyBudget
        );

        CreateButton(
            budgetPanel.transform,
            "CLOSE",
            new Vector2(670, -535),
            new Vector2(140, 50),
            CloseBudget
        );

        budgetPanel.SetActive(false);
    }

    private void CreateBudgetRow(string label, string value, float y)
    {
        CreateText(
            budgetPanel.transform,
            label,
            new Vector2(40, -y),
            new Vector2(500, 40),
            17,
            FontStyle.Bold
        );

        CreateText(
            budgetPanel.transform,
            value,
            new Vector2(570, -y),
            new Vector2(180, 40),
            17,
            FontStyle.Bold,
            TextAnchor.MiddleRight
        );
    }

    private void CreateButton(
        Transform parent,
        string label,
        Vector2 position,
        Vector2 size,
        UnityEngine.Events.UnityAction action)
    {
        GameObject go = new GameObject(
            "BUTTON_" + label,
            typeof(RectTransform),
            typeof(Image),
            typeof(Button)
        );

        go.transform.SetParent(parent, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
        rt.anchoredPosition = position;
        rt.sizeDelta = size;

        Image image = go.GetComponent<Image>();
        image.color = new Color32(28, 40, 53, 255);

        Button button = go.GetComponent<Button>();
        button.onClick.AddListener(action);

        CreateText(
            go.transform,
            label,
            Vector2.zero,
            size,
            14,
            FontStyle.Bold,
            TextAnchor.MiddleCenter
        );
    }

    private void CreateText(
        Transform parent,
        string value,
        Vector2 position,
        Vector2 size,
        int fontSize,
        FontStyle style,
        TextAnchor alignment = TextAnchor.UpperLeft)
    {
        GameObject go = new GameObject(
            "TEXT_" + value.Replace(" ", "_"),
            typeof(RectTransform),
            typeof(Text)
        );

        go.transform.SetParent(parent, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
        rt.anchoredPosition = position;
        rt.sizeDelta = size;

        Text text = go.GetComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.text = value;
        text.fontSize = fontSize;
        text.fontStyle = style;
        text.alignment = alignment;
        text.color = Color.white;
        text.raycastTarget = false;
    }

    private void OpenBudget()
    {
        if (budgetPanel != null)
            budgetPanel.SetActive(true);
    }

    private void CloseBudget()
    {
        if (budgetPanel != null)
            budgetPanel.SetActive(false);
    }

    private void ApplyBudget()
    {
        Debug.Log("[FINANCE] Budget allocation applied.");
        CloseBudget();
    }
}
