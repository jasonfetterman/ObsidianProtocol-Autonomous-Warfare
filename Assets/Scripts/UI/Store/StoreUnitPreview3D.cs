using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ObsidianProtocol.UI.Store
{
    [DisallowMultipleComponent]
    public sealed class StoreUnitPreview3D : MonoBehaviour,
        IPointerDownHandler, IDragHandler, IScrollHandler
    {
        private const string WardenUnitId = "obs-air-warden";
        private const string WardenResourcePath = "StorePreviews/WardenPreview";
        private const int PreviewLayer = 30;

        private RawImage previewImage;
        private Text statusText;
        private Camera previewCamera;
        private RenderTexture renderTexture;
        private GameObject previewModel;
        private Light keyLight;
        private Light fillLight;

        private float rotationY = 25f;
        private float rotationX = 0f;
        private float zoom = 1f;
        private float modelRadius = 2f;
        private Vector3 modelCenter;
        private bool initialized;

        public void Initialize()
        {
            if (initialized)
                return;

            initialized = true;

            RectTransform host = transform as RectTransform;
            if (host == null)
            {
                Debug.LogError("[STORE PREVIEW] Preview host needs a RectTransform.", this);
                return;
            }

            previewImage = GetComponentInChildren<RawImage>(true);
            if (previewImage == null)
            {
                GameObject imageObject = new GameObject(
                    "MODEL_RENDER_TEXTURE",
                    typeof(RectTransform),
                    typeof(CanvasRenderer),
                    typeof(RawImage));

                imageObject.transform.SetParent(transform, false);
                RectTransform imageRect = imageObject.GetComponent<RectTransform>();
                imageRect.anchorMin = Vector2.zero;
                imageRect.anchorMax = Vector2.one;
                imageRect.offsetMin = Vector2.zero;
                imageRect.offsetMax = Vector2.zero;

                previewImage = imageObject.GetComponent<RawImage>();
            }

            previewImage.raycastTarget = true;
            previewImage.color = Color.white;

            GameObject statusObject = new GameObject(
                "PREVIEW_STATUS",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Text));

            statusObject.transform.SetParent(transform, false);
            RectTransform statusRect = statusObject.GetComponent<RectTransform>();
            statusRect.anchorMin = new Vector2(0.05f, 0.40f);
            statusRect.anchorMax = new Vector2(0.95f, 0.60f);
            statusRect.offsetMin = Vector2.zero;
            statusRect.offsetMax = Vector2.zero;

            statusText = statusObject.GetComponent<Text>();
            statusText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            statusText.fontSize = 20;
            statusText.alignment = TextAnchor.MiddleCenter;
            statusText.color = new Color(0.82f, 0.85f, 0.89f, 1f);
            statusText.horizontalOverflow = HorizontalWrapMode.Wrap;
            statusText.verticalOverflow = VerticalWrapMode.Overflow;
            statusText.raycastTarget = false;
            statusText.text = "SELECT A UNIT";

            CreatePreviewCamera();
        }

        public void ShowUnit(string unitId)
        {
            if (!initialized)
                Initialize();

            if (previewCamera == null)
                return;

            ClearPreviewModel();

            if (unitId != WardenUnitId)
            {
                SetStatus("3D MODEL NOT ASSIGNED\n\nPreview unavailable for this unit.");
                return;
            }

            GameObject prefab = Resources.Load<GameObject>(WardenResourcePath);
            if (prefab == null)
            {
                SetStatus("WARDEN MODEL NOT FOUND\n\nCheck the StorePreviews resource asset.");
                Debug.LogError("[STORE PREVIEW] Could not load Resources/" +
                    WardenResourcePath + ".prefab", this);
                return;
            }

            previewModel = Instantiate(prefab);
            previewModel.name = "STORE_PREVIEW_WARDEN";
            previewModel.transform.position = Vector3.zero;
            previewModel.transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

            SetLayerRecursively(previewModel, PreviewLayer);

            Renderer[] renderers = previewModel.GetComponentsInChildren<Renderer>(true);
            if (renderers.Length == 0)
            {
                SetStatus("WARDEN MODEL HAS NO RENDERERS");
                Debug.LogError("[STORE PREVIEW] Warden prefab contains no renderers.", this);
                ClearPreviewModel();
                return;
            }

            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
                bounds.Encapsulate(renderers[i].bounds);

            modelCenter = bounds.center;
            modelRadius = Mathf.Max(bounds.extents.magnitude, 0.25f);

            previewModel.transform.position -= modelCenter;
            modelCenter = Vector3.zero;

            rotationX = 0f;
            zoom = 1f;
            UpdateModelRotation();
            FrameCamera();

            SetStatus("WARDEN  /  3D INSPECTION\nDrag to rotate  |  Scroll to zoom");
            Debug.Log("[STORE PREVIEW] Warden model loaded. Renderers: " + renderers.Length, this);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (previewModel != null)
                UpdateModelRotation();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (previewModel == null || eventData == null)
                return;

            rotationY += eventData.delta.x * 0.45f;
            rotationX = Mathf.Clamp(rotationX - eventData.delta.y * 0.25f, -20f, 20f);
            UpdateModelRotation();
        }

        public void OnScroll(PointerEventData eventData)
        {
            if (previewModel == null || eventData == null)
                return;

            zoom = Mathf.Clamp(zoom - eventData.scrollDelta.y * 0.08f, 0.65f, 1.8f);
            FrameCamera();
        }

        private void CreatePreviewCamera()
        {
            GameObject cameraObject = new GameObject("STORE_PREVIEW_CAMERA");
            cameraObject.transform.SetParent(transform, false);

            previewCamera = cameraObject.AddComponent<Camera>();
            previewCamera.enabled = true;
            previewCamera.clearFlags = CameraClearFlags.SolidColor;
            previewCamera.backgroundColor = new Color(0.035f, 0.045f, 0.06f, 1f);
            previewCamera.fieldOfView = 32f;
            previewCamera.nearClipPlane = 0.01f;
            previewCamera.farClipPlane = 1000f;
            previewCamera.cullingMask = 1 << PreviewLayer;
            previewCamera.allowHDR = true;
            previewCamera.allowMSAA = true;

            renderTexture = new RenderTexture(1024, 1024, 24, RenderTextureFormat.ARGB32);
            renderTexture.name = "STORE_WARDEN_PREVIEW_RT";
            renderTexture.Create();
            previewCamera.targetTexture = renderTexture;
            previewImage.texture = renderTexture;

            GameObject keyObject = new GameObject("STORE_PREVIEW_KEY_LIGHT");
            keyObject.transform.SetParent(transform, false);
            keyLight = keyObject.AddComponent<Light>();
            keyLight.type = LightType.Directional;
            keyLight.color = new Color(0.78f, 0.86f, 1f);
            keyLight.intensity = 1.35f;
            keyLight.cullingMask = 1 << PreviewLayer;
            keyObject.transform.rotation = Quaternion.Euler(35f, -35f, 0f);

            GameObject fillObject = new GameObject("STORE_PREVIEW_FILL_LIGHT");
            fillObject.transform.SetParent(transform, false);
            fillLight = fillObject.AddComponent<Light>();
            fillLight.type = LightType.Directional;
            fillLight.color = new Color(1f, 0.72f, 0.42f);
            fillLight.intensity = 0.65f;
            fillLight.cullingMask = 1 << PreviewLayer;
            fillObject.transform.rotation = Quaternion.Euler(20f, 145f, 0f);
        }

        private void UpdateModelRotation()
        {
            if (previewModel == null)
                return;

            previewModel.transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
            FrameCamera();
        }

        private void FrameCamera()
        {
            if (previewCamera == null || previewModel == null)
                return;

            float distance = (modelRadius / Mathf.Sin(previewCamera.fieldOfView * Mathf.Deg2Rad * 0.5f))
                * 1.15f / zoom;

            Vector3 target = previewModel.transform.position + modelCenter;
            previewCamera.transform.position = target + new Vector3(
                0f,
                modelRadius * 0.12f,
                -distance);

            previewCamera.transform.LookAt(target + Vector3.up * modelRadius * 0.08f);
        }

        private void SetStatus(string message)
        {
            if (statusText != null)
                statusText.text = message;
        }

        private static void SetLayerRecursively(GameObject root, int layer)
        {
            root.layer = layer;

            foreach (Transform child in root.transform)
                SetLayerRecursively(child.gameObject, layer);
        }

        private void ClearPreviewModel()
        {
            if (previewModel == null)
                return;

            if (Application.isPlaying)
                Destroy(previewModel);
            else
                DestroyImmediate(previewModel);

            previewModel = null;
        }

        private void OnDestroy()
        {
            ClearPreviewModel();

            if (previewCamera != null)
            {
                previewCamera.targetTexture = null;
                if (Application.isPlaying)
                    Destroy(previewCamera.gameObject);
                else
                    DestroyImmediate(previewCamera.gameObject);
            }

            if (keyLight != null)
            {
                if (Application.isPlaying)
                    Destroy(keyLight.gameObject);
                else
                    DestroyImmediate(keyLight.gameObject);
            }

            if (fillLight != null)
            {
                if (Application.isPlaying)
                    Destroy(fillLight.gameObject);
                else
                    DestroyImmediate(fillLight.gameObject);
            }

            if (renderTexture != null)
            {
                renderTexture.Release();
                if (Application.isPlaying)
                    Destroy(renderTexture);
                else
                    DestroyImmediate(renderTexture);
            }
        }
    }
}

