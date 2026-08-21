using UnityEngine;
using UnityEngine.UI;

public class MinimapUnitMarker : MonoBehaviour
{
    [Header("Minimap")]
    [SerializeField] private RectTransform minimapRect;
    [SerializeField] private RectTransform markerParent;

    [Header("Marker")]
    [SerializeField] private Color markerColor = new Color(0.1f, 0.85f, 1f, 1f);
    [SerializeField] private float markerSize = 8f;

    [Header("World Area")]
    [SerializeField] private float worldMinX = -500f;
    [SerializeField] private float worldMaxX = 500f;
    [SerializeField] private float worldMinZ = -500f;
    [SerializeField] private float worldMaxZ = 500f;

    private RectTransform marker;

    private void Start()
    {
        CreateMarker();
    }

    private void LateUpdate()
    {
        if (marker == null)
            return;

        UpdateMarkerPosition();
    }

    private void CreateMarker()
    {
        GameObject markerObject = new GameObject(
            gameObject.name + "_MinimapMarker"
        );

        markerObject.transform.SetParent(markerParent, false);

        marker = markerObject.AddComponent<RectTransform>();

        marker.anchorMin = new Vector2(0.5f, 0.5f);
        marker.anchorMax = new Vector2(0.5f, 0.5f);
        marker.pivot = new Vector2(0.5f, 0.5f);

        marker.sizeDelta = new Vector2(
            markerSize,
            markerSize
        );

        Image image = markerObject.AddComponent<Image>();

        image.color = markerColor;

        CreateGlow(markerObject);
    }

    private void CreateGlow(GameObject markerObject)
    {
        GameObject glowObject = new GameObject("Glow");

        glowObject.transform.SetParent(markerObject.transform, false);

        RectTransform glow = glowObject.AddComponent<RectTransform>();

        glow.anchorMin = new Vector2(0.5f, 0.5f);
        glow.anchorMax = new Vector2(0.5f, 0.5f);
        glow.pivot = new Vector2(0.5f, 0.5f);

        glow.sizeDelta = new Vector2(
            markerSize * 2.5f,
            markerSize * 2.5f
        );

        Image glowImage = glowObject.AddComponent<Image>();

        glowImage.color = new Color(
            markerColor.r,
            markerColor.g,
            markerColor.b,
            0.2f
        );

        glow.SetAsFirstSibling();
    }

    private void UpdateMarkerPosition()
    {
        float normalizedX = Mathf.InverseLerp(
            worldMinX,
            worldMaxX,
            transform.position.x
        );

        float normalizedZ = Mathf.InverseLerp(
            worldMinZ,
            worldMaxZ,
            transform.position.z
        );

        float x = (normalizedX - 0.5f) * minimapRect.rect.width;
        float y = (normalizedZ - 0.5f) * minimapRect.rect.height;

        marker.anchoredPosition = new Vector2(x, y);
    }

    private void OnDestroy()
    {
        if (marker != null)
            Destroy(marker.gameObject);
    }
}