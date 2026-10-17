using UnityEngine;
using UnityEngine.UI;

public class FinancePurchasesController : MonoBehaviour
{
    private GameObject panel;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Install()
    {
        GameObject hud = GameObject.Find("FINANCE HUD");
        if (hud == null) return;

        var c = hud.GetComponent<FinancePurchasesController>();
        if (c == null) c = hud.AddComponent<FinancePurchasesController>();

        c.Setup();
    }

    private void Setup()
    {
        Transform t = transform.Find("FINANCE CONTROL PANEL/BUTTON_PURCHASES");
        if (t == null)
        {
            Debug.LogWarning("[FINANCE] PURCHASES button not found.");
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
            "PURCHASES PANEL",
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

        Text(panel.transform, "PURCHASES",
            new Vector2(30, -25), new Vector2(940, 45), 28);

        Text(panel.transform, "UNIT ACQUISITION / FACILITY / EQUIPMENT",
            new Vector2(30, -70), new Vector2(940, 30), 14);

        Item("WARDEN", "750 CR", 110);
        Item("BEACON", "500 CR", 165);
        Item("BULLDOG", "1,250 CR", 220);
        Item("FORGE", "1,400 CR", 275);
        Item("SENTINEL", "1,000 CR", 330);
        Item("OVERSEER", "1,500 CR", 385);
        Item("ECHO", "7,500 CR", 440);

        Text(panel.transform,
            "CURRENT CREDITS",
            new Vector2(40, -520),
            new Vector2(300, 30), 14);

        Text(panel.transform,
            "84,250 CR",
            new Vector2(40, -550),
            new Vector2(300, 40), 24);

        Button(panel.transform, "CLOSE",
            new Vector2(800, -585),
            new Vector2(150, 50),
            Close);

        panel.SetActive(false);
    }

    private void Item(string name, string price, float y)
    {
        GameObject row = new GameObject(
            "PURCHASE_" + name,
            typeof(RectTransform),
            typeof(Image),
            typeof(Button));

        row.transform.SetParent(panel.transform, false);

        RectTransform rt = row.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
        rt.anchoredPosition = new Vector2(40, -y);
        rt.sizeDelta = new Vector2(920, 45);

        row.GetComponent<Image>().color =
            new Color32(20, 29, 39, 255);

        Text(row.transform, name,
            new Vector2(15, 0),
            new Vector2(500, 45), 15,
            TextAnchor.MiddleLeft);

        Text(row.transform, price,
            new Vector2(650, 0),
            new Vector2(230, 45), 15,
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
