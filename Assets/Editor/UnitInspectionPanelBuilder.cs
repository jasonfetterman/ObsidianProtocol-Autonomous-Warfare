using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public static class UnitInspectionPanelBuilder
{
    private const string MenuPath =
        "Obsidian Protocol/Command Center/BUILD UNIT INSPECTION PANEL";

    [MenuItem(MenuPath)]
    public static void Build()
    {
        string scenePath = FindScene();

        if (string.IsNullOrEmpty(scenePath))
        {
            Debug.LogError("[UNIT INSPECTION] Command Center scene not found.");
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        var scene = EditorSceneManager.OpenScene(
            scenePath,
            OpenSceneMode.Single);

        GameObject fleetPanel = null;

        GameObject[] allObjects =
            Object.FindObjectsByType<GameObject>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None);

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "FLEET_REAL_PANEL")
            {
                fleetPanel = obj;
                break;
            }
        }

        if (fleetPanel == null)
        {
            Debug.LogError("[UNIT INSPECTION] FLEET_REAL_PANEL not found.");
            return;
        }

        Transform existing = FindDeep(
            fleetPanel.transform,
            "UNIT_INSPECTION_PANEL");

        if (existing != null)
        {
            Object.DestroyImmediate(existing.gameObject);
        }

        GameObject panel = CreateUI(
            "UNIT_INSPECTION_PANEL",
            fleetPanel.transform);

        RectTransform panelRect =
            panel.GetComponent<RectTransform>();

        panelRect.anchorMin = new Vector2(0.03f, 0.04f);
        panelRect.anchorMax = new Vector2(0.97f, 0.96f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        Image panelImage = panel.AddComponent<Image>();
        panelImage.color = new Color(0.025f, 0.035f, 0.045f, 0.98f);
        panelImage.raycastTarget = true;

        BuildHeader(panel.transform);
        BuildClose(panel.transform);
        BuildScrollArea(panel.transform);

        panel.SetActive(false);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Selection.activeGameObject = panel;

        Debug.Log(
            "[UNIT INSPECTION] BUILD COMPLETE. " +
            "UNIT_INSPECTION_PANEL created under FLEET_REAL_PANEL and left inactive.");
    }

    private static void BuildHeader(Transform parent)
    {
        GameObject header = CreateUI(
            "HEADER",
            parent);

        SetRect(
            header,
            new Vector2(0.025f, 0.91f),
            new Vector2(0.975f, 0.985f));

        CreateText(
            "UNIT_TITLE",
            header.transform,
            "WARDEN-021",
            30,
            FontStyle.Bold,
            new Vector2(0.02f, 0.40f),
            new Vector2(0.55f, 0.98f));

        CreateText(
            "UNIT_SUBTITLE",
            header.transform,
            "ADVANCED OPERATIONAL INTELLIGENCE",
            15,
            FontStyle.Normal,
            new Vector2(0.02f, 0.02f),
            new Vector2(0.70f, 0.45f));

        CreateText(
            "UNIT_STATUS",
            header.transform,
            "AIR // SURVEILLANCE / RECON // ACTIVE",
            14,
            FontStyle.Bold,
            new Vector2(0.68f, 0.25f),
            new Vector2(0.98f, 0.78f));
    }

    private static void BuildClose(Transform parent)
    {
        GameObject close = CreateUI(
            "CLOSE",
            parent);

        SetRect(
            close,
            new Vector2(0.94f, 0.925f),
            new Vector2(0.975f, 0.975f));

        Image image = close.AddComponent<Image>();
        image.color = new Color(0.15f, 0.18f, 0.21f, 1f);

        Button button = close.AddComponent<Button>();
        button.targetGraphic = image;

        ColorBlock colors = button.colors;
        colors.normalColor = image.color;
        colors.highlightedColor =
            new Color(0.25f, 0.30f, 0.34f, 1f);
        colors.pressedColor =
            new Color(0.35f, 0.40f, 0.44f, 1f);
        button.colors = colors;

        CreateText(
            "LABEL",
            close.transform,
            "X",
            18,
            FontStyle.Bold,
            Vector2.zero,
            Vector2.one);
    }

    private static void BuildScrollArea(Transform parent)
    {
        GameObject viewport = CreateUI(
            "INSPECTION_VIEWPORT",
            parent);

        SetRect(
            viewport,
            new Vector2(0.025f, 0.055f),
            new Vector2(0.975f, 0.895f));

        Image viewportImage = viewport.AddComponent<Image>();
        viewportImage.color =
            new Color(0.015f, 0.022f, 0.030f, 0.85f);
        viewportImage.raycastTarget = true;

        Mask mask = viewport.AddComponent<Mask>();
        mask.showMaskGraphic = true;

        GameObject content = CreateUI(
            "INSPECTION_CONTENT",
            viewport.transform);

        RectTransform contentRect =
            content.GetComponent<RectTransform>();

        contentRect.anchorMin =
            new Vector2(0f, 1f);

        contentRect.anchorMax =
            new Vector2(1f, 1f);

        contentRect.pivot =
            new Vector2(0.5f, 1f);

        contentRect.anchoredPosition =
            Vector2.zero;

        contentRect.sizeDelta =
            new Vector2(0f, 2850f);

        float y = 1f;
        float h = 0.045f;
        float gap = 0.008f;

        AddSection(content.transform, ref y, h, gap,
            "OPERATIONAL PARAMETERS",
            new[]
            {
                "Mission Type: Aerial Surveillance / Recon",
                "Optimal Deployment Zone: Open air / elevated terrain",
                "Command Latency: 0.4 s",
                "Relay Load: 38%",
                "Signal Integrity: 96%",
                "AI Sync Rate: 99%",
                "Tactical Refresh Interval: 3 s",
                "Autonomous Decision Depth: Tier II",
                "Override Priority: Network > AI > Manual"
            });

        AddSection(content.transform, ref y, h, gap,
            "ENVIRONMENTAL INTERACTION LAYER",
            new[]
            {
                "Terrain Adaptation: Open Air 1.0 / Urban 0.85 / Forest 0.65 / Snow 0.75 / Water 0.0",
                "Weather Sensitivity Index: 0.68",
                "Visibility: Day 1.0 / Night 0.7 / Fog 0.6 / Rain 0.8",
                "Signal Propagation: -0.2% per 10% humidity increase"
            });

        AddSection(content.transform, ref y, h, gap,
            "MISSION BEHAVIOR LAYER",
            new[]
            {
                "Recon Sweep — Sensor precision — 94%",
                "Target Marking — Relay accuracy — 91%",
                "Surveillance Hold — Stability — 97%",
                "Data Relay — Transmission integrity — 96%"
            });

        AddSection(content.transform, ref y, h, gap,
            "NETWORK DYNAMICS",
            new[]
            {
                "Node Handoff Efficiency: 96%",
                "Packet Loss Rate: 1.2% under load",
                "Encryption Latency: 0.09 s",
                "Mesh Self-Healing Threshold: 1 lost node",
                "Relay Saturation: Throughput drops 5% per node beyond 8"
            });

        AddSection(content.transform, ref y, h, gap,
            "INTER-UNIT COORDINATION",
            new[]
            {
                "Beacon — 0.93 — Mesh relay optimization",
                "Ironwalker — 0.78 — Ground-to-air data sync",
                "Drop — 0.85 — Delivery zone confirmation"
            });

        AddSection(content.transform, ref y, h, gap,
            "ENVIRONMENTAL MODIFIERS",
            new[]
            {
                "High Winds: -15% stability / -10% accuracy",
                "Heavy Rain: -20% visual range / -5% thermal clarity",
                "Fog: -25% visual range / +10% thermal range",
                "Night Operations: -30% visual range / +15% thermal range",
                "Electromagnetic Interference: -40% signal integrity / -25% relay range"
            });

        AddSection(content.transform, ref y, h, gap,
            "AI COGNITIVE LAYER",
            new[]
            {
                "Decision Confidence: 0.91",
                "Threat Recognition Accuracy: 88%",
                "Adaptive Learning Rate: 0.05 per mission",
                "Command Override Resistance: 0.08",
                "Cooperative Behavior Index: 0.92"
            });

        AddSection(content.transform, ref y, h, gap,
            "ENERGY CURVE LAYER",
            new[]
            {
                "Active: 55 Wh/min",
                "Idle: 12 Wh/min",
                "Idle Recovery: 6% per minute",
                "Recharge: 100% in 8 min"
            });

        AddSection(content.transform, ref y, h, gap,
            "AI BEHAVIOR MATRIX",
            new[]
            {
                "Patrol — 0.75 — Maintain aerial perimeter sweep",
                "Scan — 0.90 — Prioritize sensor data acquisition",
                "Mark Target — 0.85 — Tag and relay recon data",
                "Stabilize — 0.95 — Maintain hover and imaging stability",
                "Retreat — 0.60 — Withdraw under threat or low battery",
                "Re-Engage — 0.55 — Resume surveillance after recharge",
                "Assist — 0.70 — Support allied recon units",
                "Override Response — 0.80 — Accept network command instantly"
            });

        AddSection(content.transform, ref y, h, gap,
            "RESOURCE & MAINTENANCE ANALYTICS",
            new[]
            {
                "Energy Recovery Rate: 6% per idle minute",
                "Component Wear Curve: 0.3% degradation per hour under load",
                "Maintenance Priority: Tier II",
                "Spare Part Dependency Index: 0.52",
                "Operational Cost per Hour: 4 Metal / 3 Electronics"
            });

        AddSection(content.transform, ref y, h, gap,
            "UPGRADE SCALING",
            new[]
            {
                "Thermal Lens Upgrade: +15% thermal range / +8% mission success",
                "Extended Battery Module: +20% endurance / +10% uptime",
                "Silent Rotor Kit: -25% noise signature / +5% stealth efficiency"
            });

        AddSection(content.transform, ref y, h, gap,
            "MAINTENANCE & LOGISTICS CURVES",
            new[]
            {
                "Mean Time Between Failures: 68 h",
                "Repair Cycle: 10 s",
                "Resource Drain per Hour: 1.1%",
                "Battery Efficiency: 93%",
                "Rotor Stability: 96%",
                "Sensor Calibration Drift: 0.4% per hour",
                "Cooling Efficiency: 89%",
                "Operational Uptime: 97%"
            });

        AddSection(content.transform, ref y, h, gap,
            "TACTICAL INTEGRATION",
            new[]
            {
                "Formation Adaptability: 0.87",
                "Tactical Role Weight: 0.78",
                "Command Response Delay: 0.4 s",
                "Reinforcement: Battery < 20% or Signal Loss > 30%",
                "Retreat: Structural Integrity < 30% or Aggression > 0.2"
            });

        AddSection(content.transform, ref y, h, gap,
            "PERFORMANCE METRICS",
            new[]
            {
                "Mission Success Probability: 92%",
                "Average Engagement Duration: 4.2 min",
                "Recon Data Yield: 18 MB/min",
                "Communication Uptime: 98%",
                "Threat Avoidance Efficiency: 83%"
            });

        AddSection(content.transform, ref y, h, gap,
            "THREAT-RESPONSE MODEL",
            new[]
            {
                "Ballistic — 0.2 s — 41%",
                "Explosive — 0.6 s — 38%",
                "EMP — 1.0 s — 32%",
                "Thermal — 0.3 s — 55%",
                "Signal Jam — 0.8 s — 47%"
            });

        AddSection(content.transform, ref y, h, gap,
            "COMMAND-NETWORK ANALYTICS",
            new[]
            {
                "Standard Warden Mesh — 38% — 0.4 s — Optimal",
                "Relay Node Alpha — 42% — 0.6 s — Stable",
                "Sentinel / Overseer — 45% — 0.5 s — Active",
                "Drone Bay Control — 40% — 0.7 s — Stable"
            });

        AddSection(content.transform, ref y, h, gap,
            "DAMAGE RESPONSE",
            new[]
            {
                "Ballistic: Sensor degradation -10% — Auto-recalibration in 30 s",
                "EMP: Signal loss -40% — Reboot in 90 s",
                "Weather: Stability -15% — Auto-hover compensation in 20 s"
            });
    }

    private static void AddSection(
        Transform parent,
        ref float y,
        float h,
        float gap,
        string title,
        string[] lines)
    {
        float sectionHeight =
            0.018f + lines.Length * 0.018f;

        GameObject section = CreateUI(
            title.Replace(" ", "_"),
            parent);

        RectTransform rect =
            section.GetComponent<RectTransform>();

        rect.anchorMin =
            new Vector2(0.02f, y - sectionHeight);

        rect.anchorMax =
            new Vector2(0.98f, y);

        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        Image bg = section.AddComponent<Image>();
        bg.color =
            new Color(0.035f, 0.050f, 0.065f, 0.96f);

        CreateText(
            "TITLE",
            section.transform,
            title,
            17,
            FontStyle.Bold,
            new Vector2(0.015f, 0.82f),
            new Vector2(0.985f, 0.995f));

        string body = string.Join("\n", lines);

        CreateText(
            "DATA",
            section.transform,
            body,
            13,
            FontStyle.Normal,
            new Vector2(0.02f, 0.04f),
            new Vector2(0.98f, 0.80f));

        y -= sectionHeight + gap;
    }

    private static GameObject CreateUI(
        string name,
        Transform parent)
    {
        GameObject obj =
            new GameObject(
                name,
                typeof(RectTransform));

        obj.transform.SetParent(
            parent,
            false);

        return obj;
    }

    private static void SetRect(
        GameObject obj,
        Vector2 min,
        Vector2 max)
    {
        RectTransform rect =
            obj.GetComponent<RectTransform>();

        rect.anchorMin = min;
        rect.anchorMax = max;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private static Text CreateText(
        string name,
        Transform parent,
        string value,
        int size,
        FontStyle style,
        Vector2 min,
        Vector2 max)
    {
        GameObject obj =
            CreateUI(name, parent);

        SetRect(obj, min, max);

        Text text =
            obj.AddComponent<Text>();

        text.text = value;
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = size;
        text.fontStyle = style;
        text.color = new Color(0.78f, 0.84f, 0.88f, 1f);
        text.alignment = TextAnchor.UpperLeft;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;
        text.raycastTarget = false;

        return text;
    }

    private static Transform FindDeep(
        Transform root,
        string name)
    {
        foreach (Transform child in root.GetComponentsInChildren<Transform>(true))
        {
            if (child.name == name)
                return child;
        }

        return null;
    }

    private static string FindScene()
    {
        string[] guids =
            AssetDatabase.FindAssets(
                "Command_Center t:Scene");

        foreach (string guid in guids)
        {
            string path =
                AssetDatabase.GUIDToAssetPath(guid);

            if (path.EndsWith(
                "Command_Center.unity"))
            {
                return path;
            }
        }

        return null;
    }
}