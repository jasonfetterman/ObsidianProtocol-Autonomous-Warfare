using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class SelectionBox : MonoBehaviour
{
    public Image boxImage;

    private RectTransform rect;
    private Vector2 startPos;
    private bool isDragging;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();

        if (boxImage != null)
            boxImage.enabled = false;
    }

    private void Update()
    {
        // Start drag
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            startPos = Input.mousePosition;

            if (boxImage != null)
                boxImage.enabled = true;
        }

        // End drag
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            if (boxImage != null)
                boxImage.enabled = false;
        }

        // Update drag box
        if (isDragging)
            UpdateBox(Input.mousePosition);
    }

    private void UpdateBox(Vector2 currentPos)
    {
        Vector2 size = currentPos - startPos;

        // Set size
        rect.sizeDelta = new Vector2(Mathf.Abs(size.x), Mathf.Abs(size.y));

        // Set pivot based on drag direction
        rect.pivot = new Vector2(
            size.x >= 0 ? 0f : 1f,
            size.y >= 0 ? 0f : 1f
        );

        // Set position
        rect.anchoredPosition = startPos;
    }
}
