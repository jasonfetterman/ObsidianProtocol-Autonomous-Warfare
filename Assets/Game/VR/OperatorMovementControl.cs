using System;

namespace ObsidianProtocol.Game.VR
{
    public sealed class OperatorMovementControl
    {
        public bool Initialized { get; private set; }

        public bool Active { get; private set; }

        public string UnitId { get; private set; }

        public float ForwardInput { get; private set; }

        public float StrafeInput { get; private set; }

        public float VerticalInput { get; private set; }

        public float RotationInput { get; private set; }

        public bool Initialize(
            string unitId)
        {
            if (Initialized ||
                string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            UnitId =
                unitId.Trim();

            ClearInputs();

            Active = false;
            Initialized = true;

            return true;
        }

        public bool Activate()
        {
            if (!Initialized)
            {
                return false;
            }

            Active = true;

            return true;
        }

        public bool Deactivate()
        {
            if (!Initialized)
            {
                return false;
            }

            Active = false;
            ClearInputs();

            return true;
        }

        public bool SetMovement(
            float forward,
            float strafe,
            float vertical,
            float rotation)
        {
            if (!Initialized ||
                !Active ||
                forward < -1f ||
                forward > 1f ||
                strafe < -1f ||
                strafe > 1f ||
                vertical < -1f ||
                vertical > 1f ||
                rotation < -1f ||
                rotation > 1f)
            {
                return false;
            }

            ForwardInput = forward;
            StrafeInput = strafe;
            VerticalInput = vertical;
            RotationInput = rotation;

            return true;
        }

        public void ClearInputs()
        {
            ForwardInput = 0f;
            StrafeInput = 0f;
            VerticalInput = 0f;
            RotationInput = 0f;
        }

        public void Reset()
        {
            Initialized = false;
            Active = false;

            UnitId =
                string.Empty;

            ClearInputs();
        }
    }
}
