using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class BuildMainMenuScene : EditorWindow
{
    [MenuItem("OPAW/Build AAA Biomech Main Menu")]
    public static void BuildScene()
    {
        string scenePath = "Assets/Scenes/Main_Menu/Main_Menu.unity";
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // CAMERA
        GameObject camGO = new GameObject("Main Camera");
        Camera cam = camGO.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black;
        camGO.transform.position = new Vector3(0, 0, -10);

        // EVENT SYSTEM
        GameObject esGO = new GameObject("EventSystem");
        esGO.AddComponent<EventSystem>();
        esGO.AddComponent<StandaloneInputModule>();

        // CANVAS
        GameObject canvasGO = new GameObject("Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        canvasGO.AddComponent<GraphicRaycaster>();

        // BACKGROUND (HOLOGRAPHIC GRID)
        GameObject bgGO = new GameObject("MainMenu_Background");
        bgGO.transform.SetParent(canvasGO.transform, false);

        RectTransform bgRT = bgGO.AddComponent<RectTransform>();
        bgRT.anchorMin = Vector2.zero;
        bgRT.anchorMax = Vector2.one;
        bgRT.offsetMin = Vector2.zero;
        bgRT.offsetMax = Vector2.zero;

        Image bgImg = bgGO.AddComponent<Image>();
        bgImg.color = new Color(0.02f, 0.06f, 0.12f, 1f);

        // GRID OVERLAY
        GameObject gridGO = new GameObject("Grid_Overlay");
        gridGO.transform.SetParent(bgGO.transform, false);

        RectTransform gridRT = gridGO.AddComponent<RectTransform>();
        gridRT.anchorMin = Vector2.zero;
        gridRT.anchorMax = Vector2.one;
        gridRT.offsetMin = Vector2.zero;
        gridRT.offsetMax = Vector2.zero;

        Image gridImg = gridGO.AddComponent<Image>();
        gridImg.color = new Color(0.1f, 0.3f, 0.6f, 0.15f);

        // HUD ROOT
        GameObject hudGO = new GameObject("MainMenu_HUD");
        hudGO.transform.SetParent(canvasGO.transform, false);

        RectTransform hudRT = hudGO.AddComponent<RectTransform>();
        hudRT.anchorMin = new Vector2(0.5f, 0.5f);
        hudRT.anchorMax = new Vector2(0.5f, 0.5f);
        hudRT.pivot     = new Vector2(0.5f, 0.5f);
        hudRT.sizeDelta = new Vector2(900, 700);
        hudRT.anchoredPosition = Vector2.zero;

        // LOGO
        GameObject logoGO = new GameObject("Logo");
        logoGO.transform.SetParent(hudGO.transform, false);

        RectTransform logoRT = logoGO.AddComponent<RectTransform>();
        logoRT.anchorMin = new Vector2(0.5f, 1f);
        logoRT.anchorMax = new Vector2(0.5f, 1f);
        logoRT.pivot     = new Vector2(0.5f, 1f);
        logoRT.sizeDelta = new Vector2(600, 120);
        logoRT.anchoredPosition = new Vector2(0, -40);

        Image logoImg = logoGO.AddComponent<Image>();
        logoImg.color = new Color(0.4f, 0.8f, 1f, 0.35f);

        // PROFILE
        GameObject profileGO = new GameObject("Profile");
        profileGO.transform.SetParent(hudGO.transform, false);

        RectTransform profileRT = profileGO.AddComponent<RectTransform>();
        profileRT.anchorMin = new Vector2(1f, 1f);
        profileRT.anchorMax = new Vector2(1f, 1f);
        profileRT.pivot     = new Vector2(1f, 1f);
        profileRT.sizeDelta = new Vector2(260, 100);
        profileRT.anchoredPosition = new Vector2(-40, -40);

        Image profileImg = profileGO.AddComponent<Image>();
        profileImg.color = new Color(0.15f, 0.4f, 0.7f, 0.4f);

        // CONNECTION STATUS
        GameObject connGO = new GameObject("ConnectionStatus");
        connGO.transform.SetParent(hudGO.transform, false);

        RectTransform connRT = connGO.AddComponent<RectTransform>();
        connRT.anchorMin = new Vector2(0f, 1f);
        connRT.anchorMax = new Vector2(0f, 1f);
        connRT.pivot     = new Vector2(0f, 1f);
        connRT.sizeDelta = new Vector2(260, 60);
        connRT.anchoredPosition = new Vector2(40, -40);

        Image connImg = connGO.AddComponent<Image>();
        connImg.color = new Color(0.1f, 0.35f, 0.6f, 0.4f);

        // MAIN BUTTONS ROOT
        GameObject buttonsRoot = new GameObject("MainButtons");
        buttonsRoot.transform.SetParent(hudGO.transform, false);

        RectTransform buttonsRT = buttonsRoot.AddComponent<RectTransform>();
        buttonsRT.anchorMin = new Vector2(0.5f, 0.5f);
        buttonsRT.anchorMax = new Vector2(0.5f, 0.5f);
        buttonsRT.pivot     = new Vector2(0.5f, 0.5f);
        buttonsRT.sizeDelta = new Vector2(520, 600);
        buttonsRT.anchoredPosition = new Vector2(0, -40);

        Font arial = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

        void MakeButton(string name, float yOffset)
        {
            GameObject btnGO = new GameObject(name);
            btnGO.transform.SetParent(buttonsRoot.transform, false);

            RectTransform rt = btnGO.AddComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot     = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(420, 64);
            rt.anchoredPosition = new Vector2(0, yOffset);

            btnGO.AddComponent<CanvasRenderer>();

            Image img = btnGO.AddComponent<Image>();
            img.color = new Color(0.12f, 0.45f, 0.8f, 0.7f);

            Button button = btnGO.AddComponent<Button>();

            GameObject textGO = new GameObject("Text");
            textGO.transform.SetParent(btnGO.transform, false);

            RectTransform trt = textGO.AddComponent<RectTransform>();
            trt.anchorMin = Vector2.zero;
            trt.anchorMax = Vector2.one;
            trt.offsetMin = Vector2.zero;
            trt.offsetMax = Vector2.zero;

            textGO.AddComponent<CanvasRenderer>();

            Text txt = textGO.AddComponent<Text>();
            txt.text = name.Replace("_Button", "").Replace("_", " ");
            txt.font = arial;
            txt.alignment = TextAnchor.MiddleCenter;
            txt.color = new Color(0.9f, 0.95f, 1f, 1f);
            txt.fontSize = 26;
        }

        float[] offsets = { 220, 150, 80, 10, -60, -130, -200, -270, -340 };
        string[] names = {
            "Continue_Button",
            "Campaign_Button",
            "Multiplayer_Button",
            "Garage_Button",
            "Store_Button",
            "VR_Operator_Button",
            "Settings_Button",
            "Credits_Button",
            "Exit_Button"
        };

        for (int i = 0; i < names.Length; i++)
            MakeButton(names[i], offsets[i]);

        EditorSceneManager.SaveScene(scene, scenePath);
        Debug.Log("AAA biomech Main_Menu scene built.");
    }
}
