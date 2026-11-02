using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public static class DeploymentForceCompositionHoloBuilder
{
    private const string LeftRootName =
        "[HOLO] FORCE COMPOSITION - FORCE";

    private const string RightRootName =
        "[HOLO] FORCE COMPOSITION - OVERVIEW";

    private static readonly Color Holo =
        new Color(1f, 0.549f, 0.157f, 1f);

    private static readonly Color HoloSoft =
        new Color(1f, 0.549f, 0.157f, 0.35f);

    private static readonly Color HoloDim =
        new Color(1f, 0.549f, 0.157f, 0.12f);

    private static readonly Color White =
        new Color(0.86f, 0.94f, 1f, 0.92f);

    private static readonly Color DimWhite =
        new Color(0.65f, 0.75f, 0.82f, 0.72f);

    [MenuItem(
        "Obsidian Protocol/Deployment/Build Force Composition Holo Screens")]
    public static void Build()
    {
        GameObject existingLeft =
            GameObject.Find(LeftRootName);

        GameObject existingRight =
            GameObject.Find(RightRootName);

        if (existingLeft != null)
            Object.DestroyImmediate(existingLeft);

        if (existingRight != null)
            Object.DestroyImmediate(existingRight);

        GameObject left =
            CreateScreen(
                LeftRootName,
                new Vector3(-5.61f, 7.00f, 18.455f));

        GameObject right =
            CreateScreen(
                RightRootName,
                new Vector3(-11.81f, 7.00f, 18.455f));

        BuildForceScreen(left.transform);
        BuildOverviewScreen(right.transform);

        EditorSceneManager.MarkSceneDirty(
            EditorSceneManager.GetActiveScene());

        Debug.Log(
            "[HOLO] Force Composition holo screens created.");
    }

    private static GameObject CreateScreen(
        string name,
        Vector3 position)
    {
        GameObject root =
            new GameObject(name);

        root.transform.position = position;
        root.transform.rotation = Quaternion.identity;
        root.transform.localScale =
            new Vector3(0.01f, 0.01f, 0.01f);

        Canvas canvas =
            root.AddComponent<Canvas>();

        canvas.renderMode =
            RenderMode.WorldSpace;

        RectTransform rect =
            canvas.GetComponent<RectTransform>();

        rect.sizeDelta =
            new Vector2(580f, 240f);

        CreateGlass(root.transform);
        CreateBorder(root.transform);
        CreateScanlines(root.transform);
        CreateGrid(root.transform);

        return root;
    }

    private static void BuildForceScreen(
        Transform root)
    {
        CreateText(
            root,
            "TITLE",
            "FORCE COMPOSITION",
            new Vector2(0f, 94f),
            new Vector2(530f, 32f),
            24,
            Holo,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "SYSTEM",
            "FORCE PACKAGE // DEPLOYMENT LOADOUT",
            new Vector2(0f, 70f),
            new Vector2(530f, 18f),
            11,
            DimWhite,
            TextAnchor.MiddleLeft);

        CreatePanel(
            root,
            "FORCE SUMMARY",
            new Vector2(-135f, 23f),
            new Vector2(245f, 72f));

        CreatePanel(
            root,
            "FORCE STATUS",
            new Vector2(135f, 23f),
            new Vector2(245f, 72f));

        CreateText(
            root,
            "SUMMARY_HEADER",
            "SELECTED FORCE",
            new Vector2(-135f, 48f),
            new Vector2(215f, 18f),
            11,
            Holo,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "SUMMARY_VALUE",
            "5 UNITS",
            new Vector2(-135f, 27f),
            new Vector2(215f, 26f),
            20,
            White,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "SUMMARY_DETAIL",
            "WARDEN  /  BEACON  /  BULLDOG",
            new Vector2(-135f, 7f),
            new Vector2(215f, 18f),
            9,
            DimWhite,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "STATUS_HEADER",
            "FORCE READINESS",
            new Vector2(135f, 48f),
            new Vector2(215f, 18f),
            11,
            Holo,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "STATUS_VALUE",
            "READY",
            new Vector2(135f, 27f),
            new Vector2(215f, 26f),
            20,
            Holo,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "STATUS_DETAIL",
            "CAPACITY  05 / 08",
            new Vector2(135f, 7f),
            new Vector2(215f, 18f),
            9,
            DimWhite,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "UNIT_01",
            "01   WARDEN",
            new Vector2(-190f, -25f),
            new Vector2(180f, 18f),
            11,
            White,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "UNIT_01_ROLE",
            "COMMAND / CONTROL",
            new Vector2(-190f, -43f),
            new Vector2(180f, 16f),
            8,
            DimWhite,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "UNIT_02",
            "02   BEACON",
            new Vector2(0f, -25f),
            new Vector2(180f, 18f),
            11,
            White,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "UNIT_02_ROLE",
            "RECON / SENSOR",
            new Vector2(0f, -43f),
            new Vector2(180f, 16f),
            8,
            DimWhite,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "UNIT_03",
            "03   BULLDOG",
            new Vector2(190f, -25f),
            new Vector2(180f, 18f),
            11,
            White,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "UNIT_03_ROLE",
            "ASSAULT / GROUND",
            new Vector2(190f, -43f),
            new Vector2(180f, 16f),
            8,
            DimWhite,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "FOOTER",
            "FORCE COMPOSITION CONTROL  //  AWAITING FINAL CONFIRMATION",
            new Vector2(0f, -91f),
            new Vector2(530f, 18f),
            9,
            HoloSoft,
            TextAnchor.MiddleCenter);
    }

    private static void BuildOverviewScreen(
        Transform root)
    {
        CreateText(
            root,
            "TITLE",
            "FORCE OVERVIEW",
            new Vector2(0f, 94f),
            new Vector2(530f, 32f),
            24,
            Holo,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "SYSTEM",
            "DEPLOYMENT FORCE // COMPOSITION ANALYSIS",
            new Vector2(0f, 70f),
            new Vector2(530f, 18f),
            11,
            DimWhite,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "TOTAL_LABEL",
            "TOTAL FORCE",
            new Vector2(-195f, 42f),
            new Vector2(170f, 18f),
            10,
            Holo,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "TOTAL_VALUE",
            "05",
            new Vector2(-195f, 15f),
            new Vector2(170f, 32f),
            25,
            White,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "COMMAND_LABEL",
            "COMMAND",
            new Vector2(-55f, 42f),
            new Vector2(150f, 18f),
            10,
            Holo,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "COMMAND_VALUE",
            "1",
            new Vector2(-55f, 15f),
            new Vector2(150f, 32f),
            25,
            White,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "COMBAT_LABEL",
            "COMBAT",
            new Vector2(70f, 42f),
            new Vector2(150f, 18f),
            10,
            Holo,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "COMBAT_VALUE",
            "02",
            new Vector2(70f, 15f),
            new Vector2(150f, 32f),
            25,
            White,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "SUPPORT_LABEL",
            "SUPPORT",
            new Vector2(195f, 42f),
            new Vector2(150f, 18f),
            10,
            Holo,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "SUPPORT_VALUE",
            "02",
            new Vector2(195f, 15f),
            new Vector2(150f, 32f),
            25,
            White,
            TextAnchor.MiddleLeft);

        CreateText(
            root,
            "BAR_LABEL",
            "FORCE ALLOCATION",
            new Vector2(0f, -18f),
            new Vector2(500f, 18f),
            10,
            Holo,
            TextAnchor.MiddleLeft);

        CreateLine(
            root,
            "ALLOCATION_BAR",
            new Vector2(0f, -42f),
            new Vector2(470f, 5f),
            Holo);

        CreateLine(
            root,
            "ALLOCATION_1",
            new Vector2(-120f, -42f),
            new Vector2(4f, 15f),
            Holo);

        CreateLine(
            root,
            "ALLOCATION_2",
            new Vector2(0f, -42f),
            new Vector2(4f, 15f),
            Holo);

        CreateLine(
            root,
            "ALLOCATION_3",
            new Vector2(120f, -42f),
            new Vector2(4f, 15f),
            Holo);

        CreateText(
            root,
            "ALLOCATION_DETAIL",
            "COMMAND        COMBAT        SUPPORT",
            new Vector2(0f, -63f),
            new Vector2(470f, 16f),
            8,
            DimWhite,
            TextAnchor.MiddleCenter);

        CreateText(
            root,
            "READINESS",
            "READINESS  92%     |     DEPLOYMENT CAPACITY  62%",
            new Vector2(0f, -82f),
            new Vector2(500f, 18f),
            10,
            White,
            TextAnchor.MiddleCenter);

        CreateText(
            root,
            "FOOTER",
            "FORCE COMPOSITION LINK  //  LIVE PACKAGE DATA",
            new Vector2(0f, -103f),
            new Vector2(530f, 16f),
            8,
            HoloSoft,
            TextAnchor.MiddleCenter);
    }

    private static void CreateGlass(
        Transform parent)
    {
        GameObject go =
            new GameObject("HOLO GLASS");

        go.transform.SetParent(parent, false);

        Image image =
            go.AddComponent<Image>();

        image.color =
            new Color(0.005f, 0.015f, 0.02f, 0.30f);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    private static void CreateBorder(
        Transform parent)
    {
        CreateLine(
            parent,
            "BORDER TOP",
            new Vector2(0f, 115f),
            new Vector2(570f, 2f),
            Holo);

        CreateLine(
            parent,
            "BORDER BOTTOM",
            new Vector2(0f, -115f),
            new Vector2(570f, 2f),
            Holo);

        CreateLine(
            parent,
            "BORDER LEFT",
            new Vector2(-285f, 0f),
            new Vector2(2f, 230f),
            Holo);

        CreateLine(
            parent,
            "BORDER RIGHT",
            new Vector2(285f, 0f),
            new Vector2(2f, 230f),
            Holo);
    }

    private static void CreateScanlines(
        Transform parent)
    {
        for (int i = -10; i <= 10; i++)
        {
            CreateLine(
                parent,
                "SCANLINE_" + i,
                new Vector2(0f, i * 10f),
                new Vector2(550f, 1f),
                new Color(
                    Holo.r,
                    Holo.g,
                    Holo.b,
                    0.035f));
        }
    }

    private static void CreateGrid(
        Transform parent)
    {
        for (int x = -5; x <= 5; x++)
        {
            CreateLine(
                parent,
                "GRID_V_" + x,
                new Vector2(x * 50f, 0f),
                new Vector2(1f, 220f),
                HoloDim);
        }

        for (int y = -4; y <= 4; y++)
        {
            CreateLine(
                parent,
                "GRID_H_" + y,
                new Vector2(0f, y * 25f),
                new Vector2(550f, 1f),
                HoloDim);
        }
    }

    private static void CreatePanel(
        Transform parent,
        string name,
        Vector2 position,
        Vector2 size)
    {
        GameObject go =
            new GameObject(name);

        go.transform.SetParent(parent, false);

        Image image =
            go.AddComponent<Image>();

        image.color =
            new Color(
                Holo.r,
                Holo.g,
                Holo.b,
                0.025f);

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.sizeDelta = size;
        rect.anchoredPosition = position;
    }

    private static void CreateLine(
        Transform parent,
        string name,
        Vector2 position,
        Vector2 size,
        Color color)
    {
        GameObject go =
            new GameObject(name);

        go.transform.SetParent(parent, false);

        Image image =
            go.AddComponent<Image>();

        image.color = color;

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.sizeDelta = size;
        rect.anchoredPosition = position;
    }

    private static void CreateText(
        Transform parent,
        string name,
        string value,
        Vector2 position,
        Vector2 size,
        int fontSize,
        Color color,
        TextAnchor alignment)
    {
        GameObject go =
            new GameObject(name);

        go.transform.SetParent(parent, false);

        Text text =
            go.AddComponent<Text>();

        text.font =
            Resources.GetBuiltinResource<Font>(
                "LegacyRuntime.ttf");

        text.text = value;
        text.fontSize = fontSize;
        text.color = color;
        text.alignment = alignment;
        text.horizontalOverflow =
            HorizontalWrapMode.Overflow;
        text.verticalOverflow =
            VerticalWrapMode.Overflow;

        RectTransform rect =
            go.GetComponent<RectTransform>();

        rect.sizeDelta = size;
        rect.anchoredPosition = position;
    }
}
