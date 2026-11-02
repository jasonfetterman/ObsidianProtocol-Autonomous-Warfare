
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.Game.Units;

namespace ObsidianProtocol.Game.UI.Battlefield
{
    [DisallowMultipleComponent]
    public sealed class SensorCoverageOverlay : MonoBehaviour
    {
        [Header("Map World Bounds")]
        [SerializeField] private Vector2 worldMin = new Vector2(-500f, -500f);
        [SerializeField] private Vector2 worldMax = new Vector2(500f, 500f);

        [Header("Scale")]
        [Min(0.001f)]
        [SerializeField] private float metersPerUnityUnit = 1f;

        [Header("Refresh")]
        [Min(0.05f)]
        [SerializeField] private float refreshInterval = 0.5f;

        [Header("Appearance")]
        [SerializeField] private Color visualColor = new Color(0.15f, 0.85f, 0.35f, 0.18f);
        [SerializeField] private Color thermalColor = new Color(1f, 0.45f, 0.1f, 0.18f);
        [Range(12, 96)]
        [SerializeField] private int circleSegments = 48;

        private RectTransform mapRect;
        private RectTransform coverageLayer;
        private readonly Dictionary<string, Image> visualRings = new Dictionary<string, Image>();
        private readonly Dictionary<string, Image> thermalRings = new Dictionary<string, Image>();
        private float nextRefresh;

        private void Awake()
        {
            mapRect = transform as RectTransform;

            if (mapRect == null || worldMax.x <= worldMin.x ||
                worldMax.y <= worldMin.y)
            {
                Debug.LogError("[SENSOR COVERAGE] Invalid map RectTransform or bounds.", this);
                enabled = false;
                return;
            }

            GameObject layer = new GameObject(
                "[RUNTIME] SENSOR COVERAGE LAYER", typeof(RectTransform));

            coverageLayer = layer.GetComponent<RectTransform>();
            coverageLayer.SetParent(mapRect, false);
            coverageLayer.anchorMin = Vector2.zero;
            coverageLayer.anchorMax = Vector2.one;
            coverageLayer.offsetMin = Vector2.zero;
            coverageLayer.offsetMax = Vector2.zero;
            coverageLayer.SetAsFirstSibling();
        }

        private void Update()
        {
            if (Time.unscaledTime < nextRefresh)
                return;

            nextRefresh = Time.unscaledTime + Mathf.Max(0.05f, refreshInterval);
            RefreshCoverage();
        }

        private void RefreshCoverage()
        {
            UnitInstance[] units = FindObjectsByType<UnitInstance>(FindObjectsSortMode.None);
            HashSet<string> seenVisual = new HashSet<string>();
            HashSet<string> seenThermal = new HashSet<string>();
            int visualCount = 0;
            int thermalCount = 0;

            foreach (UnitInstance unit in units)
            {
                if (unit == null || !unit.gameObject.activeInHierarchy ||
                    unit.Definition == null || unit.Definition.Sensors == null)
                    continue;

                var entityId = unit.GetComponent<ObsidianProtocol.Game.Core.EntityId>();
                if (entityId == null || string.IsNullOrEmpty(entityId.Id))
                    continue;

                string id = entityId.Id;
                Vector3 position = unit.transform.position;

                if (position.x < worldMin.x || position.x > worldMax.x ||
                    position.z < worldMin.y || position.z > worldMax.y)
                {
                    RemoveRing(visualRings, id);
                    RemoveRing(thermalRings, id);
                    continue;
                }

                var sensors = unit.Definition.Sensors;

                UpdateRing(
                    visualRings, seenVisual, id, position,
                    sensors.visualRangeMeters, visualColor);

                UpdateRing(
                    thermalRings, seenThermal, id, position,
                    sensors.thermalRangeMeters, thermalColor);

                if (sensors.visualRangeMeters > 0f) visualCount++;
                if (sensors.thermalRangeMeters > 0f) thermalCount++;
            }

            RemoveStale(visualRings, seenVisual);
            RemoveStale(thermalRings, seenThermal);

            Debug.Log(
                "[SENSOR COVERAGE] Units scanned: " + units.Length +
                ", visual coverage rings: " + visualCount +
                ", thermal coverage rings: " + thermalCount, this);
        }

        private void UpdateRing(
            Dictionary<string, Image> rings,
            HashSet<string> seen,
            string id,
            Vector3 worldPosition,
            float rangeMeters,
            Color color)
        {
            if (rangeMeters <= 0f)
            {
                RemoveRing(rings, id);
                return;
            }

            seen.Add(id);

            if (!rings.TryGetValue(id, out Image image) || image == null)
                image = CreateRing(rings, id, color);

            image.color = color;

            Vector2 center = WorldToMap(worldPosition);
            float radiusX = MetersToMapWidth(rangeMeters);
            float radiusY = radiusX *
                (worldMax.x - worldMin.x) / (worldMax.y - worldMin.y);

            RectTransform rect = image.rectTransform;
            rect.anchorMin = center;
            rect.anchorMax = center;
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(
                radiusX * 2f * mapRect.rect.width,
                radiusY * 2f * mapRect.rect.height);
        }

        private Image CreateRing(
            Dictionary<string, Image> rings, string id, Color color)
        {
            GameObject go = new GameObject(
                "[RUNTIME] SENSOR RING " + id,
                typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));

            go.transform.SetParent(coverageLayer, false);

            Image image = go.GetComponent<Image>();
            image.color = color;
            image.raycastTarget = false;
            image.sprite = BuildCircleSprite();
            image.type = Image.Type.Simple;
            image.preserveAspect = false;
            rings[id] = image;
            return image;
        }

        private Sprite circleSprite;

        private Sprite BuildCircleSprite()
        {
            if (circleSprite != null)
                return circleSprite;

            int size = Mathf.Clamp(circleSegments * 4, 64, 256);
            Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.name = "Runtime Sensor Coverage Circle";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;

            float center = (size - 1) * 0.5f;
            float radius = size * 0.46f;

            for (int y = 0; y < size; y++)
                for (int x = 0; x < size; x++)
                {
                    float dx = (x - center) / radius;
                    float dy = (y - center) / radius;
                    float distance = Mathf.Sqrt(dx * dx + dy * dy);
                    float edge = Mathf.Clamp01((1f - distance) * size * 0.5f);
                    float fill = distance <= 1f ? 1f : 0f;

                    // Soft outer edge; transparent outside the circle.
                    texture.SetPixel(x, y, new Color(1f, 1f, 1f, fill * edge));
                }

            texture.Apply();
            circleSprite = Sprite.Create(
                texture, new Rect(0, 0, size, size),
                new Vector2(0.5f, 0.5f), 100f);
            return circleSprite;
        }

        private Vector2 WorldToMap(Vector3 worldPosition)
        {
            return new Vector2(
                Mathf.InverseLerp(worldMin.x, worldMax.x, worldPosition.x),
                Mathf.InverseLerp(worldMin.y, worldMax.y, worldPosition.z));
        }

        private float MetersToMapWidth(float meters)
        {
            return (meters / Mathf.Max(0.001f, metersPerUnityUnit)) /
                   (worldMax.x - worldMin.x);
        }

        private void RemoveRing(Dictionary<string, Image> rings, string id)
        {
            if (rings.TryGetValue(id, out Image image) && image != null)
                Destroy(image.gameObject);

            rings.Remove(id);
        }

        private void RemoveStale(
            Dictionary<string, Image> rings, HashSet<string> seen)
        {
            List<string> stale = new List<string>();

            foreach (var pair in rings)
                if (!seen.Contains(pair.Key))
                    stale.Add(pair.Key);

            foreach (string id in stale)
                RemoveRing(rings, id);
        }

        private void OnDestroy()
        {
            DestroyRings(visualRings);
            DestroyRings(thermalRings);

            if (coverageLayer != null)
                Destroy(coverageLayer.gameObject);

            if (circleSprite != null)
            {
                if (circleSprite.texture != null)
                    Destroy(circleSprite.texture);
                Destroy(circleSprite);
            }
        }

        private void DestroyRings(Dictionary<string, Image> rings)
        {
            foreach (Image image in rings.Values)
                if (image != null)
                    Destroy(image.gameObject);

            rings.Clear();
        }
    }
}

