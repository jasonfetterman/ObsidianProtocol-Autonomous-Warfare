using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace MiniRTS
{
    /// <summary>
    /// World-space RTS camera rig with keyboard/edge panning and wheel zoom.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class CameraController : MonoBehaviour
    {
        private Camera controlledCamera;
        private float zoomDistance = BalanceConfig.CameraDefaultZoom;
        private float targetZoomDistance = BalanceConfig.CameraDefaultZoom;
        private float mapHalfExtent;

        public void Initialize(Camera targetCamera, float mapSize)
        {
            controlledCamera = targetCamera;
            mapHalfExtent = mapSize * 0.5f;
            zoomDistance = BalanceConfig.CameraDefaultZoom;
            targetZoomDistance = zoomDistance;
            ApplyCameraTransform();
            ClampRigPosition();
        }

        public void FocusOn(Vector3 worldPosition)
        {
            transform.position = new Vector3(worldPosition.x, 0f, worldPosition.z);
            ClampRigPosition();
        }

        private void Update()
        {
            if (controlledCamera == null)
            {
                return;
            }

            Vector2 panInput = ReadPanInput();
            if (panInput.sqrMagnitude > 1f)
            {
                panInput.Normalize();
            }

            float zoomFactor = Mathf.Lerp(
                0.75f,
                1.35f,
                Mathf.InverseLerp(
                    BalanceConfig.CameraMinZoom,
                    BalanceConfig.CameraMaxZoom,
                    zoomDistance));
            Vector3 movement = new Vector3(panInput.x, 0f, panInput.y) *
                (BalanceConfig.CameraPanSpeed * zoomFactor * Time.unscaledDeltaTime);
            transform.position += movement;
            ClampRigPosition();

            Mouse mouse = Mouse.current;
            if (mouse != null)
            {
                float scroll = mouse.scroll.ReadValue().y;
                if (!Mathf.Approximately(scroll, 0f))
                {
                    targetZoomDistance = Mathf.Clamp(
                        targetZoomDistance - scroll * BalanceConfig.CameraZoomSpeed,
                        BalanceConfig.CameraMinZoom,
                        BalanceConfig.CameraMaxZoom);
                }
            }

            float zoomBlend = 1f - Mathf.Exp(
                -BalanceConfig.CameraZoomSharpness * Time.unscaledDeltaTime);
            float smoothedZoom = Mathf.Lerp(
                zoomDistance,
                targetZoomDistance,
                zoomBlend);
            if (!Mathf.Approximately(smoothedZoom, zoomDistance))
            {
                zoomDistance = smoothedZoom;
                ApplyCameraTransform();
            }
        }

        private Vector2 ReadPanInput()
        {
            Vector2 input = Vector2.zero;
            Keyboard keyboard = Keyboard.current;
            if (keyboard != null)
            {
                if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
                {
                    input.x -= 1f;
                }

                if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
                {
                    input.x += 1f;
                }

                if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
                {
                    input.y -= 1f;
                }

                if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
                {
                    input.y += 1f;
                }
            }

            Mouse mouse = Mouse.current;
            if (mouse == null)
            {
                return input;
            }

            if (EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject())
            {
                return input;
            }

            Vector2 pointer = mouse.position.ReadValue();
            if (pointer.x >= 0f && pointer.y >= 0f &&
                pointer.x <= Screen.width && pointer.y <= Screen.height)
            {
                if (pointer.x <= BalanceConfig.CameraEdgeSize)
                {
                    input.x -= 1f;
                }
                else if (pointer.x >= Screen.width - BalanceConfig.CameraEdgeSize)
                {
                    input.x += 1f;
                }

                if (pointer.y <= BalanceConfig.CameraEdgeSize)
                {
                    input.y -= 1f;
                }
                else if (pointer.y >= Screen.height - BalanceConfig.CameraEdgeSize)
                {
                    input.y += 1f;
                }
            }

            return input;
        }

        private void ApplyCameraTransform()
        {
            float pitchRadians = BalanceConfig.CameraPitch * Mathf.Deg2Rad;
            controlledCamera.transform.localPosition = new Vector3(
                0f,
                Mathf.Sin(pitchRadians) * zoomDistance,
                -Mathf.Cos(pitchRadians) * zoomDistance);
            controlledCamera.transform.localRotation =
                Quaternion.Euler(BalanceConfig.CameraPitch, 0f, 0f);
        }

        private void ClampRigPosition()
        {
            float limit = Mathf.Max(0f, mapHalfExtent - BalanceConfig.CameraBoundsPadding);
            Vector3 position = transform.position;
            position.x = Mathf.Clamp(position.x, -limit, limit);
            position.y = 0f;
            position.z = Mathf.Clamp(position.z, -limit, limit);
            transform.position = position;
        }
    }
}
