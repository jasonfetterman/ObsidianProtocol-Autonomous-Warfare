using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ObsidianProtocol.Game.Deployment
{
    public sealed class BattlefieldMinimapUnit : MonoBehaviour
    {
        [SerializeField] private bool isFriendly = true;
        public bool IsFriendly => isFriendly;

        public void SetFriendly(bool value)
        {
            isFriendly = value;
        }
    }

    public sealed class BattlefieldMinimapMarkerController : MonoBehaviour
    {
        private readonly Dictionary<Transform, RectTransform> markers =
            new Dictionary<Transform, RectTransform>();

        private readonly HashSet<Transform> visibleUnits =
            new HashSet<Transform>();

        private RectTransform overlay;
        private Renderer terrainRenderer;
        private Sprite markerSprite;
        private float refreshTimer;

        private static readonly Color FriendlyColor =
            new Color(0.1f, 0.55f, 1f, 1f);

        private static readonly Color EnemyColor =
            new Color(1f, 0.12f, 0.12f, 1f);

        private void Awake()
        {
            overlay = GetComponent<RectTransform>();

            GameObject terrain =
                GameObject.Find("MAP001_BLACKSPIRE_EXPANSE_TERRAIN");

            if (terrain != null)
                terrainRenderer = terrain.GetComponent<Renderer>();

            markerSprite = CreateMarkerSprite();
        }

        private void LateUpdate()
        {
            if (overlay == null || terrainRenderer == null)
                return;

            refreshTimer -= Time.unscaledDeltaTime;

            if (refreshTimer <= 0f)
            {
                refreshTimer = 0.25f;
                RefreshUnits();
            }

            UpdateMarkerPositions();
        }

        private void RefreshUnits()
        {
            visibleUnits.Clear();

            BattlefieldMinimapUnit[] friendlyUnits =
                FindObjectsOfType<BattlefieldMinimapUnit>();

            foreach (BattlefieldMinimapUnit unit in friendlyUnits)
            {
                if (unit == null || !unit.gameObject.activeInHierarchy)
                    continue;

                if (unit.IsFriendly)
                    Register(unit.transform, FriendlyColor);

                visibleUnits.Add(unit.transform);
            }

            GameObject[] enemies;

            try
            {
                enemies = GameObject.FindGameObjectsWithTag("Enemy");
            }
            catch (UnityException)
            {
                return;
            }

            foreach (GameObject enemy in enemies)
            {
                if (enemy == null || !enemy.activeInHierarchy)
                    continue;

                Register(enemy.transform, EnemyColor);
                visibleUnits.Add(enemy.transform);
            }

            List<Transform> removed = new List<Transform>();

            foreach (KeyValuePair<Transform, RectTransform> pair in markers)
            {
                if (pair.Key == null || !visibleUnits.Contains(pair.Key))
                    removed.Add(pair.Key);
            }

            foreach (Transform target in removed)
            {
                if (markers.TryGetValue(target, out RectTransform marker))
                {
                    if (marker != null)
                        Destroy(marker.gameObject);
                }

                markers.Remove(target);
            }
        }

        private void Register(Transform target, Color color)
        {
            if (target == null)
                return;

            if (!markers.TryGetValue(target, out RectTransform marker))
            {
                GameObject markerObject =
                    new GameObject("[MINIMAP MARKER] " + target.name,
                        typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));

                markerObject.transform.SetParent(overlay, false);

                marker = markerObject.GetComponent<RectTransform>();
                marker.sizeDelta = new Vector2(9f, 9f);
                marker.anchorMin = new Vector2(0.5f, 0.5f);
                marker.anchorMax = new Vector2(0.5f, 0.5f);
                marker.pivot = new Vector2(0.5f, 0.5f);

                Image image = markerObject.GetComponent<Image>();
                image.sprite = markerSprite;
                image.color = color;
                image.raycastTarget = false;

                marker.SetAsLastSibling();
                markers.Add(target, marker);
            }
            else
            {
                Image image = marker.GetComponent<Image>();
                if (image != null)
                    image.color = color;
            }
        }

        private void UpdateMarkerPositions()
        {
            Bounds bounds = terrainRenderer.bounds;
            Rect rect = overlay.rect;

            if (bounds.size.x <= 0f || bounds.size.z <= 0f)
                return;

            foreach (KeyValuePair<Transform, RectTransform> pair in markers)
            {
                Transform target = pair.Key;
                RectTransform marker = pair.Value;

                if (target == null || marker == null)
                    continue;

                Vector3 position = target.position;

                float normalizedX =
                    (position.x - bounds.min.x) / bounds.size.x;

                float normalizedY =
                    (position.z - bounds.min.z) / bounds.size.z;

                bool inside =
                    normalizedX >= 0f && normalizedX <= 1f &&
                    normalizedY >= 0f && normalizedY <= 1f;

                marker.gameObject.SetActive(inside);

                if (!inside)
                    continue;

                marker.anchoredPosition = new Vector2(
                    (normalizedX - 0.5f) * rect.width,
                    (normalizedY - 0.5f) * rect.height);
            }
        }

        private Sprite CreateMarkerSprite()
        {
            const int size = 16;

            Texture2D texture =
                new Texture2D(size, size, TextureFormat.RGBA32, false);

            texture.filterMode = FilterMode.Bilinear;

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float dx = x - 7.5f;
                    float dy = y - 7.5f;
                    float distance = Mathf.Sqrt(dx * dx + dy * dy);

                    texture.SetPixel(
                        x, y,
                        distance <= 7f
                            ? Color.white
                            : Color.clear);
                }
            }

            texture.Apply();

            return Sprite.Create(
                texture,
                new Rect(0, 0, size, size),
                new Vector2(0.5f, 0.5f));
        }

        private void OnDestroy()
        {
            if (markerSprite != null)
                Destroy(markerSprite.texture);
        }
    }
}
