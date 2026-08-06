using UnityEngine;

namespace Obsidian.VR
{
    public class BaseUnitVRController : MonoBehaviour
    {
        public int UnitId { get; set; }
        public Camera UnitPOVCamera { get; set; }

        // Movement input
        public Vector2 MoveVector { get; private set; }
        public float Throttle { get; private set; }

        public void SetMoveInput(Vector2 move) => MoveVector = move;
        public void SetThrottle(float value) => Throttle = value;

        // ⭐ REQUIRED BY VRUnitHaptics
        public float GetThrottleLevel()
        {
            return Throttle;
        }

        // VR pose fields
        public Vector3 HeadPosition { get; set; }
        public Quaternion HeadRotation { get; set; }

        public Vector3 BodyPosition { get; set; }
        public Quaternion BodyRotation { get; set; }

        // Operator state
        public float Posture { get; private set; }
        public float Stance { get; private set; }
        public float BreathingRate { get; private set; }
        public float StressLevel { get; private set; }

        public void SetOperatorBreathing(float value) => BreathingRate = value;
        public void SetOperatorPosture(float value) => Posture = value;
        public void SetOperatorStance(float value) => Stance = value;
        public void SetOperatorStress(float value) => StressLevel = value;

        // Telemetry
        public float GetHealth() => 100f;
        public bool IsInCombat() => false;
        public bool IsAlive() => true;
        public float GetBatteryLevel() => 100f;
        public float GetCurrentSpeed() => 0f;
        public int GetAmmoCount() => 0;

        // ⭐ REQUIRED BY VRUnitDamageRelay
        public float GetRecentDamage()
        {
            return 0f;
        }

        // ⭐ REQUIRED BY VRInputMapper
        public void ApplyMovement(float throttle, float strafe, float yaw, float pitch)
        {
        }

        public void FirePrimary()
        {
        }

        public void CycleTarget()
        {
        }

        // ⭐ REQUIRED BY VRUnitCollisionHandler
        public void OnImpact(float strength)
        {
        }

        // ⭐ REQUIRED BY VRUnitDeathHandler
        public void DeactivateVRControl()
        {
        }
    }
}
