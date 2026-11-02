using UnityEngine;
using UnityEngine.UI;

public class FinanceTransactionsController : MonoBehaviour
{
    private GameObject panel;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Install()
    {
        GameObject hud = GameObject.Find("FINANCE HUD");
        if (hud == null) return;

        var c = hud.GetComponent<FinanceTransactionsController>();
        if (c == null) c = hud.AddComponent<FinanceTransactionsController>();

        c.Setup();
    }

    private void Setup()
    {
        Transform t = transform.Find("FINANCE CONTROL PANEL/BUTTON_TRANSACTIONS");
        if (t == null)
        {
            Debug.LogWarning("[FINANCE] TRANSACTIONS button not found.");
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
            "TRANSACTIONS PANEL",
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

        Text(panel.transform, "TRANSACTION HISTORY",
            new Vector2(30, -25),
            new Vector2(940, 45), 28);

        Text(panel.transform, "FINANCIAL ACTIVITY",
            new Vector2(30, -70),
            new Vector2(940, 30), 14);

        Header("DATE / TIME", 40, 115, 220);
        Header("TRANSACTION", 270, 115, 420);
        Header("AMOUNT", 720, 115, 180);

        Entry("TODAY 08:42", "Operations revenue", "+1,920 CR", 160);
        Entry("TODAY 08:31", "Fleet maintenance", "-1,080 CR", 210);
        Entry("TODAY 08:14", "Research allocation", "-760 CR", 260);
        Entry("TODAY 07:58", "Logistics revenue", "+1,140 CR", 310);
        Entry("TODAY 07:41", "Manufacturing", "-620 CR", 360);
        Entry("TODAY 07:20", "Infrastructure", "-380 CR", 410);

        Text(panel.transform, "CURRENT BALANCE",
            new Vector2(40, -500),
            new Vector2(300, 30), 14);

        Text(panel.transform, "84,250 CR",
            new Vector2(40, -535),
            new Vector2(400, 45), 25);

        Button(panel.transform, "CLOSE",
            new Vector2(800, -585),
            new Vector2(150, 50),
            Close);

        panel.SetActive(false);
    }

    private void Header(
        string value,
        float x,
        float y,
        float width)
    {
        Text(panel.transform, value,
            new Vector2(x, -y),
            new Vector2(width, 30), 13);
    }

    private void Entry(
        string time,
        string transaction,
        string amount,
        float y)
    {
        Text(panel.transform, time,
            new Vector2(40, -y),
            new Vector2(220, 35), 13);

        Text(panel.transform, transaction,
            new Vector2(270, -y),
            new Vector2(420, 35), 14);

        Text(panel.transform, amount,
            new Vector2(720, -y),
            new Vector2(180, 35), 14,
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
