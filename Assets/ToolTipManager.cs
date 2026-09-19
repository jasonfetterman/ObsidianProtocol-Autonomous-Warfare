using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class TooltipManager : MonoBehaviour
{
    [Header("References")]
    public Canvas tooltipCanvas;
    public RectTransform tooltipBackground;
    public TextMeshProUGUI tooltipText;

    [Header("Settings")]
    public Vector2 offset = new Vector2(15f, -15f);
    private bool isVisible;

    void Awake()
    {
        HideTooltip();
    }

    void Update()
    {
        if (isVisible)
        {
            Vector3 mousePos = Input.mousePosition;
            tooltipCanvas.transform.position = mousePos + new Vector3(offset.x, offset.y, 0f);
        }
    }

    public void ShowTooltip(string message)
    {
        if (tooltipText == null || tooltipBackground == null || tooltipCanvas == null)
            return;

        tooltipText.text = message;
        tooltipBackground.sizeDelta = new Vector2(
            tooltipText.preferredWidth + 20f,
            tooltipText.preferredHeight + 10f
        );

        tooltipCanvas.enabled = true;
        isVisible = true;
    }

    public void HideTooltip()
    {
        if (tooltipCanvas != null)
            tooltipCanvas.enabled = false;

        isVisible = false;
    }
}

