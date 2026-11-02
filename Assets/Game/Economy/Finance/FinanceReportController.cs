using UnityEngine;
using UnityEngine.UI;

public class FinanceReportController : MonoBehaviour
{
    private GameObject panel;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Install()
    {
        GameObject hud = GameObject.Find("FINANCE HUD");
        if (hud == null) return;

        var c = hud.GetComponent<FinanceReportController>();
        if (c == null) c = hud.AddComponent<FinanceReportController>();

        c.Setup();
    }

    private void Setup()
    {
        Transform t = transform.Find("FINANCE CONTROL PANEL/BUTTON_FINANCIAL_REPORT");
        if (t == null)
        {
            Debug.LogWarning("[FINANCE] FINANCIAL REPORT button not found.");
            return;
        }

        Button b = t.GetComponent<Button>();
        if (b == null) return;

        b.onClick.RemoveListener(Open);
        b.onClick.AddListener(Open);

        CreatePanel();
    }

    private void CreatePanel()
    {
        if (panel != null) return;

        panel = new GameObject(
            "FINANCIAL REPORT PANEL",
            typeof(RectTransform),
            typeof(Image));

        panel.transform.SetParent(transform, false);

        RectTransform rt = panel.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(.5f, .5f);
        rt.anchorMax = new Vector2(.5f, .5f);
        rt.pivot = new Vector2(.5f, .5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(1000, 680);

        panel.GetComponent<Image>().color =
            new Color32(12, 18, 26, 250);

        Text(panel.transform, "FINANCIAL REPORT",
            new Vector2(30, -25),
            new Vector2(940, 45), 28);

        Text(panel.transform, "COMMAND ECONOMY STATUS REPORT",
            new Vector2(30, -70),
            new Vector2(940, 30), 14);

        Report("CURRENT CREDITS", "84,250 CR", 125);
        Report("NET BALANCE", "+2,840 / HR", 190);
        Report("INCOME RATE", "+4,920 / HR", 255);
        Report("EXPENDITURE RATE", "-2,080 / HR", 320);
        Report("DEPLOYMENT BUDGET", "10,000 DP", 385);
        Report("POWER LIMIT", "ENFORCED", 450);

        Text(panel.transform,
            "FINANCIAL STATUS",
            new Vector2(520, -125),
            new Vector2(380, 30), 16);

        Text(panel.transform,
            "ECONOMY STABLE",
            new Vector2(520, -165),
            new Vector2(380, 40), 23);

        Text(panel.transform,
            "Reserve remains available for command operations.",
            new Vector2(520, -220),
            new Vector2(380, 70), 14);

        Button(panel.transform, "CLOSE",
            new Vector2(800, -585),
            new Vector2(150, 50),
            Close);

        panel.SetActive(false);
    }

    private void Report(string label, string value, float y)
    {
        Text(panel.transform, label,
            new Vector2(40, -y),
            new Vector2(300, 30), 14);

        Text(panel.transform, value,
            new Vector2(300, -y),
            new Vector2(180, 30), 16,
            TextAnchor.MiddleRight);
    }

    private void Button(
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
            typeof(Button));

        go.transform.SetParent(parent, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
        rt.anchoredPosition = position;
        rt.sizeDelta = size;

        go.GetComponent<Image>().color =
            new Color32(28, 40, 53, 255);

        go.GetComponent<Button>().onClick.AddListener(action);

        Text(go.transform, label,
            Vector2.zero, size, 14,
            TextAnchor.MiddleCenter);
    }

    private void Text(
        Transform parent,
        string value,
        Vector2 position,
        Vector2 size,
        int fontSize,
        TextAnchor alignment = TextAnchor.UpperLeft)
    {
        GameObject go = new GameObject(
            "TEXT_" + value.Replace(" ", "_"),
            typeof(RectTransform),
            typeof(Text));

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
        text.fontStyle = FontStyle.Bold;
        text.alignment = alignment;
        text.color = Color.white;
        text.raycastTarget = false;
    }

    private void Open()
    {
        if (panel != null)
            panel.SetActive(true);
    }

    private void Close()
    {
        if (panel != null)
            panel.SetActive(false);
    }
}
