using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ObsidianProtocol.Game.Command;

namespace ObsidianProtocol.UI.HUD.Minimap
{
    public class MAP003_FriendlyUnits : MonoBehaviour
    {
        public const string ID = "MAP-003";

        [Header("Blackspire Expanse Bounds")]
        [SerializeField] private float minX = -24f;
        [SerializeField] private float maxX = 1000f;
        [SerializeField] private float minZ = 0f;
        [SerializeField] private float maxZ = 1024f;

        [Header("Marker Settings")]
        [SerializeField] private float refreshInterval = 0.5f;
        [SerializeField] private float markerSize = 8f;
        [SerializeField] private Color friendlyColor =
            new Color(0.1f, 0.85f, 1f, 1f);
        [SerializeField] private Color selectedColor =
            new Color(0.2f, 1f, 0.35f, 1f);

        private RectTransform markerLayer;
        private readonly Dictionary<EntityId, Image> markers =
            new Dictionary<EntityId, Image>();
        private readonly Dictionary<EntityId, SelectableUnit> units =
            new Dictionary<EntityId, SelectableUnit>();

        private readonly MinimapSystem minimapSystem =
            new MinimapSystem();

        private float refreshTimer;

        private void Awake()
        {
            Initialize();
        }

        public void Initialize()
        {
            markerLayer = GetComponent<RectTransform>();
            RefreshUnits();
            Debug.Log("[Minimap] MAP-003 friendly unit tracking initialized.");
        }

        private void Update()
        {
            refreshTimer -= Time.deltaTime;

            if (refreshTimer <= 0f)
            {
                RefreshUnits();
                refreshTimer = refreshInterval;
            }

            UpdateMarkers();
        }

        private void RefreshUnits()
        {
            SelectableUnit[] foundUnits =
                FindObjectsOfType<SelectableUnit>();

            HashSet<EntityId> activeIds = new HashSet<EntityId>();

            foreach (SelectableUnit unit in foundUnits)
            {
                if (unit == null)
                    continue;

                EntityId id = unit.GetEntityId();
                activeIds.Add(id);

                if (!markers.ContainsKey(id))
                {
                    string markerId = "friendly_" + id;

                    MinimapMarker marker = new MinimapMarker(
                        markerId,
                        MinimapMarkerType.FriendlyUnit,
                        unit.transform.position.x,
                        unit.transform.position.z
                    );

                    minimapSystem.Register(marker);

                    GameObject markerObject = new GameObject(
                        "FRIENDLY_UNIT_" + id,
                        typeof(RectTransform),
                        typeof(CanvasRenderer),
                        typeof(Image)
                    );

                    markerObject.transform.SetParent(markerLayer, false);

                    RectTransform markerRect =
                        markerObject.GetComponent<RectTransform>();

                    markerRect.sizeDelta =
                        new Vector2(markerSize, markerSize);

                    Image image = markerObject.GetComponent<Image>();
                    image.sprite = MAPMinimapMarkerUtility.DotSprite;
                    image.raycastTarget = false;
                    image.color = friendlyColor;

                    markers.Add(id, image);
                    units.Add(id, unit);
                }
                else
                {
                    units[id] = unit;
                }
            }

            List<EntityId> removedIds = new List<EntityId>();

            foreach (EntityId id in markers.Keys)
            {
                if (!activeIds.Contains(id))
                    removedIds.Add(id);
            }

            foreach (EntityId id in removedIds)
            {
                Destroy(markers[id].gameObject);
                markers.Remove(id);
                units.Remove(id);
                minimapSystem.Remove("friendly_" + id);
            }
        }

        private void UpdateMarkers()
        {
            if (markerLayer == null)
                return;

            foreach (KeyValuePair<EntityId, SelectableUnit> entry in units)
            {
                SelectableUnit unit = entry.Value;
                Image image = markers[entry.Key];

                if (unit == null || image == null)
                    continue;

                Vector3 position = unit.transform.position;

                minimapSystem.SetMarkerPosition(
                    "friendly_" + entry.Key,
                    position.x,
                    position.z
                );

                bool insideMap =
                    position.x >= minX && position.x <= maxX &&
                    position.z >= minZ && position.z <= maxZ;

                image.enabled = insideMap;

                if (!insideMap)
                    continue;

                image.rectTransform.anchoredPosition =
                    MAPMinimapMarkerUtility.WorldToAnchored(
                        position,
                        markerLayer,
                        minX, maxX, minZ, maxZ
                    );

                image.color = unit.IsSelected
                    ? selectedColor
                    : friendlyColor;

                image.rectTransform.sizeDelta = unit.IsSelected
                    ? new Vector2(markerSize * 1.6f, markerSize * 1.6f)
                    : new Vector2(markerSize, markerSize);
            }
        }
    }

    internal static class MAPMinimapMarkerUtility
    {
        private static Sprite dotSprite;

        public static Sprite DotSprite
        {
            get
            {
                if (dotSprite != null)
                    return dotSprite;

                const int resolution = 32;
                Texture2D texture = new Texture2D(
                    resolution, resolution, TextureFormat.RGBA32, false
                );

                Color32[] pixels = new Color32[resolution * resolution];

                for (int y = 0; y < resolution; y++)
                {
                    for (int x = 0; x < resolution; x++)
                    {
                        float dx = x - (resolution - 1) * 0.5f;
                        float dy = y - (resolution - 1) * 0.5f;
                        float distance = Mathf.Sqrt(dx * dx + dy * dy);

                        byte alpha = distance <= resolution * 0.43f
                            ? (byte)255
                            : (byte)0;

                        pixels[y * resolution + x] =
                            new Color32(255, 255, 255, alpha);
                    }
                }

                texture.SetPixels32(pixels);
                texture.Apply();

                dotSprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, resolution, resolution),
                    new Vector2(0.5f, 0.5f)
                );

                return dotSprite;
            }
        }

        public static Vector2 WorldToAnchored(
            Vector3 worldPosition,
            RectTransform rect,
            float minX,
            float maxX,
            float minZ,
            float maxZ)
        {
            float x = Mathf.InverseLerp(minX, maxX, worldPosition.x);
            float y = Mathf.InverseLerp(minZ, maxZ, worldPosition.z);

            return new Vector2(
                (x - 0.5f) * rect.rect.width,
                (y - 0.5f) * rect.rect.height
            );
        }
    }
}




