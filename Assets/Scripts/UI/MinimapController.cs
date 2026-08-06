using Assets.Scripts.AI;     // SquadController
using Assets.Scripts.UI;     // SelectionManager
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class MinimapController : MonoBehaviour
    {
        public RectTransform minimapRect;
        public Camera minimapCamera;
        public Image clickMarker;

        private SquadController squadController;
        private SelectionManager selectionManager;

        void Awake()
        {
            squadController = ServiceLocator.Get<SquadController>();
            selectionManager = Object.FindAnyObjectByType<SelectionManager>();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
                HandleMinimapClick();
        }

        private void HandleMinimapClick()
        {
            Vector2 mousePos = Input.mousePosition;

            if (!RectTransformUtility.RectangleContainsScreenPoint(minimapRect, mousePos))
                return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                minimapRect, mousePos, null, out Vector2 localPoint);

            Vector2 normalized = new Vector2(
                (localPoint.x / minimapRect.rect.width) + 0.5f,
                (localPoint.y / minimapRect.rect.height) + 0.5f
            );

            Ray ray = minimapCamera.ViewportPointToRay(normalized);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var selected = selectionManager.GetSelected();
                if (selected.Count == 0)
                    return;

                squadController.IssueMoveCommand(selected, hit.point);

                ShowClickMarker(hit.point);
            }
        }

        private void ShowClickMarker(Vector3 worldPos)
        {
            if (clickMarker == null)
                return;

            Vector3 screenPos = minimapCamera.WorldToScreenPoint(worldPos);
            clickMarker.transform.position = screenPos;
            clickMarker.enabled = true;

            CancelInvoke(nameof(HideMarker));
            Invoke(nameof(HideMarker), 0.8f);
        }

        private void HideMarker()
        {
            if (clickMarker != null)
                clickMarker.enabled = false;
        }
    }
}
