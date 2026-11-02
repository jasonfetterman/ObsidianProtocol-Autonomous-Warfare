using UnityEngine;
using UnityEngine.UI;

public class FinanceInvestmentsController : MonoBehaviour
{
    private GameObject panel;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Install()
    {
        GameObject hud = GameObject.Find("FINANCE HUD");
        if (hud == null) return;

        var c = hud.GetComponent<FinanceInvestmentsController>();
        if (c == null) c = hud.AddComponent<FinanceInvestmentsController>();

        c.Setup();
    }

    private void Setup()
    {
        Transform t = transform.Find("FINANCE CONTROL PANEL/BUTTON_INVESTMENTS");
        if (t == null)
        {
            Debug.LogWarning("[FINANCE] INVESTMENTS button not found.");
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
            "INVESTMENTS PANEL",
            typeof(RectTransform),
            typeof(Image));

        panel.transform.SetParent(transform, false);

        RectTransform rt = panel.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(.5f, .5f);
        rt.anchorMax = new Vector2(.5f, .5f);
        rt.pivot = new Vector2(.5f, .5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(850, 620);

        panel.GetComponent<Image>().color =
            new Color32(12, 18, 26, 250);

        Text(panel.transform, "INVESTMENTS",
            new Vector2(30, -25), new Vector2(790, 45), 28);

        Text(panel.transform, "COMMAND CAPITAL ALLOCATION",
            new Vector2(30, -70), new Vector2(790, 30), 14);

        Row("INFRASTRUCTURE", "25,000 CR", 145);
        Row("RESEARCH FUND", "15,000 CR", 215);
        Row("FLEET EXPANSION", "20,000 CR", 285);
        Row("LOGISTICS NETWORK", "10,000 CR", 355);
        Row("RESERVE", "14,250 CR", 425);

        Button(panel.transform, "CLOSE",
            new Vector2(670, -535),
            new Vector2(140, 50),
            Close);

        panel.SetActive(false);
    }

    private void Row(string name, string value, float y)
    {
        Text(panel.transform, name,
            new Vector2(40, -y),
            new Vector2(500, 40), 17);

        Text(panel.transform, value,
            new Vector2(570, -y),
            new Vector2(180, 40), 17,
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
            typeof(UnityEngine.UI.Button));

        go.transform.SetParent(parent, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
        rt.anchoredPosition = position;
        rt.sizeDelta = size;

        go.GetComponent<Image>().color =
            new Color32(28, 40, 53, 255);

        go.GetComponent<UnityEngine.UI.Button>()
            .onClick.AddListener(action);

        Text(go.transform, label, Vector2.zero, size, 14,
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
            typeof(UnityEngine.UI.Text));

        go.transform.SetParent(parent, false);

        RectTransform rt = go.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(0, 1);
        rt.pivot = new Vector2(0, 1);
        rt.anchoredPosition = position;
        rt.sizeDelta = size;

        var text = go.GetComponent<UnityEngine.UI.Text>();
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
