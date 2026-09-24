using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MiniRTS
{
    /// <summary>
    /// Runtime-built hand-drawn minimap with fog, faction markers, camera indicator,
    /// and click/drag camera focusing.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class MinimapUI : MonoBehaviour
    {
        private static readonly Color32 TerrainColor =
            new Color32(48, 76, 42, 255);
        private static readonly Color32 ObstacleColor =
            new Color32(48, 50, 52, 255);
        private static readonly Color32 UndiscoveredColor =
            new Color32(0, 0, 0, 255);
        private static readonly Color32 PlayerMarkerColor =
            new Color32(50, 125, 255, 255);
        private static readonly Color32 EnemyMarkerColor =
            new Color32(235, 55, 45, 255);
        private static readonly Vector2[] ViewportCorners =
        {
            new Vector2(0f, 0f),
            new Vector2(1f, 0f),
            new Vector2(1f, 1f),
            new Vector2(0f, 1f)
        };

        private WalkGrid grid;
        private FogOfWar fogOfWar;
        private Camera worldCamera;
        private Texture2D minimapTexture;
        private Color32[] pixels;
        private RectTransform frustumIndicator;
        private float nextTextureUpdate;

        public void Initialize(
            WalkGrid walkGrid,
            FogOfWar fog,
            Camera targetCamera,
            CameraController cameraController)
        {
            grid = walkGrid ??
                throw new ArgumentNullException(nameof(walkGrid));
            fogOfWar = fog ??
                throw new ArgumentNullException(nameof(fog));
            worldCamera = targetCamera ??
                throw new ArgumentNullException(nameof(targetCamera));
            if (cameraController == null)
            {
                throw new ArgumentNullException(nameof(cameraController));
            }

            BuildUi(cameraController);
            RefreshTexture();
            UpdateFrustumIndicator();
        }

        private void Update()
        {
            if (grid == null || fogOfWar == null)
            {
                return;
            }

            if (Time.unscaledTime >= nextTextureUpdate)
            {
                RefreshTexture();
            }

            UpdateFrustumIndicator();
        }

        private void OnDestroy()
        {
            if (minimapTexture != null)
            {
                Destroy(minimapTexture);
            }
        }

        private void BuildUi(CameraController cameraController)
        {
            minimapTexture = new Texture2D(
                grid.Width,
                grid.Height,
                TextureFormat.RGBA32,
                false,
                true)
            {
                name = "MinimapTexture",
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
            pixels = new Color32[grid.Width * grid.Height];

            GameObject canvasObject = new GameObject(
                "MinimapCanvas",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster));
            canvasObject.transform.SetParent(transform, false);
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 15;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            GameObject frameObject = new GameObject(
                "MinimapFrame",
                typeof(RectTransform),
                typeof(Image));
            frameObject.transform.SetParent(canvasObject.transform, false);
            RectTransform frame = frameObject.GetComponent<RectTransform>();
            frame.anchorMin = Vector2.zero;
            frame.anchorMax = Vector2.zero;
            frame.pivot = Vector2.zero;
            frame.anchoredPosition = new Vector2(18f, 18f);
            float frameSize = BalanceConfig.MinimapSize + 12f;
            frame.sizeDelta = new Vector2(frameSize, frameSize);
            Image frameImage = frameObject.GetComponent<Image>();
            frameImage.color = new Color(0.025f, 0.04f, 0.065f, 0.96f);
            frameImage.raycastTarget = false;

            GameObject mapObject = new GameObject(
                "MinimapMap",
                typeof(RectTransform),
                typeof(RawImage));
            mapObject.transform.SetParent(frameObject.transform, false);
            RectTransform mapRect = mapObject.GetComponent<RectTransform>();
            mapRect.anchorMin = new Vector2(0.5f, 0.5f);
            mapRect.anchorMax = new Vector2(0.5f, 0.5f);
            mapRect.pivot = new Vector2(0.5f, 0.5f);
            mapRect.anchoredPosition = Vector2.zero;
            mapRect.sizeDelta = new Vector2(
                BalanceConfig.MinimapSize,
                BalanceConfig.MinimapSize);

            RawImage rawImage = mapObject.GetComponent<RawImage>();
            rawImage.texture = minimapTexture;
            rawImage.color = Color.white;
            rawImage.raycastTarget = true;

            MinimapPointerHandler pointerHandler =
                mapObject.AddComponent<MinimapPointerHandler>();
            pointerHandler.Initialize(mapRect, grid, cameraController);

            GameObject indicatorObject = new GameObject(
                "CameraFrustum",
                typeof(RectTransform));
            indicatorObject.transform.SetParent(mapObject.transform, false);
            frustumIndicator =
                indicatorObject.GetComponent<RectTransform>();
            frustumIndicator.anchorMin = new Vector2(0.4f, 0.4f);
            frustumIndicator.anchorMax = new Vector2(0.6f, 0.6f);
            frustumIndicator.offsetMin = Vector2.zero;
            frustumIndicator.offsetMax = Vector2.zero;
            CreateIndicatorBorder("Top", frustumIndicator, BorderSide.Top);
            CreateIndicatorBorder("Bottom", frustumIndicator, BorderSide.Bottom);
            CreateIndicatorBorder("Left", frustumIndicator, BorderSide.Left);
            CreateIndicatorBorder("Right", frustumIndicator, BorderSide.Right);
        }

        private void RefreshTexture()
        {
            FogGrid visibility = fogOfWar.VisibilityGrid;
            for (int y = 0; y < grid.Height; y++)
            {
                for (int x = 0; x < grid.Width; x++)
                {
                    Color32 terrain = grid.IsWalkable(x, y)
                        ? TerrainColor
                        : ObstacleColor;
                    switch (visibility.GetState(x, y))
                    {
                        case FogState.Visible:
                            break;
                        case FogState.Discovered:
                            terrain = Darken(terrain, 0.42f);
                            break;
                        default:
                            terrain = UndiscoveredColor;
                            break;
                    }

                    pixels[y * grid.Width + x] = terrain;
                }
            }

            DrawUnitMarkers(visibility);
            DrawBuildingMarkers();
            minimapTexture.SetPixels32(pixels);
            minimapTexture.Apply(false, false);
            nextTextureUpdate =
                Time.unscaledTime + BalanceConfig.MinimapUpdateInterval;
        }

        private void DrawUnitMarkers(FogGrid visibility)
        {
            System.Collections.Generic.IReadOnlyList<Unit> units =
                Unit.ActiveUnits;
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];
                if (unit == null || !unit.IsAlive)
                {
                    continue;
                }

                bool player =
                    unit.OwnerId == BalanceConfig.PlayerOwnerId;
                if (!player &&
                    !visibility.IsVisible(unit.CombatTargetPosition))
                {
                    continue;
                }

                DrawMarker(
                    grid.WorldToCell(unit.transform.position),
                    1,
                    player ? PlayerMarkerColor : EnemyMarkerColor);
            }
        }

        private void DrawBuildingMarkers()
        {
            System.Collections.Generic.IReadOnlyList<Building> buildings =
                Building.ActiveBuildings;
            for (int i = 0; i < buildings.Count; i++)
            {
                Building building = buildings[i];
                if (building == null || !building.IsAlive)
                {
                    continue;
                }

                bool player =
                    building.OwnerId == BalanceConfig.PlayerOwnerId;
                if (!player &&
                    !FogOfWar.IsVisibleToPlayer(building))
                {
                    continue;
                }

                DrawMarker(
                    grid.WorldToCell(building.transform.position),
                    2,
                    player ? PlayerMarkerColor : EnemyMarkerColor);
            }
        }

        private void DrawMarker(
            Vector2Int center,
            int radius,
            Color32 color)
        {
            for (int x = center.x - radius; x <= center.x + radius; x++)
            {
                for (int y = center.y - radius; y <= center.y + radius; y++)
                {
                    if (x >= 0 && x < grid.Width &&
                        y >= 0 && y < grid.Height)
                    {
                        pixels[y * grid.Width + x] = color;
                    }
                }
            }
        }

        private void UpdateFrustumIndicator()
        {
            if (frustumIndicator == null || worldCamera == null)
            {
                return;
            }

            Vector2 minimum = Vector2.one;
            Vector2 maximum = Vector2.zero;
            Plane ground = new Plane(Vector3.up, Vector3.zero);
            for (int i = 0; i < ViewportCorners.Length; i++)
            {
                Ray ray = worldCamera.ViewportPointToRay(
                    ViewportCorners[i]);
                if (!ground.Raycast(ray, out float distance))
                {
                    frustumIndicator.gameObject.SetActive(false);
                    return;
                }

                Vector3 point = ray.GetPoint(distance);
                Vector2 normalized = new Vector2(
                    (point.x - grid.Origin.x) /
                    (grid.Width * grid.CellSize),
                    (point.z - grid.Origin.z) /
                    (grid.Height * grid.CellSize));
                minimum = Vector2.Min(minimum, normalized);
                maximum = Vector2.Max(maximum, normalized);
            }

            minimum.x = Mathf.Clamp01(minimum.x);
            minimum.y = Mathf.Clamp01(minimum.y);
            maximum.x = Mathf.Clamp01(maximum.x);
            maximum.y = Mathf.Clamp01(maximum.y);
            frustumIndicator.gameObject.SetActive(true);
            frustumIndicator.anchorMin = minimum;
            frustumIndicator.anchorMax = maximum;
            frustumIndicator.offsetMin = Vector2.zero;
            frustumIndicator.offsetMax = Vector2.zero;
        }

        private static Color32 Darken(Color32 color, float multiplier)
        {
            return new Color32(
                (byte)Mathf.RoundToInt(color.r * multiplier),
                (byte)Mathf.RoundToInt(color.g * multiplier),
                (byte)Mathf.RoundToInt(color.b * multiplier),
                color.a);
        }

        private static void CreateIndicatorBorder(
            string objectName,
            RectTransform parent,
            BorderSide side)
        {
            GameObject borderObject = new GameObject(
                objectName,
                typeof(RectTransform),
                typeof(Image));
            borderObject.transform.SetParent(parent, false);
            RectTransform border =
                borderObject.GetComponent<RectTransform>();
            const float thickness = 2f;

            if (side == BorderSide.Top || side == BorderSide.Bottom)
            {
                float verticalAnchor =
                    side == BorderSide.Top ? 1f : 0f;
                border.anchorMin = new Vector2(0f, verticalAnchor);
                border.anchorMax = new Vector2(1f, verticalAnchor);
                border.pivot = new Vector2(0.5f, verticalAnchor);
                border.sizeDelta = new Vector2(0f, thickness);
                border.anchoredPosition = Vector2.zero;
            }
            else
            {
                float horizontalAnchor =
                    side == BorderSide.Right ? 1f : 0f;
                border.anchorMin = new Vector2(horizontalAnchor, 0f);
                border.anchorMax = new Vector2(horizontalAnchor, 1f);
                border.pivot = new Vector2(horizontalAnchor, 0.5f);
                border.sizeDelta = new Vector2(thickness, 0f);
                border.anchoredPosition = Vector2.zero;
            }

            Image image = borderObject.GetComponent<Image>();
            image.color = new Color(0.92f, 0.95f, 1f, 0.95f);
            image.raycastTarget = false;
        }

        private enum BorderSide
        {
            Top,
            Bottom,
            Left,
            Right
        }
    }

    public sealed class MinimapPointerHandler :
        MonoBehaviour,
        IPointerClickHandler,
        IDragHandler
    {
        private RectTransform mapRect;
        private WalkGrid grid;
        private CameraController cameraController;

        public void Initialize(
            RectTransform targetRect,
            WalkGrid walkGrid,
            CameraController targetCameraController)
        {
            mapRect = targetRect;
            grid = walkGrid;
            cameraController = targetCameraController;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                FocusCamera(eventData);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                FocusCamera(eventData);
            }
        }

        private void FocusCamera(PointerEventData eventData)
        {
            if (mapRect == null ||
                grid == null ||
                cameraController == null ||
                !RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    mapRect,
                    eventData.position,
                    eventData.pressEventCamera,
                    out Vector2 localPoint))
            {
                return;
            }

            Rect rect = mapRect.rect;
            float normalizedX = Mathf.Clamp01(
                (localPoint.x - rect.xMin) / rect.width);
            float normalizedY = Mathf.Clamp01(
                (localPoint.y - rect.yMin) / rect.height);
            cameraController.FocusOn(new Vector3(
                grid.Origin.x +
                normalizedX * grid.Width * grid.CellSize,
                0f,
                grid.Origin.z +
                normalizedY * grid.Height * grid.CellSize));
        }
    }
}
