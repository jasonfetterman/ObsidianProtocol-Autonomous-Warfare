using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class FleetPanelProfessionalLayoutBuilder
{
    private static GameObject textTemplate;
    private static GameObject buttonTemplate;
    private static GameObject imageTemplate;

    [MenuItem("Obsidian Protocol/Command Center/REDO FLEET PANEL - MATCH COMMAND")]
    public static void Build()
    {
        GameObject commandPanel = FindObject("COMMAND_PANEL");
        GameObject fleetPanel = FindObject("FLEET_REAL_PANEL");

        if (commandPanel == null)
        {
            Debug.LogError("[FLEET] COMMAND_PANEL not found.");
            return;
        }

        if (fleetPanel == null)
        {
            Debug.LogError("[FLEET] FLEET_PANEL not found.");
            return;
        }

        textTemplate = FindComponentObject<Text>(commandPanel.transform);
        buttonTemplate = FindComponentObject<Button>(commandPanel.transform);
        imageTemplate = FindComponentObject<Image>(commandPanel.transform);

        if (textTemplate == null)
        {
            Debug.LogError("[FLEET] Working Text template not found.");
            return;
        }

        if (buttonTemplate == null)
        {
            Debug.LogError("[FLEET] Working Button template not found.");
            return;
        }

        for (int i = fleetPanel.transform.childCount - 1; i >= 0; i--)
            Object.DestroyImmediate(fleetPanel.transform.GetChild(i).gameObject);

        GameObject root = CreateRect(
            "FLEET_CONTENT",
            fleetPanel.transform,
            Vector2.zero,
            Vector2.one
        );

        // HEADER
        CreateText(
            root.transform,
            "FLEET",
            new Vector2(.035f, .915f),
            new Vector2(.35f, .975f),
            28,
            TextAnchor.MiddleLeft
        );

        CreateText(
            root.transform,
            "FORCE MANAGEMENT  //  READINESS  //  UNIT CONTROL",
            new Vector2(.035f, .875f),
            new Vector2(.65f, .915f),
            11,
            TextAnchor.MiddleLeft
        );

        CreateText(
            root.transform,
            "FLEET NETWORK ONLINE  |  42 TOTAL  |  24 DEPLOYED  |  18 AVAILABLE",
            new Vector2(.36f, .915f),
            new Vector2(.84f, .975f),
            10,
            TextAnchor.MiddleRight
        );

        // CLOSE
        Transform sourceClose = commandPanel.transform.Find("CLOSE");

        if (sourceClose != null)
        {
            GameObject close =
                Object.Instantiate(sourceClose.gameObject, root.transform);

            close.name = "CLOSE";

            RectTransform closeRect =
                close.GetComponent<RectTransform>();

            if (closeRect != null)
            {
                closeRect.anchorMin = new Vector2(.90f, .915f);
                closeRect.anchorMax = new Vector2(.965f, .975f);
                closeRect.offsetMin = Vector2.zero;
                closeRect.offsetMax = Vector2.zero;
            }

            Button closeButton =
                close.GetComponent<Button>();

            if (closeButton != null)
            {
                closeButton.onClick.RemoveAllListeners();

                GameObject controllerObject =
                    FindObject("COMMAND_CENTER_FULL_INTERACTION");

                if (controllerObject != null)
                {
                    CommandCenterFullController controller =
                        controllerObject.GetComponent<CommandCenterFullController>();

                    if (controller != null)
                    {
                        UnityEditor.Events.UnityEventTools.AddPersistentListener(
                            closeButton.onClick,
                            controller.CloseAll
                        );
                    }
                }
            }
        }

        // SUMMARY
        CreatePanel(
            root.transform,
            "SUMMARY",
            new Vector2(.035f, .775f),
            new Vector2(.965f, .855f)
        );

        CreateMetric(root.transform, "42", "TOTAL UNITS", .055f);
        CreateMetric(root.transform, "24", "DEPLOYED", .235f);
        CreateMetric(root.transform, "18", "AVAILABLE", .415f);
        CreateMetric(root.transform, "86%", "COMBAT READY", .595f);
        CreateMetric(root.transform, "3", "DAMAGED", .775f);

        // ACTIVE FORCE
        GameObject active =
            CreatePanel(
                root.transform,
                "ACTIVE_FORCE",
                new Vector2(.035f, .425f),
                new Vector2(.565f, .755f)
            );

        CreateSectionHeader(
            active.transform,
            "ACTIVE FORCE",
            "DEPLOYED UNITS"
        );

        CreateUnit(active.transform, 0, "BULLDOG-024", "GROUND", "VETERAN II", "ACTIVE");
        CreateUnit(active.transform, 1, "WARDEN-011", "AIR", "VETERAN I", "ACTIVE");
        CreateUnit(active.transform, 2, "SENTINEL-032", "GROUND", "STANDARD", "ACTIVE");
        CreateUnit(active.transform, 3, "BEACON-018", "SUPPORT", "STANDARD", "ACTIVE");
        CreateUnit(active.transform, 4, "FORGE-006", "LOGISTICS", "VETERAN I", "STANDBY");

        // READINESS
        GameObject readiness =
            CreatePanel(
                root.transform,
                "READINESS",
                new Vector2(.585f, .425f),
                new Vector2(.965f, .755f)
            );

        CreateSectionHeader(
            readiness.transform,
            "FLEET READINESS",
            "SYSTEM CONDITION"
        );

        CreateReadiness(readiness.transform, "COMBAT", "86%", .67f);
        CreateReadiness(readiness.transform, "MOBILITY", "91%", .54f);
        CreateReadiness(readiness.transform, "ARMAMENT", "83%", .41f);
        CreateReadiness(readiness.transform, "SUPPLY", "78%", .28f);
        CreateReadiness(readiness.transform, "COMMS", "94%", .15f);

        // AVAILABLE UNITS
        GameObject available =
            CreatePanel(
                root.transform,
                "AVAILABLE_UNITS",
                new Vector2(.035f, .185f),
                new Vector2(.965f, .405f)
            );

        CreateSectionHeader(
            available.transform,
            "AVAILABLE UNITS",
            "READY FORCE"
        );

        CreateAvailableRow(available.transform, 0, "WARDEN-021", "AIR", "READY");
        CreateAvailableRow(available.transform, 1, "BEACON-024", "SUPPORT", "READY");
        CreateAvailableRow(available.transform, 2, "BULLDOG-031", "GROUND", "READY");
        CreateAvailableRow(available.transform, 3, "SENTINEL-041", "GROUND", "READY");

        // COMMAND BAR
        GameObject commandBar =
            CreatePanel(
                root.transform,
                "FLEET_COMMAND",
                new Vector2(.035f, .065f),
                new Vector2(.965f, .165f)
            );

        CreateText(
            commandBar.transform,
            "FLEET COMMAND",
            new Vector2(.025f, .25f),
            new Vector2(.19f, .75f),
            12,
            TextAnchor.MiddleLeft
        );

        CreateButton(commandBar.transform, "INSPECT", .22f);
        CreateButton(commandBar.transform, "DEPLOY", .37f);
        CreateButton(commandBar.transform, "RECALL", .52f);
        CreateButton(commandBar.transform, "GARAGE", .67f);
        CreateButton(commandBar.transform, "OPERATIONS", .82f);

        CreateText(
            root.transform,
            "FLEET CONTROL  //  AUTONOMOUS FORCE MANAGEMENT  //  SYSTEMS NOMINAL",
            new Vector2(.035f, .018f),
            new Vector2(.75f, .045f),
            9,
            TextAnchor.MiddleLeft
        );

        EditorUtility.SetDirty(fleetPanel);

        EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );

        EditorSceneManager.SaveScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene()
        );

        Debug.Log("[FLEET] PANEL REBUILT USING WORKING COMMAND PANEL UI TEMPLATES.");
    }

    private static GameObject CreatePanel(
        Transform parent,
        string name,
        Vector2 min,
        Vector2 max)
    {
        GameObject obj =
            CreateRect(name, parent, min, max);

        Image image =
            obj.GetComponent<Image>();

        if (image == null)
            image = obj.AddComponent<Image>();

        image.raycastTarget = false;

        if (imageTemplate != null)
        {
            Image source =
                imageTemplate.GetComponent<Image>();

            if (source != null)
            {
                image.sprite = source.sprite;
                image.type = source.type;
                image.material = source.material;
                image.color = source.color;
            }
        }

        return obj;
    }
    private static GameObject CreateText(
        Transform parent,
        string value,
        Vector2 min,
        Vector2 max,
        int size,
        TextAnchor alignment)
    {
        GameObject obj =
            Object.Instantiate(textTemplate, parent);

        obj.name = "TEXT";

        RectTransform rect =
            obj.GetComponent<RectTransform>();

        if (rect != null)
        {
            rect.anchorMin = min;
            rect.anchorMax = max;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        Text text =
            obj.GetComponent<Text>();

        if (text != null)
        {
            text.text = value;
            text.fontSize = size;
            text.alignment = alignment;
            text.raycastTarget = false;
        }

        return obj;
    }

    private static GameObject CreateButton(
        Transform parent,
        string label,
        float x)
    {
        GameObject obj =
            Object.Instantiate(buttonTemplate, parent);

        obj.name = label;

        RectTransform rect =
            obj.GetComponent<RectTransform>();

        if (rect != null)
        {
            rect.anchorMin = new Vector2(x, .15f);
            rect.anchorMax = new Vector2(x + .13f, .85f);
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        Button button =
            obj.GetComponent<Button>();

        if (button != null)
            button.onClick.RemoveAllListeners();

        Text text =
            obj.GetComponentInChildren<Text>(true);

        if (text != null)
        {
            text.text = label;
            text.raycastTarget = false;
        }

        return obj;
    }

    private static void CreateMetric(
        Transform parent,
        string value,
        string label,
        float x)
    {
        CreateText(
            parent,
            value,
            new Vector2(x, .80f),
            new Vector2(x + .14f, .96f),
            18,
            TextAnchor.MiddleCenter
        );

        CreateText(
            parent,
            label,
            new Vector2(x, .775f),
            new Vector2(x + .14f, .815f),
            9,
            TextAnchor.MiddleCenter
        );
    }

    private static void CreateSectionHeader(
        Transform parent,
        string title,
        string subtitle)
    {
        CreateText(
            parent,
            title,
            new Vector2(.035f, .86f),
            new Vector2(.65f, .98f),
            14,
            TextAnchor.MiddleLeft
        );

        CreateText(
            parent,
            subtitle,
            new Vector2(.65f, .87f),
            new Vector2(.96f, .97f),
            8,
            TextAnchor.MiddleRight
        );
    }

    private static void CreateUnit(
        Transform parent,
        int index,
        string unit,
        string type,
        string className,
        string status)
    {
        float top =
            .77f - index * .135f;

        float bottom =
            top - .105f;

        GameObject row =
            CreateRect(
                "UNIT_" + index,
                parent,
                new Vector2(.035f, bottom),
                new Vector2(.965f, top)
            );

        CreateText(
            row.transform,
            unit,
            new Vector2(.025f, .1f),
            new Vector2(.36f, .9f),
            10,
            TextAnchor.MiddleLeft
        );

        CreateText(
            row.transform,
            type,
            new Vector2(.38f, .1f),
            new Vector2(.57f, .9f),
            9,
            TextAnchor.MiddleLeft
        );

        CreateText(
            row.transform,
            className,
            new Vector2(.59f, .1f),
            new Vector2(.79f, .9f),
            9,
            TextAnchor.MiddleLeft
        );

        CreateText(
            row.transform,
            status,
            new Vector2(.80f, .1f),
            new Vector2(.97f, .9f),
            9,
            TextAnchor.MiddleRight
        );
    }

    private static void CreateReadiness(
        Transform parent,
        string label,
        string value,
        float y)
    {
        CreateText(
            parent,
            label,
            new Vector2(.055f, y),
            new Vector2(.25f, y + .07f),
            9,
            TextAnchor.MiddleLeft
        );

        CreateText(
            parent,
            value,
            new Vector2(.82f, y),
            new Vector2(.95f, y + .07f),
            9,
            TextAnchor.MiddleRight
        );
    }

    private static void CreateAvailableRow(
        Transform parent,
        int index,
        string unit,
        string type,
        string status)
    {
        float top =
            .76f - index * .14f;

        float bottom =
            top - .11f;

        GameObject row =
            CreateRect(
                "AVAILABLE_" + index,
                parent,
                new Vector2(.035f, bottom),
                new Vector2(.965f, top)
            );

        CreateText(
            row.transform,
            unit,
            new Vector2(.025f, .1f),
            new Vector2(.45f, .9f),
            10,
            TextAnchor.MiddleLeft
        );

        CreateText(
            row.transform,
            type,
            new Vector2(.48f, .1f),
            new Vector2(.68f, .9f),
            9,
            TextAnchor.MiddleLeft
        );

        CreateText(
            row.transform,
            status,
            new Vector2(.72f, .1f),
            new Vector2(.97f, .9f),
            9,
            TextAnchor.MiddleRight
        );
    }

    private static GameObject CreateRect(
        string name,
        Transform parent,
        Vector2 min,
        Vector2 max)
    {
        GameObject obj =
            new GameObject(
                name,
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image)
            );

        obj.transform.SetParent(parent, false);

        Image containerImage =
            obj.GetComponent<Image>();

        if (containerImage != null)
        {
            containerImage.color = new Color(1f, 1f, 1f, 0f);
            containerImage.raycastTarget = false;
        }

        RectTransform rect =
            obj.GetComponent<RectTransform>();

        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        return obj;
    }

    private static GameObject FindObject(string name)
    {
        GameObject[] objects =
            Object.FindObjectsByType<GameObject>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        foreach (GameObject obj in objects)
        {
            if (obj.name == name)
                return obj;
        }

        return null;
    }

    private static GameObject FindComponentObject<T>(
        Transform root) where T : Component
    {
        T component =
            root.GetComponentInChildren<T>(true);

        if (component != null)
            return component.gameObject;

        return null;
    }
}





