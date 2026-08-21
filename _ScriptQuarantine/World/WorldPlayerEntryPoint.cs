using UnityEngine;

namespace ObsidianProtocol.World
{
    public class WorldPlayerEntryPoint : MonoBehaviour
    {
        [Header("Entry Point")]
        [SerializeField]
        private string entryPointId = "GARAGE_ENTRY_01";

        [SerializeField]
        private string worldId = "WORLD_01";

        [Header("Availability")]
        [SerializeField]
        private bool available = true;

        [Header("Modes")]
        [SerializeField]
        private bool allowRTS = true;

        [SerializeField]
        private bool allowDirectControl = true;

        [SerializeField]
        private bool allowFreeRoam = true;

        [SerializeField]
        private bool allowVR = true;

        public string EntryPointId =>
            entryPointId;

        public string WorldId =>
            worldId;

        public bool Available =>
            available;

        public Vector3 Position =>
            transform.position;

        public Quaternion Rotation =>
            transform.rotation;

        public bool Supports(
            WorldControlMode mode)
        {
            switch (mode)
            {
                case WorldControlMode.RTS:
                    return allowRTS;

                case WorldControlMode.DirectControl:
                    return allowDirectControl;

                case WorldControlMode.FreeRoam:
                    return allowFreeRoam;

                case WorldControlMode.VR:
                    return allowVR;

                default:
                    return false;
            }
        }

        public void SetAvailable(
            bool value)
        {
            available = value;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(
                transform.position,
                0.75f);

            Gizmos.DrawLine(
                transform.position,
                transform.position +
                transform.forward * 2f);
        }
    }
}
