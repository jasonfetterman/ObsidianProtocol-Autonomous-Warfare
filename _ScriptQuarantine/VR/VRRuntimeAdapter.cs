using UnityEngine;

namespace Obsidian.VR
{
    public class VRRuntimeAdapter : MonoBehaviour
    {
        public BaseUnitVRController ActiveUnit { get; private set; }

        public VRHapticsAdapter Haptics { get; private set; }
        public VRNetworkAdapter Network { get; private set; }

        public BaseUnitVRController Operator => ActiveUnit;

        public void Bind(BaseUnitVRController unit)
        {
            ActiveUnit = unit;
        }

        public void BindNetwork(VRNetworkAdapter net)
        {
            Network = net;
        }

        public void BindHaptics(VRHapticsAdapter hap)
        {
            Haptics = hap;
        }

        // Movement input
        public Vector2 GetMoveVector() =>
            ActiveUnit != null ? ActiveUnit.MoveVector : Vector2.zero;

        public float GetThrottle() =>
            ActiveUnit != null ? ActiveUnit.Throttle : 0f;

        // Pose
        public Vector3 GetHeadPosition() =>
            ActiveUnit != null ? ActiveUnit.HeadPosition : Vector3.zero;

        public Quaternion GetHeadRotation() =>
            ActiveUnit != null ? ActiveUnit.HeadRotation : Quaternion.identity;

        public Vector3 GetBodyPosition() =>
            ActiveUnit != null ? ActiveUnit.BodyPosition : Vector3.zero;

        public Quaternion GetBodyRotation() =>
            ActiveUnit != null ? ActiveUnit.BodyRotation : Quaternion.identity;

        // Telemetry
        public float GetHealth() => ActiveUnit != null ? ActiveUnit.GetHealth() : 0f;
        public float GetBattery() => ActiveUnit != null ? ActiveUnit.GetBatteryLevel() : 0f;
        public float GetSpeed() => ActiveUnit != null ? ActiveUnit.GetCurrentSpeed() : 0f;
        public int GetAmmo() => ActiveUnit != null ? ActiveUnit.GetAmmoCount() : 0;
        public bool IsAlive() => ActiveUnit != null && ActiveUnit.IsAlive();
        public bool IsInCombat() => ActiveUnit != null && ActiveUnit.IsInCombat();

        // Required by VRUnitSelectionManager
        public bool IsInteractPressed()
        {
            return Input.GetKey(KeyCode.E);
        }

        // Required by VRUnitDeathHandler
        public void SetActiveCamera(Camera cam)
        {
            if (cam == null)
            {
                if (Camera.main != null)
                    Camera.main.enabled = true;
                return;
            }

            cam.enabled = true;
        }

        // ⭐ REQUIRED BY VRUnitPoseSync
        public (Vector3 position, Quaternion rotation)? GetHeadPose()
        {
            if (ActiveUnit == null)
                return null;

            return (ActiveUnit.HeadPosition, ActiveUnit.HeadRotation);
        }

        // ⭐ REQUIRED BY VROperatorLink
        public bool IsLinkPressed()
        {
            return Input.GetKey(KeyCode.L);
        }

        public bool IsUnlinkPressed()
        {
            return Input.GetKey(KeyCode.U);
        }

        // ⭐ REQUIRED BY VRUnitAudioRelay
        public bool IsTriggerPressed()
        {
            return Input.GetKey(KeyCode.Mouse0);
        }
    }
}
