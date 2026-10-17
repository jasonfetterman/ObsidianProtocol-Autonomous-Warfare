using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public static class MilitaryStoreSceneFixer
{
    private const string TARGET_SCENE =
        "Assets/Scenes/SCN-04 GARAGE/GARAGE.unity";

    [MenuItem("Obsidian Protocol/Military Store/Fix Store View", false, 100)]
    public static void FixStoreView()
    {
        Fix();
    }

    [MenuItem("Obsidian Protocol/Military Store/Fix Store View", true)]
    private static bool ValidateFixStoreView()
    {
        return true;
    }

    [MenuItem("Obsidian Protocol/Military Store/Select Store Scene", false, 101)]
    public static void SelectStoreScene()
    {
        if (!System.IO.File.Exists(TARGET_SCENE))
        {
            Debug.LogError(
                "[Military Store] Scene not found: " + TARGET_SCENE
            );
            return;
        }

        EditorSceneManager.OpenScene(
            TARGET_SCENE,
            OpenSceneMode.Single
        );
    }

    private static void Fix()
    {
        if (!System.IO.File.Exists(TARGET_SCENE))
        {
            Debug.LogError(
                "[Military Store] Could not find scene:\n" +
                TARGET_SCENE
            );
            return;
        }

        Scene scene = SceneManager.GetActiveScene();

        if (scene.path != TARGET_SCENE)
        {
            scene = EditorSceneManager.OpenScene(
                TARGET_SCENE,
                OpenSceneMode.Single
            );
        }

        Debug.Log(
            "[Military Store] Fixing existing GARAGE scene..."
        );

        Camera cam = FindOrCreateCamera();

        Bounds bounds;
        bool foundBounds = CalculateStoreBounds(out bounds);

        if (foundBounds)
        {
            Vector3 center = bounds.center;

            float width = bounds.size.x;
            float height = bounds.size.y;

            float largest =
                Mathf.Max(width, height);

            if (largest < 1f)
                largest = 1f;

            /*
             * This scene is UI-heavy, so use an orthographic
             * camera rather than trying to perspective-frame
             * the entire store.
             */

            cam.orthographic = true;

            cam.orthographicSize =
                Mathf.Clamp(
                    height * 0.62f,
                    3.5f,
                    12f
                );

            cam.transform.position =
                new Vector3(
                    center.x,
                    center.y,
                    -20f
                );

            cam.transform.rotation =
                Quaternion.identity;

            cam.nearClipPlane = 0.01f;
            cam.farClipPlane = 1000f;

            Debug.Log(
                "[Military Store] Camera centered at " +
                center +
                " | Ortho Size: " +
                cam.orthographicSize
            );
        }
        else
        {
            cam.orthographic = true;
            cam.orthographicSize = 5f;
            cam.transform.position =
                new Vector3(0f, 0f, -20f);
            cam.transform.rotation =
                Quaternion.identity;
        }

        cam.clearFlags =
            CameraClearFlags.SolidColor;

        cam.backgroundColor =
            new Color(
                0.012f,
                0.018f,
                0.025f,
                1f
            );

        cam.tag = "MainCamera";

        FixCanvases();
        FixText();

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene);

        Debug.Log(
            "[Military Store] STORE VIEW FIX COMPLETE."
        );
    }

    private static Camera FindOrCreateCamera()
    {
        Camera[] cameras =
            Object.FindObjectsByType<Camera>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        Camera main = null;

        foreach (Camera c in cameras)
        {
            if (c.CompareTag("MainCamera"))
            {
                main = c;
                break;
            }
        }

        if (main == null && cameras.Length > 0)
            main = cameras[0];

        if (main == null)
        {
            GameObject go =
                new GameObject(
                    "MILITARY STORE CAMERA"
                );

            main =
                go.AddComponent<Camera>();
        }

        main.enabled = true;

        return main;
    }

    private static bool CalculateStoreBounds(
        out Bounds bounds
    )
    {
        Renderer[] renderers =
            Object.FindObjectsByType<Renderer>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        bool initialized = false;

        bounds = new Bounds();

        foreach (Renderer r in renderers)
        {
            if (r == null)
                continue;

            /*
             * Ignore the camera and any hidden editor-only
             * objects. UI is handled separately.
             */

            if (!r.gameObject.activeInHierarchy)
                continue;

            if (!initialized)
            {
                bounds = r.bounds;
                initialized = true;
            }
            else
            {
                bounds.Encapsulate(r.bounds);
            }
        }

        return initialized;
    }

    private static void FixCanvases()
    {
        Canvas[] canvases =
            Object.FindObjectsByType<Canvas>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        foreach (Canvas canvas in canvases)
        {
            if (canvas == null)
                continue;

            string name =
                canvas.gameObject.name.ToLower();

            /*
             * Store HUD/UI canvases should remain screen-space.
             */

            if (
                name.Contains("hud") ||
                name.Contains("ui") ||
                name.Contains("store") ||
                name.Contains("menu")
            )
            {
                canvas.renderMode =
                    RenderMode.ScreenSpaceOverlay;

                CanvasScaler scaler =
                    canvas.GetComponent<CanvasScaler>();

                if (scaler == null)
                {
                    scaler =
                        canvas.gameObject.AddComponent<
                            CanvasScaler
                        >();
                }

                scaler.uiScaleMode =
                    CanvasScaler.ScaleMode
                        .ScaleWithScreenSize;

                scaler.referenceResolution =
                    new Vector2(
                        1920f,
                        1080f
                    );

                scaler.screenMatchMode =
                    CanvasScaler.ScreenMatchMode
                        .MatchWidthOrHeight;

                scaler.matchWidthOrHeight =
                    0.5f;
            }
        }
    }

    private static void FixText()
    {
        Text[] texts =
            Object.FindObjectsByType<Text>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        foreach (Text text in texts)
        {
            if (text == null)
                continue;

            string name =
                text.gameObject.name.ToLower();

            int size = 11;

            if (
                name.Contains("title") ||
                name.Contains("header") ||
                name.Contains("section")
            )
            {
                size = 16;
            }
            else if (
                name.Contains("small") ||
                name.Contains("caption") ||
                name.Contains("status") ||
                name.Contains("description")
            )
            {
                size = 9;
            }

            text.fontSize = size;
            text.resizeTextForBestFit = false;
            text.horizontalOverflow =
                HorizontalWrapMode.Wrap;
            text.verticalOverflow =
                VerticalWrapMode.Truncate;
        }
    }
}
