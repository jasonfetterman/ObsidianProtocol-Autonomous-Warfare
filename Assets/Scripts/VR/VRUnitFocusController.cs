using UnityEngine;

namespace Obsidian.VR
{
    public class VRUnitFocusController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private VRSessionManager _session;

        private Camera _focusCamera;

        [Header("Cinematic Motion")]
        [SerializeField] private float positionSpring = 14f;
        [SerializeField] private float rotationSpring = 16f;
        [SerializeField] private float fovSpring = 10f;

        [SerializeField] private float positionDamping = 0.65f;
        [SerializeField] private float rotationDamping = 0.7f;
        [SerializeField] private float fovDamping = 0.6f;

        private Vector3 posVelocity;
        private Vector3 rotVelocity;
        private float fovVelocity;

        [Header("Cut-In Effects")]
        [SerializeField] private float whipPanIntensity = 45f;
        [SerializeField] private float whipPanDecay = 6f;

        [SerializeField] private float snapZoomAmount = 12f;
        [SerializeField] private float snapZoomDecay = 5f;

        [SerializeField] private float shakeIntensity = 0.15f;
        [SerializeField] private float shakeDecay = 4f;

        private float whipPanVelocity;
        private float snapZoomVelocity;
        private float shakeVelocity;

        private Vector3 directionalShake;

        private void Awake()
        {
            if (_session == null)
                _session = VRSessionManager.Instance;

            _focusCamera = GetComponentInChildren<Camera>();
        }

        private void Update()
        {
            if (_session == null || !_session.SessionActive)
                return;

            var info = _session.Info;

            if (info.Mode != VRMode.UnitFocus)
                return;

            BaseUnitVRController unit = _session.ActiveUnit;
            if (unit == null)
                return;

            if (unit.UnitId != info.ActiveUnitId)
                return;

            if (unit.UnitPOVCamera == null)
                return;

            CinematicFocus(unit);
        }

        private void CinematicFocus(BaseUnitVRController unit)
        {
            Camera pov = unit.UnitPOVCamera;

            Quaternion whipOffset = Quaternion.Euler(0f, whipPanVelocity, 0f);

            Vector3 targetPos = pov.transform.position;
            _focusCamera.transform.position = SpringVector(
                _focusCamera.transform.position,
                targetPos,
                ref posVelocity,
                positionSpring,
                positionDamping
            );

            Quaternion targetRot = pov.transform.rotation * whipOffset;
            _focusCamera.transform.rotation = SpringRotation(
                _focusCamera.transform.rotation,
                targetRot,
                ref rotVelocity,
                rotationSpring,
                rotationDamping
            );

            float targetFov = pov.fieldOfView - snapZoomVelocity;
            _focusCamera.fieldOfView = SpringFloat(
                _focusCamera.fieldOfView,
                targetFov,
                ref fovVelocity,
                fovSpring,
                fovDamping
            );

            ApplyShake();

            whipPanVelocity *= Mathf.Exp(-whipPanDecay * Time.deltaTime);
            snapZoomVelocity *= Mathf.Exp(-snapZoomDecay * Time.deltaTime);
        }

        public void TriggerImpact(Vector3 direction, float force)
        {
            direction.Normalize();

            whipPanVelocity += direction.x * whipPanIntensity * force;
            directionalShake += direction * shakeIntensity * force;
            snapZoomVelocity += snapZoomAmount * force;
        }

        public void TriggerExplosion(float force)
        {
            shakeVelocity += shakeIntensity * force * 2f;
            snapZoomVelocity += snapZoomAmount * force * 1.5f;
            whipPanVelocity += whipPanIntensity * force * 0.5f;
        }

        public void TriggerGunfire()
        {
            shakeVelocity += shakeIntensity * 0.5f;
            snapZoomVelocity += snapZoomAmount * 0.3f;
        }

        private void ApplyShake()
        {
            directionalShake *= Mathf.Exp(-shakeDecay * Time.deltaTime);

            float shake = shakeVelocity + directionalShake.magnitude;
            if (shake <= 0.0001f)
                return;

            Vector3 offset = new Vector3(
                (Mathf.PerlinNoise(Time.time * 12f, 0f) - 0.5f) * shake,
                (Mathf.PerlinNoise(0f, Time.time * 12f) - 0.5f) * shake,
                0f
            );

            _focusCamera.transform.position += offset;
        }

        // ------------------------------------------------------------
        // SPRING FUNCTIONS — FIXED SIGNATURE
        // ------------------------------------------------------------
        private float SpringFloat(float current, float target, ref float velocity, float spring, float damping)
        {
            float delta = target - current;
            velocity += delta * spring * Time.deltaTime;
            velocity *= damping;
            return current + velocity * Time.deltaTime;
        }

        private Vector3 SpringVector(Vector3 current, Vector3 target, ref Vector3 velocity, float spring, float damping)
        {
            Vector3 delta = target - current;
            velocity += delta * spring * Time.deltaTime;
            velocity *= damping;
            return current + velocity * Time.deltaTime;
        }

        private Quaternion SpringRotation(Quaternion current, Quaternion target, ref Vector3 velocity, float spring, float damping)
        {
            Quaternion deltaRot = target * Quaternion.Inverse(current);
            deltaRot.ToAngleAxis(out float angle, out Vector3 axis);

            if (float.IsNaN(axis.x) || float.IsInfinity(axis.x))
                return current;

            Vector3 angularDelta = axis * angle;

            velocity += angularDelta * spring * Time.deltaTime;
            velocity *= damping;

            Quaternion step = Quaternion.Euler(velocity * Time.deltaTime);
            return step * current;
        }
    }
}
