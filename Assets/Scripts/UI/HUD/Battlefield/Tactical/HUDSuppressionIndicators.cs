using UnityEngine;
using UnityEngine.UI;

public class HUDSuppressionIndicators : MonoBehaviour
{
    [SerializeField] private GameObject indicatorVisual;
    [SerializeField] private Text indicatorText;

    public float SuppressionLevel { get; private set; }
    public bool IsSuppressed => SuppressionLevel > 0f;

    public void SetSuppression(float level)
    {
        SuppressionLevel = Mathf.Clamp01(level);

        if (indicatorText != null)
            indicatorText.text = IsSuppressed ? "SUPPRESSED" : "";

        if (indicatorVisual != null)
            indicatorVisual.SetActive(IsSuppressed);
    }

    public void ClearSuppression()
    {
        SetSuppression(0f);
    }
}
