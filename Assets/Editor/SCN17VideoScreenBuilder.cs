using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public static class SCN17VideoScreenBuilder
{
    const string ScenePath = "Assets/Scenes/SCN17_PERSISTENT_MILITARY/SCN17_PERSISTENT_MILITARY.unity";
    const string RootName = "SCN17 HOLOGRAPHIC INFO BOARDS";

    static readonly string[] Screens = {
        "AI DEVELOPMENT SCREEN", "DAMAGE HISTORY SCREEN", "FLEET GROWTH SCREEN",
        "REPAIR RECORD A SCREEN", "REPAIR RECORD B SCREEN", "REPAIR RECORD C SCREEN",
        "MISSION HISTORY SCREEN", "EQUIPMENT TIMELINE SCREEN",
        "OPERATIONAL EXPERIENCE SCREEN", "COMMANDER DEVELOPMENT SCREEN",
        "RECOVERY STATUS SCREEN", "REPAIR HISTORY SCREEN",
        "DOCTRINE DEVELOPMENT SCREEN", "MISSION ARCHIVE A SCREEN",
        "MISSION ARCHIVE B SCREEN", "MISSION ARCHIVE C SCREEN",
        "COMBAT HISTORY SCREEN", "TECHNOLOGY GROWTH SCREEN",
        "LOSS RECORD SCREEN", "VETERAN TERMINAL A SCREEN",
        "VETERAN TERMINAL B SCREEN", "VETERAN TERMINAL C SCREEN",
        "UNIT HISTORY TERMINAL A SCREEN", "UNIT HISTORY TERMINAL B SCREEN",
        "UNIT HISTORY TERMINAL C SCREEN", "UNIT RECORDS DISPLAY",
        "SERVICE HISTORY DISPLAY", "MILITARY DEVELOPMENT DISPLAY",
        "ARCHIVE CONTROL DISPLAY", "VETERAN RANK DISPLAY",
        "UNIT RECORD — BULLDOG-024 SCREEN"
    };

    [MenuItem("Obsidian Protocol/SCN17/Build Holographic Info Boards")]
    public static void Build()
    {
        if (EditorSceneManager.GetActiveScene().path != ScenePath)
        {
            Debug.LogError("[SCN17 HOLO] Open SCN17_PERSISTENT_MILITARY first.");
            return;
        }

        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            return;

        GameObject architecture = GameObject.Find("FACILITY ARCHITECTURE");
        if (architecture == null)
        {
            Debug.LogError("[SCN17 HOLO] FACILITY ARCHITECTURE not found.");
            return;
        }

        foreach (string oldName in new[] {
            RootName, "SCN17 HOLOGRAM UPGRADE", "SCN17 VIDEO SCREEN UPGRADE"
        })
        {
            Transform old = architecture.transform.Find(oldName);
            if (old != null) Object.DestroyImmediate(old.gameObject);
        }

        Shader shader = Shader.Find("Universal Render Pipeline/Unlit");
        if (shader == null) shader = Shader.Find("Unlit/Color");
        if (shader == null) shader = Shader.Find("Sprites/Default");

        if (shader == null)
        {
            Debug.LogError("[SCN17 HOLO] No compatible shader found.");
            return;
        }

        GameObject root = new GameObject(RootName);
        Undo.RegisterCreatedObjectUndo(root, "Build Holographic Info Boards");
        root.transform.SetParent(architecture.transform, false);

        int built = 0;

        foreach (string name in Screens)
        {
            GameObject screen = FindScreen(name);
            if (screen == null) continue;

            Renderer source = screen.GetComponent<Renderer>();
            if (source == null) source = screen.GetComponentInChildren<Renderer>();
            if (source == null) continue;

            MeshFilter mf = source.GetComponent<MeshFilter>();
            if (mf == null || mf.sharedMesh == null) continue;

            Bounds b = mf.sharedMesh.bounds;
            Vector3 s = source.transform.lossyScale;
            float w = b.size.x * Mathf.Abs(s.x);
            float h = b.size.y * Mathf.Abs(s.y);
            if (w < 0.1f || h < 0.1f) continue;

            GameObject board = GameObject.CreatePrimitive(PrimitiveType.Quad);
            board.name = "HOLOGRAM - " + name;
            board.transform.SetParent(root.transform, true);
            board.transform.position = source.transform.TransformPoint(b.center)
                + source.transform.forward *
                (b.extents.z * Mathf.Abs(s.z) + 0.035f);
            board.transform.rotation = source.transform.rotation;
            board.transform.localScale = new Vector3(w * 0.97f, h * 0.97f, 1f);

            Collider col = board.GetComponent<Collider>();
            if (col != null) Object.DestroyImmediate(col);

            Material mat = new Material(shader);
            mat.name = "Cyan Hologram - " + name;
            Color cyan = new Color(0.05f, 0.85f, 1f, 0.24f);

            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", cyan);
            if (mat.HasProperty("_Color")) mat.SetColor("_Color", cyan);
            if (mat.HasProperty("_Surface")) mat.SetFloat("_Surface", 1f);
            if (mat.HasProperty("_Blend")) mat.SetFloat("_Blend", 0f);
            if (mat.HasProperty("_ZWrite")) mat.SetFloat("_ZWrite", 0f);
            if (mat.HasProperty("_SrcBlend"))
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            if (mat.HasProperty("_DstBlend"))
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            if (mat.HasProperty("_EmissionColor"))
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", new Color(0.02f, 0.3f, 0.4f, 1f));
            }
            mat.renderQueue = 3000;

            Renderer br = board.GetComponent<Renderer>();
            br.sharedMaterial = mat;
            br.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            br.receiveShadows = false;

            GameObject textObj = new GameObject("HOLOGRAM TEXT - " + name);
            textObj.transform.SetParent(board.transform, false);
            textObj.transform.localPosition = new Vector3(-0.45f, 0.43f, -0.02f);
            textObj.transform.localRotation = Quaternion.identity;
            textObj.transform.localScale = Vector3.one;

            TextMesh text = textObj.AddComponent<TextMesh>();
            text.text = GetInfo(name);
            text.anchor = TextAnchor.UpperLeft;
            text.alignment = TextAlignment.Left;
            text.fontSize = 48;
            text.characterSize = Mathf.Clamp(h / 7.5f, 0.015f, 0.12f);
            text.color = new Color(0.35f, 0.95f, 1f, 1f);
            text.richText = true;

            MeshRenderer tr = textObj.GetComponent<MeshRenderer>();
            tr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            tr.receiveShadows = false;

            built++;
        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        AssetDatabase.SaveAssets();

        Debug.Log("[SCN17 HOLO] Built " + built +
            " holographic information boards. Building geometry and HUD unchanged.");
    }

    static string GetInfo(string n)
    {
        if (n.Contains("AI DEVELOPMENT"))
            return "AI DEVELOPMENT\n> NEURAL SYSTEMS\n> AUTONOMY ANALYSIS\n> MODEL STATUS: ACTIVE\n> SECURITY: ENCRYPTED";
        if (n.Contains("DAMAGE HISTORY"))
            return "DAMAGE HISTORY\n> IMPACT LOG\n> STRUCTURAL DAMAGE\n> COMPONENT FAILURES\n> REVIEW INCIDENTS";
        if (n.Contains("FLEET GROWTH"))
            return "FLEET OVERVIEW\n> ACTIVE UNITS: --\n> RESERVE UNITS: --\n> DEPLOYMENT READINESS\n> INVENTORY STATUS";
        if (n.Contains("REPAIR RECORD"))
            return "MAINTENANCE LOG\n> REPAIR CASES\n> COMPONENT REPLACEMENT\n> TECHNICIAN NOTES\n> SERVICE VERIFICATION";
        if (n.Contains("REPAIR HISTORY"))
            return "REPAIR HISTORY\n> SERVICE TIMELINE\n> RECURRING FAULTS\n> PARTS REPLACED\n> NEXT INSPECTION";
        if (n.Contains("MISSION HISTORY"))
            return "MISSION HISTORY\n> OPERATION LOG\n> OBJECTIVES\n> OUTCOME REVIEW\n> AFTER-ACTION REPORT";
        if (n.Contains("MISSION ARCHIVE"))
            return "MISSION ARCHIVE\n> CLASSIFIED RECORDS\n> OPERATION TIMELINE\n> FIELD REPORTS\n> ACCESS: AUTHORIZED";
        if (n.Contains("EQUIPMENT TIMELINE"))
            return "EQUIPMENT TIMELINE\n> PROCUREMENT\n> DEPLOYMENT\n> UPGRADES\n> RETIREMENT RECORDS";
        if (n.Contains("OPERATIONAL EXPERIENCE"))
            return "OPERATIONAL EXPERIENCE\n> FIELD HOURS: --\n> MISSIONS COMPLETED: --\n> PERFORMANCE REVIEW\n> TRAINING HISTORY";
        if (n.Contains("COMMANDER DEVELOPMENT"))
            return "COMMANDER PROFILE\n> QUALIFICATIONS\n> TRAINING PROGRESS\n> LEADERSHIP RECORD\n> CERTIFICATION STATUS";
        if (n.Contains("RECOVERY STATUS"))
            return "RECOVERY STATUS\n> RECOVERY QUEUE\n> UNIT CONDITION\n> EXTRACTION LOG\n> CLEARANCE REQUIRED";
        if (n.Contains("DOCTRINE"))
            return "DOCTRINE DATABASE\n> STANDARD PROCEDURES\n> TACTICAL GUIDANCE\n> REVISION HISTORY\n> CURRENT EDITION";
        if (n.Contains("COMBAT HISTORY"))
            return "COMBAT HISTORY\n> ENGAGEMENT RECORDS\n> THREAT ASSESSMENTS\n> MISSION OUTCOMES\n> DEBRIEF ARCHIVE";
        if (n.Contains("TECHNOLOGY GROWTH"))
            return "TECHNOLOGY PROGRAM\n> RESEARCH PROJECTS\n> PROTOTYPE STATUS\n> SYSTEM UPGRADES\n> DEVELOPMENT MILESTONES";
        if (n.Contains("LOSS RECORD"))
            return "LOSS RECORDS\n> UNIT STATUS CHANGES\n> INCIDENT REPORTS\n> RECOVERY OPTIONS\n> REVIEW REQUIRED";
        if (n.Contains("VETERAN"))
            return "VETERAN DATABASE\n> SERVICE RECORD\n> DEPLOYMENT HISTORY\n> AWARDS & CITATIONS\n> HONOR ROLL";
        if (n.Contains("UNIT HISTORY"))
            return "UNIT HISTORY\n> ASSIGNMENT RECORD\n> MISSION LOG\n> MAINTENANCE HISTORY\n> PERFORMANCE PROFILE";
        if (n.Contains("UNIT RECORD"))
            return "UNIT: BULLDOG-024\n> STATUS: RECORD VIEW\n> ASSIGNMENT HISTORY\n> SERVICE & REPAIR LOG\n> ACCESS: AUTHORIZED";
        if (n.Contains("UNIT RECORDS"))
            return "UNIT RECORDS\n> SEARCH DATABASE\n> UNIT IDENTIFICATION\n> SERVICE HISTORY\n> RECORDS INDEX";
        if (n.Contains("SERVICE HISTORY"))
            return "SERVICE HISTORY\n> ASSIGNMENTS\n> MAINTENANCE\n> DEPLOYMENT RECORD\n> QUALIFICATION LOG";
        if (n.Contains("MILITARY DEVELOPMENT"))
            return "FORCE DEVELOPMENT\n> PERSONNEL TRAINING\n> UNIT READINESS\n> DOCTRINE UPDATES\n> PROGRAM MILESTONES";
        if (n.Contains("ARCHIVE CONTROL"))
            return "ARCHIVE CONTROL\n> INDEXED RECORDS\n> DATA INTEGRITY\n> ACCESS MONITORING\n> SYSTEM: STANDBY";
        if (n.Contains("VETERAN RANK"))
            return "VETERAN RANKS\n> SERVICE TIER\n> QUALIFICATIONS\n> AWARDS & CITATIONS\n> ELIGIBILITY REVIEW";
        return "FACILITY INFORMATION\n> SYSTEM STATUS\n> RECORDS DATABASE\n> ACCESS CONTROL\n> READY";
    }

    static GameObject FindScreen(string name)
    {
        foreach (GameObject item in Resources.FindObjectsOfTypeAll<GameObject>())
            if (item != null && item.scene.IsValid() &&
                item.scene.path == ScenePath && item.name == name)
                return item;
        return null;
    }
}
