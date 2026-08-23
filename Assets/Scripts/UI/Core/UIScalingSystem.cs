using UnityEngine;
using UnityEngine.UI;

public class UIScalingSystem : MonoBehaviour
{
    public static UIScalingSystem Instance { get; private set; }

    [SerializeField] private CanvasScaler canvasScaler;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (canvasScaler == null)
            canvasScaler = GetComponent<CanvasScaler>();
    }

    public void SetReferenceResolution(Vector2 resolution)
    {
        if (canvasScaler == null)
            return;

        canvasScaler.referenceResolution = resolution;
    }

    public void SetScaleMode(CanvasScaler.ScaleMode mode)
    {
        if (canvasScaler == null)
            return;

        canvasScaler.uiScaleMode = mode;
    }

    public Vector2 GetReferenceResolution()
    {
        if (canvasScaler == null)
            return Vector2.zero;

        return canvasScaler.referenceResolution;
    }
}
