using UnityEngine;

public class VehicleHoverRing : MonoBehaviour
{
    [Header("Ring Appearance")]
    [SerializeField] private Color ringColor = new Color(0.1f, 0.85f, 1f, 1f);
    [SerializeField] private float ringWidth = 0.035f;
    [SerializeField] private float ringRadiusMultiplier = 1.15f;

    [Header("Glow")]
    [SerializeField] private float pulseSpeed = 3f;
    [SerializeField] private float minimumAlpha = 0.45f;
    [SerializeField] private float maximumAlpha = 1f;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundLayers = ~0;
    [SerializeField] private float raycastHeight = 10f;
    [SerializeField] private float groundOffset = 0.05f;

    private GameObject ringObject;
    private LineRenderer ring;
    private Material ringMaterial;

    private bool isHovered;
    private float radius;

    private void Awake()
    {
        CreateRing();
        CalculateRadius();

        ringObject.SetActive(false);
    }

    private void Update()
    {
        if (!isHovered)
            return;

        UpdateRingPosition();
        AnimateRing();
    }

    private void OnMouseEnter()
    {
        isHovered = true;

        if (ringObject != null)
            ringObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        isHovered = false;

        if (ringObject != null)
            ringObject.SetActive(false);
    }

    private void CreateRing()
    {
        ringObject = new GameObject("Hover Ring");
        ringObject.transform.SetParent(transform);

        ring = ringObject.AddComponent<LineRenderer>();

        ring.useWorldSpace = true;
        ring.loop = true;
        ring.positionCount = 64;

        ring.startWidth = ringWidth;
        ring.endWidth = ringWidth;

        ring.shadowCastingMode =
            UnityEngine.Rendering.ShadowCastingMode.Off;

        ring.receiveShadows = false;

        CreateMaterial();

        ring.material = ringMaterial;
    }

    private void CreateMaterial()
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Unlit");

        if (shader == null)
            shader = Shader.Find("Unlit/Color");

        ringMaterial = new Material(shader);

        ringMaterial.name = "Vehicle Hover Ring Material";

        ringMaterial.color = ringColor;

        if (ringMaterial.HasProperty("_BaseColor"))
            ringMaterial.SetColor("_BaseColor", ringColor);

        if (ringMaterial.HasProperty("_Color"))
            ringMaterial.SetColor("_Color", ringColor);

        if (ringMaterial.HasProperty("_EmissionColor"))
        {
            ringMaterial.SetColor(
                "_EmissionColor",
                ringColor * 3f
            );

            ringMaterial.EnableKeyword("_EMISSION");
        }
    }

    private void CalculateRadius()
    {
        BoxCollider boxCollider = GetComponent<BoxCollider>();

        if (boxCollider != null)
        {
            Vector3 worldSize = Vector3.Scale(
                boxCollider.size,
                transform.lossyScale
            );

            radius = Mathf.Max(
                worldSize.x,
                worldSize.z
            ) * 0.5f;

            radius *= ringRadiusMultiplier;
        }
        else
        {
            Renderer renderer =
                GetComponentInChildren<Renderer>();

            if (renderer != null)
            {
                Vector3 size = renderer.bounds.size;

                radius = Mathf.Max(
                    size.x,
                    size.z
                ) * 0.5f;

                radius *= ringRadiusMultiplier;
            }
            else
            {
                radius = 1f;
            }
        }
    }

    private void UpdateRingPosition()
    {
        Vector3 center = transform.position;

        // Start above the vehicle and raycast toward the ground.
        Vector3 rayStart = center + Vector3.up * raycastHeight;

        if (Physics.Raycast(
            rayStart,
            Vector3.down,
            out RaycastHit hit,
            raycastHeight * 2f,
            groundLayers,
            QueryTriggerInteraction.Ignore))
        {
            center.y = hit.point.y + groundOffset;
        }
        else
        {
            // Fallback if no ground collider is found.
            center.y = transform.position.y + groundOffset;
        }

        for (int i = 0; i < ring.positionCount; i++)
        {
            float angle =
                (float)i / ring.positionCount * Mathf.PI * 2f;

            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            Vector3 ringPosition =
                center + new Vector3(x, 0f, z);

            ring.SetPosition(i, ringPosition);
        }
    }

    private void AnimateRing()
    {
        float pulse = Mathf.Lerp(
            minimumAlpha,
            maximumAlpha,
            (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f
        );

        Color currentColor = ringColor;
        currentColor.a = pulse;

        if (ringMaterial.HasProperty("_BaseColor"))
            ringMaterial.SetColor(
                "_BaseColor",
                currentColor
            );

        if (ringMaterial.HasProperty("_Color"))
            ringMaterial.SetColor(
                "_Color",
                currentColor
            );

        if (ringMaterial.HasProperty("_EmissionColor"))
            ringMaterial.SetColor(
                "_EmissionColor",
                ringColor * 3f
            );
    }

    private void OnDestroy()
    {
        if (ringMaterial != null)
            Destroy(ringMaterial);

        if (ringObject != null)
            Destroy(ringObject);
    }
}