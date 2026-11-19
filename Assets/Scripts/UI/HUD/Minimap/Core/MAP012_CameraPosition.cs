using UnityEngine;
using UnityEngine.UI;

namespace ObsidianProtocol.UI.HUD.Minimap
{
    public class MAP012_CameraPosition : MonoBehaviour
    {
        public const string ID = "MAP-012";

        [SerializeField] private float markerSize = 13f;
        [SerializeField] private Color cameraColor = Color.white;

        private RectTransform markerLayer;
        private RectTransform markerRect;
        private Transform playerTransform;
        private Camera battlefieldCamera;
        private Renderer terrainRenderer;

        private void Awake()
        {
            markerLayer = GetComponent<RectTransform>();
            battlefieldCamera = Camera.main;

            GameObject player = GameObject.Find("PLAYER_INSTANCE");
            if (player != null) playerTransform = player.transform;

            GameObject terrain = GameObject.Find("MAP001_BLACKSPIRE_EXPANSE_TERRAIN");
            if (terrain != null) terrainRenderer = terrain.GetComponent<Renderer>();

            GameObject marker = new GameObject(
                "PLAYER_CAMERA_POSITION",
                typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));

            marker.transform.SetParent(markerLayer, false);
            markerRect = marker.GetComponent<RectTransform>();
            markerRect.sizeDelta = new Vector2(markerSize, markerSize);

            Image image = marker.GetComponent<Image>();
            image.sprite = MAPMinimapMarkerUtility.DotSprite;
            image.color = cameraColor;
            image.raycastTarget = false;

            Debug.Log("[Minimap] MAP-012 dynamic terrain-bound tracking initialized.");
        }

        private void LateUpdate()
        {
            if (playerTransform == null)
            {
                GameObject player = GameObject.Find("PLAYER_INSTANCE");
                if (player != null) playerTransform = player.transform;
            }

            if (battlefieldCamera == null) battlefieldCamera = Camera.main;
            if (markerRect == null || markerLayer == null) return;

            Transform target = playerTransform != null
                ? playerTransform
                : battlefieldCamera != null ? battlefieldCamera.transform : null;

            if (target == null || terrainRenderer == null) return;

            Bounds bounds = terrainRenderer.bounds;
            Vector3 position = target.position;

            float x = Mathf.InverseLerp(bounds.min.x, bounds.max.x, position.x);
            float y = Mathf.InverseLerp(bounds.min.z, bounds.max.z, position.z);

            Rect rect = markerLayer.rect;
            markerRect.anchoredPosition = new Vector2(
                (x - 0.5f) * rect.width,
                (y - 0.5f) * rect.height
            );

            float yaw = battlefieldCamera != null
                ? battlefieldCamera.transform.eulerAngles.y
                : target.eulerAngles.y;

            markerRect.localEulerAngles = new Vector3(0f, 0f, -yaw);
        }
    }
}
