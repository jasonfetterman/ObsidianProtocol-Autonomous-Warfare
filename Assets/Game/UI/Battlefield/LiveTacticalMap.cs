using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.Game.PersistentMilitary;
using ObsidianProtocol.Game.Core;

namespace ObsidianProtocol.Game.UI.Battlefield
{
    public sealed class LiveTacticalMap : MonoBehaviour
    {
        [SerializeField] private Vector2 worldMin = new Vector2(-500f, -500f);
        [SerializeField] private Vector2 worldMax = new Vector2(500f, 500f);
        [SerializeField] private float refreshInterval = 0.5f;
        [SerializeField] private float markerSize = 9f;
        [SerializeField] private Color unitColor = new Color(0.12f, 0.86f, 0.88f, 1f);

        private readonly Dictionary<string, Image> markers =
            new Dictionary<string, Image>();

        private RectTransform mapRect;
        private float nextRefresh;
        private bool hasLoggedDiagnostics;

        private void Awake()
        {
            mapRect = transform as RectTransform;

            if (mapRect == null)
            {
                Debug.LogError(
                    "[LIVE TACTICAL MAP] A RectTransform is required.", this);
                enabled = false;
                return;
            }

            Debug.Log(
                "[LIVE TACTICAL MAP] Ready. World bounds X/Z: " +
                worldMin.x + ".." + worldMax.x + " / " +
                worldMin.y + ".." + worldMax.y, this);
        }

        private void Update()
        {
            if (Time.unscaledTime < nextRefresh)
                return;

            nextRefresh = Time.unscaledTime +
                Mathf.Max(0.05f, refreshInterval);

            RefreshMarkers();
        }

        private void RefreshMarkers()
        {
            MilitaryUnitIdentity[] units =
                FindObjectsByType<MilitaryUnitIdentity>(
                    FindObjectsSortMode.None);

            HashSet<string> seen = new HashSet<string>();

            int activeUnits = 0;
            int missingEntityIds = 0;
            int outsideBounds = 0;
            int shownMarkers = 0;

            if (worldMax.x <= worldMin.x || worldMax.y <= worldMin.y)
            {
                Debug.LogError(
                    "[LIVE TACTICAL MAP] Invalid world bounds.", this);
                enabled = false;
                return;
            }

            foreach (MilitaryUnitIdentity unit in units)
            {
                if (unit == null || !unit.gameObject.activeInHierarchy)
                    continue;

                activeUnits++;

                ObsidianProtocol.Game.Core.EntityId entityId = unit.GetComponent<ObsidianProtocol.Game.Core.EntityId>();

                // Do not manufacture or modify persistent unit identity here.
                if (entityId == null || string.IsNullOrEmpty(entityId.Id))
                {
                    missingEntityIds++;
                    continue;
                }

                string id = entityId.Id;
                seen.Add(id);

                if (!markers.TryGetValue(id, out Image marker) || marker == null)
                {
                    GameObject go = new GameObject(
                        "[RUNTIME MAP MARKER] " + unit.name,
                        typeof(RectTransform),
                        typeof(CanvasRenderer),
                        typeof(Image));

                    go.transform.SetParent(mapRect, false);

                    marker = go.GetComponent<Image>();
                    marker.raycastTarget = false;
                    markers[id] = marker;
                }

                Vector3 world = unit.transform.position;

                bool inBounds =
                    world.x >= worldMin.x && world.x <= worldMax.x &&
                    world.z >= worldMin.y && world.z <= worldMax.y;

                marker.enabled = inBounds;
                marker.color = unitColor;

                if (!inBounds)
                {
                    outsideBounds++;
                    continue;
                }

                float x = Mathf.InverseLerp(
                    worldMin.x, worldMax.x, world.x);
                float y = Mathf.InverseLerp(
                    worldMin.y, worldMax.y, world.z);

                RectTransform markerRect = marker.rectTransform;
                markerRect.anchorMin = new Vector2(x, y);
                markerRect.anchorMax = new Vector2(x, y);
                markerRect.anchoredPosition = Vector2.zero;
                markerRect.sizeDelta = Vector2.one * markerSize;

                shownMarkers++;
            }

            List<string> stale = new List<string>();

            foreach (KeyValuePair<string, Image> pair in markers)
            {
                if (!seen.Contains(pair.Key))
                {
                    if (pair.Value != null)
                        Destroy(pair.Value.gameObject);

                    stale.Add(pair.Key);
                }
            }

            foreach (string id in stale)
                markers.Remove(id);

            // Report the first refresh and any later change in results.
            string summary =
                "active=" + activeUnits +
                ", visible markers=" + shownMarkers +
                ", missing EntityId=" + missingEntityIds +
                ", outside map bounds=" + outsideBounds;

            if (!hasLoggedDiagnostics)
            {
                Debug.Log("[LIVE TACTICAL MAP] " + summary, this);
                hasLoggedDiagnostics = true;
            }
        }

        private void OnDestroy()
        {
            foreach (Image marker in markers.Values)
            {
                if (marker != null)
                    Destroy(marker.gameObject);
            }

            markers.Clear();
        }
    }
}

