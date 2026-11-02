using UnityEngine;

[DisallowMultipleComponent]
public sealed class StrategicDeploymentSignAnimation : MonoBehaviour
{
    [Header("Motion")]
    [SerializeField, Min(0f)] private float rotationDegreesPerSecond = 8f;

    [Header("Cyan Holographic Glow")]
    [SerializeField] private Color emissionColor = new Color(0.02f, 0.9f, 1f, 1f);
    [SerializeField, Min(0f)] private float minimumGlow = 0.6f;
    [SerializeField, Min(0f)] private float maximumGlow = 2.2f;
    [SerializeField, Min(0.01f)] private float pulseSpeed = 1.2f;

    private Renderer signRenderer;
    private MaterialPropertyBlock propertyBlock;
    private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        signRenderer = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();

        if (signRenderer == null)
        {
            Debug.LogWarning("[DEPLOYMENT SIGN] No Renderer found on " + name, this);
            enabled = false;
            return;
        }

        signRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(EmissionColorId, emissionColor * minimumGlow);
        signRenderer.SetPropertyBlock(propertyBlock);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationDegreesPerSecond * Time.deltaTime, Space.Self);

        if (signRenderer == null)
            return;

        float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) * 0.5f;
        float intensity = Mathf.Lerp(minimumGlow, maximumGlow, pulse);

        signRenderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(EmissionColorId, emissionColor * intensity);
        signRenderer.SetPropertyBlock(propertyBlock);
    }
}
