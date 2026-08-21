using UnityEngine;
using UnityEngine.InputSystem;

namespace Obsidian.Vehicles
{
    public class VehicleSelectionManager : MonoBehaviour
    {
        [Header("Selection")]
        [SerializeField] private LayerMask vehicleLayer;

        [Header("Movement")]
        [SerializeField] private LayerMask groundLayer;

        private VehicleSelection selectedVehicle;
        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Mouse.current == null)
                return;

            if (Mouse.current.leftButton.wasPressedThisFrame)
                SelectVehicleUnderMouse();

            if (Mouse.current.rightButton.wasPressedThisFrame)
                MoveSelectedVehicle();
        }

        private void SelectVehicleUnderMouse()
        {
            if (mainCamera == null)
                return;

            Ray ray = mainCamera.ScreenPointToRay(
                Mouse.current.position.ReadValue()
            );

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, vehicleLayer))
            {
                VehicleSelection vehicle =
                    hit.collider.GetComponentInParent<VehicleSelection>();

                if (vehicle != null)
                {
                    SelectVehicle(vehicle);
                    return;
                }
            }

            ClearSelection();
        }

        private void SelectVehicle(VehicleSelection vehicle)
        {
            if (selectedVehicle == vehicle)
                return;

            if (selectedVehicle != null)
                selectedVehicle.Deselect();

            selectedVehicle = vehicle;
            selectedVehicle.Select();

            Debug.Log("Selected vehicle: " + selectedVehicle.name);
        }

        private void MoveSelectedVehicle()
        {
            if (selectedVehicle == null || mainCamera == null)
                return;

            Ray ray = mainCamera.ScreenPointToRay(
                Mouse.current.position.ReadValue()
            );

            if (!Physics.Raycast(
                ray,
                out RaycastHit hit,
                2000f,
                groundLayer,
                QueryTriggerInteraction.Ignore))
                return;

            VehicleMoveToPoint movement =
                selectedVehicle.GetComponent<VehicleMoveToPoint>();

            if (movement == null)
                return;

            movement.MoveTo(hit.point);

            Debug.Log("Vehicle moving to: " + hit.point);
        }

        private void ClearSelection()
        {
            if (selectedVehicle != null)
            {
                selectedVehicle.Deselect();
                selectedVehicle = null;
            }
        }

        public VehicleSelection GetSelectedVehicle()
        {
            return selectedVehicle;
        }
    }
}