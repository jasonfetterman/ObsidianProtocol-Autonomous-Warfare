using System;

namespace ObsidianProtocol.World
{
    public enum WorldControlMode
    {
        RTS,
        DirectControl,
        FreeRoam,
        VR
    }

    [Serializable]
    public class WorldControlState
    {
        public WorldControlMode mode =
            WorldControlMode.RTS;

        public string controlledUnitInstanceId;

        public bool hasControlledUnit;

        public void SetMode(
            WorldControlMode newMode)
        {
            mode = newMode;
        }

        public void SetControlledUnit(
            string unitInstanceId)
        {
            controlledUnitInstanceId =
                unitInstanceId;

            hasControlledUnit =
                !string.IsNullOrWhiteSpace(
                    unitInstanceId);
        }

        public void ClearControlledUnit()
        {
            controlledUnitInstanceId =
                string.Empty;

            hasControlledUnit = false;
        }
    }
}
