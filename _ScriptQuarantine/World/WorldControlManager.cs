using UnityEngine;

namespace ObsidianProtocol.World
{
    public class WorldControlManager : MonoBehaviour
    {
        [Header("Control State")]
        [SerializeField]
        private WorldControlState controlState =
            new WorldControlState();

        [Header("World Systems")]
        [SerializeField]
        private WorldUnitRegistry unitRegistry;

        public WorldControlState State =>
            controlState;

        public WorldControlMode CurrentMode =>
            controlState.mode;

        public string ControlledUnitId =>
            controlState.controlledUnitInstanceId;

        public bool HasControlledUnit =>
            controlState.hasControlledUnit;

        public bool SetMode(
            WorldControlMode mode)
        {
            if (!IsModeAvailable(mode))
            {
                Debug.LogWarning(
                    $"WorldControlManager: Control mode unavailable: {mode}");

                return false;
            }

            controlState.SetMode(mode);

            Debug.Log(
                $"WORLD CONTROL MODE: {mode}");

            return true;
        }

        public bool ControlUnit(
            string unitInstanceId)
        {
            if (unitRegistry == null)
            {
                Debug.LogWarning(
                    "WorldControlManager: Unit registry missing.");

                return false;
            }

            if (!unitRegistry.Contains(
                    unitInstanceId))
            {
                Debug.LogWarning(
                    $"WorldControlManager: Unit not found: {unitInstanceId}");

                return false;
            }

            controlState.SetControlledUnit(
                unitInstanceId);

            return true;
        }

        public void ReleaseUnit()
        {
            controlState.ClearControlledUnit();
        }

        public bool IsModeAvailable(
            WorldControlMode mode)
        {
            switch (mode)
            {
                case WorldControlMode.RTS:
                    return true;

                case WorldControlMode.DirectControl:
                    return true;

                case WorldControlMode.FreeRoam:
                    return true;

                case WorldControlMode.VR:
                    return true;

                default:
                    return false;
            }
        }

        public bool IsControlling(
            string unitInstanceId)
        {
            return controlState.hasControlledUnit &&
                   controlState.controlledUnitInstanceId ==
                   unitInstanceId;
        }

        public void ResetControl()
        {
            controlState.SetMode(
                WorldControlMode.RTS);

            controlState.ClearControlledUnit();
        }
    }
}
