using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Provides damage‑related feedback for FPV units (UGV/UAV/USV/UUV/etc).
    /// Screen shake, red flash, impact audio, etc.
    /// </summary>
    public class VRUnitDamageFeedback : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;

        [Header("Visual")]
        [SerializeField] private CanvasGroup _damageFlash;
        [SerializeField] private float flashDuration = 0.25f;
        private float _flashTimer;

        [Header("Audio")]
        [SerializeField] private AudioSource _impactAudio;

        private BaseUnitVRController _unit;

        private void Awake()
        {
            if (_session == null)
                _session = Object.FindAnyObjectByType<VRSessionManager>();

            if (_damageFlash != null)
                _damageFlash.alpha = 0f;
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

            if (_unit == null)
                return;

            TickFlash();
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

        public void TriggerDamageFlash()
        {
            if (_damageFlash == null)
                return;

            _flashTimer = flashDuration;
            _damageFlash.alpha = 1f;

            if (_impactAudio != null)
                _impactAudio.Play();
        }

        private void TickFlash()
        {
            if (_damageFlash == null)
                return;

            if (_flashTimer > 0f)
            {
                _flashTimer -= Time.deltaTime;
                float t = Mathf.Clamp01(_flashTimer / flashDuration);
                _damageFlash.alpha = t;
            }
        }
    }
}
