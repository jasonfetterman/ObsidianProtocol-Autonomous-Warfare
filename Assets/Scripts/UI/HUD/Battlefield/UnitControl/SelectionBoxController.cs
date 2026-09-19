using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectionBoxController : MonoBehaviour
{
    [SerializeField] private RectTransform selectionBoxRect;
    [SerializeField] private Camera battlefieldCamera;
    [SerializeField] private UnitSelectionManager selectionManager;

    private Vector2 startPosition;
    private bool isDragging;

    private void Awake()
    {
        selectionBoxRect.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartDrag();
        }

        if (isDragging && Mouse.current.leftButton.isPressed)
        {
            UpdateDrag();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            EndDrag();
        }
    }

    private void StartDrag()
    {
        isDragging = true;
        startPosition = Mouse.current.position.ReadValue();
        selectionBoxRect.gameObject.SetActive(true);
    }

    private void UpdateDrag()
    {
        Vector2 currentPosition = Mouse.current.position.ReadValue();

        Vector2 center = (startPosition + currentPosition) / 2f;
        Vector2 size = new Vector2(
            Mathf.Abs(currentPosition.x - startPosition.x),
            Mathf.Abs(currentPosition.y - startPosition.y));

        selectionBoxRect.position = center;
        selectionBoxRect.sizeDelta = size;
    }

    private void EndDrag()
    {
        isDragging = false;
        selectionBoxRect.gameObject.SetActive(false);

        Vector2 endPosition = Mouse.current.position.ReadValue();

        float minX = Mathf.Min(startPosition.x, endPosition.x);
        float maxX = Mathf.Max(startPosition.x, endPosition.x);
        float minY = Mathf.Min(startPosition.y, endPosition.y);
        float maxY = Mathf.Max(startPosition.y, endPosition.y);

        if (maxX - minX < 5f && maxY - minY < 5f)
        {
            return;
        }

        selectionManager.ClearSelection();

        SelectableUnit[] allUnits = FindObjectsByType<SelectableUnit>(FindObjectsSortMode.None);

        foreach (SelectableUnit unit in allUnits)
        {
            Vector3 screenPoint = battlefieldCamera.WorldToScreenPoint(unit.transform.position);

            if (screenPoint.x >= minX && screenPoint.x <= maxX &&
                screenPoint.y >= minY && screenPoint.y <= maxY)
            {
                selectionManager.AddToSelection(unit);
            }
        }
    }
}