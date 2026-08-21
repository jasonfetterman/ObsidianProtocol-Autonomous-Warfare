using UnityEngine;
using UnityEngine.UI;

public class ObsidianHUD : MonoBehaviour
{
    private Canvas canvas;
    private RectTransform root;

    private void Awake()
    {
        CreateHUD();
    }

    private void CreateHUD()
    {
        // Canvas
        GameObject canvasObject = new GameObject("ObsidianHUD_Canvas");
        canvasObject.transform.SetParent(transform, false);

        canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1536f, 1024f);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        canvasObject.AddComponent<GraphicRaycaster>();

        // Root
        GameObject rootObject = new GameObject("HUD_Root");
        rootObject.transform.SetParent(canvasObject.transform, false);

        root = rootObject.AddComponent<RectTransform>();

        root.anchorMin = Vector2.zero;
        root.anchorMax = Vector2.one;
        root.offsetMin = Vector2.zero;
        root.offsetMax = Vector2.zero;
    }
}