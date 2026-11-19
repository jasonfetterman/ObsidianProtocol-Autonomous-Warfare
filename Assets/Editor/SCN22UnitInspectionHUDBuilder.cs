using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public static class SCN22UnitInspectionHUDBuilder
{
    private const string ScenePath = "Assets/Scenes/SCN22_UNIT_INSPECTION/SCN22_UNIT_INSPECTION.unity";

    [MenuItem("Obsidian Protocol/Unit Inspection/Rebuild HUD")]
    public static void Rebuild()
    {
        if (!File.Exists(ScenePath))
        {
            Debug.LogError("SCN22 scene not found: " + ScenePath);
            return;
        }

        string backup = ScenePath + ".pre_HUD_rebuild_" +
                        System.DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak";
        File.Copy(ScenePath, backup, true);

        Scene scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        GameObject canvasObject = GameObject.Find("[HUD] UNIT INSPECTION HUD");

        if (canvasObject == null || canvasObject.GetComponent<Canvas>() == null)
        {
            Debug.LogError("Existing Unit Inspection Canvas was not found. Scene was not modified.");
            return;
        }

        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.transform.localScale = Vector3.one;
        canvasObject.transform.localPosition = Vector3.zero;
        canvasObject.transform.localRotation = Quaternion.identity;

        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        if (scaler != null)
        {
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 0.5f;
        }

        // Only direct children of this existing Canvas are removed.
        for (int i = canvasObject.transform.childCount - 1; i >= 0; i--)
            Object.DestroyImmediate(canvasObject.transform.GetChild(i).gameObject);

        Transform root = canvasObject.transform;
        Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        Color panel = new Color(0.025f, 0.055f, 0.075f, 0.94f);
        Color panelAlt = new Color(0.035f, 0.085f, 0.105f, 0.94f);
        Color cyan = new Color(0.1f, 0.82f, 0.94f, 1f);
        Color white = new Color(0.82f, 0.91f, 0.94f, 1f);
        Color muted = new Color(0.39f, 0.58f, 0.64f, 1f);

        GameObject MakePanel(Transform parent, string name, Vector2 min, Vector2 max, Color color)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Image));
            go.transform.SetParent(parent, false);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMin = min;
            rect.anchorMax = max;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            go.GetComponent<Image>().color = color;
            return go;
        }

        Text MakeText(Transform parent, string name, string initial, Vector2 min,
                      Vector2 max, int size, Color color, FontStyle style = FontStyle.Normal)
        {
            GameObject go = new GameObject(name, typeof(RectTransform), typeof(Text));
            go.transform.SetParent(parent, false);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchorMin = min;
            rect.anchorMax = max;
            rect.offsetMin = new Vector2(12, 4);
            rect.offsetMax = new Vector2(-12, -4);
            Text text = go.GetComponent<Text>();
            text.font = font;
            text.text = initial;
            text.fontSize = size;
            text.color = color;
            text.fontStyle = style;
            text.alignment = TextAnchor.MiddleLeft;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            return text;
        }

        MakePanel(root, "HUD HEADER", new Vector2(.02f,.89f), new Vector2(.98f,.98f), panel);
        MakeText(root, "HUD TITLE", "OBSIDIAN PROTOCOL   /   UNIT INSPECTION",
            new Vector2(.025f,.90f), new Vector2(.70f,.97f), 25, cyan, FontStyle.Bold);
        Text service = MakeText(root, "SERVICE RECORD", "SERVICE RECORD  /  UNAVAILABLE",
            new Vector2(.68f,.90f), new Vector2(.975f,.97f), 15, white);

        MakePanel(root, "IDENTITY PANEL", new Vector2(.02f,.54f), new Vector2(.29f,.87f), panel);
        MakeText(root, "IDENTITY LABEL", "UNIT IDENTITY", new Vector2(.025f,.81f),
            new Vector2(.28f,.86f), 16, cyan, FontStyle.Bold);
        Text name = MakeText(root, "UNIT NAME", "NO UNIT SELECTED",
            new Vector2(.025f,.72f), new Vector2(.28f,.81f), 22, white, FontStyle.Bold);
        Text type = MakeText(root, "UNIT TYPE", "UNIT TYPE  /  UNAVAILABLE",
            new Vector2(.025f,.65f), new Vector2(.28f,.72f), 15, white);
        Text status = MakeText(root, "UNIT STATUS", "SERVICE STATUS  /  UNAVAILABLE",
            new Vector2(.025f,.56f), new Vector2(.28f,.65f), 15, cyan);

        MakePanel(root, "LIVE CONDITION PANEL", new Vector2(.71f,.54f),
            new Vector2(.98f,.87f), panel);
        MakeText(root, "CONDITION TITLE", "LIVE CONDITION",
            new Vector2(.715f,.81f), new Vector2(.975f,.86f), 16, cyan, FontStyle.Bold);
        Text health = MakeText(root, "LIVE HEALTH", "HEALTH     /     UNAVAILABLE",
            new Vector2(.715f,.73f), new Vector2(.975f,.81f), 16, white);
        Text armor = MakeText(root, "LIVE ARMOR", "ARMOR      /     UNAVAILABLE",
            new Vector2(.715f,.65f), new Vector2(.975f,.73f), 16, white);
        Text energy = MakeText(root, "LIVE ENERGY", "ENERGY     /     UNAVAILABLE",
            new Vector2(.715f,.57f), new Vector2(.975f,.65f), 16, white);

        MakePanel(root, "SERVICE HISTORY PANEL", new Vector2(.02f,.20f),
            new Vector2(.29f,.51f), panelAlt);
        MakeText(root, "HISTORY TITLE", "SERVICE HISTORY",
            new Vector2(.025f,.45f), new Vector2(.28f,.50f), 16, cyan, FontStyle.Bold);
        Text missions = MakeText(root, "MISSIONS", "MISSIONS     /     UNAVAILABLE",
            new Vector2(.025f,.37f), new Vector2(.28f,.45f), 15, white);
        Text victories = MakeText(root, "VICTORIES", "VICTORIES    /     UNAVAILABLE",
            new Vector2(.025f,.30f), new Vector2(.28f,.37f), 15, white);
        Text losses = MakeText(root, "LOSSES", "LOSSES       /     UNAVAILABLE",
            new Vector2(.025f,.23f), new Vector2(.28f,.30f), 15, white);

        MakePanel(root, "MAINTENANCE PANEL", new Vector2(.71f,.20f),
            new Vector2(.98f,.51f), panelAlt);
        MakeText(root, "MAINTENANCE TITLE", "MAINTENANCE RECORD",
            new Vector2(.715f,.45f), new Vector2(.975f,.50f), 16, cyan, FontStyle.Bold);
        Text damage = MakeText(root, "RECORDED DAMAGE", "RECORDED DAMAGE  /  UNAVAILABLE",
            new Vector2(.715f,.36f), new Vector2(.975f,.45f), 15, white);
        Text repairs = MakeText(root, "REPAIRS", "REPAIRS  /  UNAVAILABLE",
            new Vector2(.715f,.27f), new Vector2(.975f,.36f), 15, white);

        MakePanel(root, "FOOTER", new Vector2(.02f,.04f), new Vector2(.98f,.16f), panel);
        MakeText(root, "FOOTER LEFT", "TACTICAL SYSTEMS DIVISION",
            new Vector2(.025f,.05f), new Vector2(.45f,.15f), 13, muted);
        MakeText(root, "FOOTER RIGHT", "CONFIGURED DATA  /  SERVICE RECORD  /  LIVE CONDITION",
            new Vector2(.50f,.05f), new Vector2(.975f,.15f), 13, cyan);

        GameObject controllerObject = new GameObject("Unit Inspection Data Controller");
        controllerObject.transform.SetParent(root, false);
        var controller = controllerObject.AddComponent<SCN22UnitInspectionHUDController>();
        controller.unitName = name;
        controller.serviceId = service;
        controller.unitType = type;
        controller.status = status;
        controller.missions = missions;
        controller.victories = victories;
        controller.losses = losses;
        controller.damage = damage;
        controller.repairs = repairs;
        controller.liveHealth = health;
        controller.liveArmor = armor;
        controller.liveEnergy = energy;

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);
        AssetDatabase.SaveAssets();
        Debug.Log("SCN22 Unit Inspection HUD rebuilt. Backup: " + backup);
    }
}

