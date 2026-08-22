using System;

namespace ObsidianProtocol.Game.Command
{
    public sealed class RTSCameraSystem
    {
        public float PositionX { get; private set; }
        public float PositionY { get; private set; }
        public float PositionZ { get; private set; }

        public float RotationY { get; private set; }
        public float Zoom { get; private set; }

        public float MinimumZoom { get; private set; }
        public float MaximumZoom { get; private set; }

        public float PanSpeed { get; private set; }
        public float RotationSpeed { get; private set; }
        public float ZoomSpeed { get; private set; }

        public bool Enabled { get; private set; }

        public RTSCameraSystem()
        {
            PositionX = 0f;
            PositionY = 20f;
            PositionZ = 0f;

            RotationY = 0f;
            Zoom = 20f;

            MinimumZoom = 5f;
            MaximumZoom = 60f;

            PanSpeed = 20f;
            RotationSpeed = 90f;
            ZoomSpeed = 20f;

            Enabled = true;
        }

        public void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }

        public void SetPosition(
            float x,
            float y,
            float z)
        {
            PositionX = x;
            PositionY = y;
            PositionZ = z;
        }

        public void Pan(
            float horizontal,
            float vertical)
        {
            if (!Enabled)
                return;

            PositionX += horizontal * PanSpeed;
            PositionZ += vertical * PanSpeed;
        }

        public void Rotate(float amount)
        {
            if (!Enabled)
                return;

            RotationY +=
                amount * RotationSpeed;

            if (RotationY >= 360f)
                RotationY -= 360f;

            if (RotationY < 0f)
                RotationY += 360f;
        }

        public void ZoomIn(float amount)
        {
            if (!Enabled)
                return;

            Zoom -=
                Math.Abs(amount) * ZoomSpeed;

            Zoom =
                Math.Max(MinimumZoom, Zoom);
        }

        public void ZoomOut(float amount)
        {
            if (!Enabled)
                return;

            Zoom +=
                Math.Abs(amount) * ZoomSpeed;

            Zoom =
                Math.Min(MaximumZoom, Zoom);
        }

        public void SetZoomLimits(
            float minimum,
            float maximum)
        {
            MinimumZoom =
                Math.Max(0.1f, minimum);

            MaximumZoom =
                Math.Max(
                    MinimumZoom,
                    maximum);

            Zoom =
                Math.Max(
                    MinimumZoom,
                    Math.Min(
                        MaximumZoom,
                        Zoom));
        }

        public void SetMovementSettings(
            float panSpeed,
            float rotationSpeed,
            float zoomSpeed)
        {
            PanSpeed =
                Math.Max(0f, panSpeed);

            RotationSpeed =
                Math.Max(0f, rotationSpeed);

            ZoomSpeed =
                Math.Max(0f, zoomSpeed);
        }

        public void Reset()
        {
            PositionX = 0f;
            PositionY = 20f;
            PositionZ = 0f;

            RotationY = 0f;
            Zoom = 20f;

            Enabled = true;
        }
    }
}
