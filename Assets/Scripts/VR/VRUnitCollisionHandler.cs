using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Handles collision events for FPV units (UGV/UAV/USV/UUV/etc).
    /// Provides haptic feedback, impact effects, and VR damage integration.
    /// </summary>
    public class VRUnitCollisionHandler : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRUnitDamageFeedback _damageFeedback;

        [Header("Impact Settings")]
        public float minImpactVelocity = 1.5f;
        public float maxImpactVelocity = 12f;

        private BaseUnitVRController _unit;
        private Rigidbody _rb;

        private void Awake()
        {
            if (_session == null)
                _session = Object.FindAnyObjectByType<VRSessionManager>();

            if (_damageFeedback == null)
                _damageFeedback = Object.FindAnyObjectByType<VRUnitDamageFeedback>();

            _rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (_session == null)
                return;

            if (_session.Info.Mode != VRMode.Operator)
                return;

            int id = _session.Info.ActiveUnitId;
            if (id < 0)
                return;

            if (_unit == null || _unit.UnitId != id)
                _unit = FindUnit(id);
        }

        private BaseUnitVRController FindUnit(int id)
        {
            var units = Object.FindObjectsByType<BaseUnitVRController>();
            foreach (var u in units)
            {
                if (u.UnitId == id)
                    return u;
            }
            return null;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (_unit == null)
                return;

            if (_rb == null)
                return;

            float impact = collision.relativeVelocity.magnitude;
            if (impact < minImpactVelocity)
                return;

            float normalized = Mathf.InverseLerp(minImpactVelocity, maxImpactVelocity, impact);

            if (_damageFeedback != null)
                _damageFeedback.TriggerDamageFlash();

            _unit.OnImpact(normalized);
        }
    }
}
