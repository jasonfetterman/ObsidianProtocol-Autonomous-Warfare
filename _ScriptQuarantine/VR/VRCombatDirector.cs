using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Adaptive combat camera director.
    /// Dynamically blends between two styles based on combat intensity,
    /// health, stress, speed, suppression, explosions, kills, etc.
    /// </summary>
    public class VRCombatDirector : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRUnitFocusController _focus;

        [Header("Adaptive Style Blend")]
        [SerializeField] private CombatStyle calmStyle;     // Low intensity baseline
        [SerializeField] private CombatStyle intenseStyle;   // High intensity peak
        [SerializeField, Range(0f, 1f)] private float blendT;

        [Header("Adaptive Parameters")]
        [SerializeField] private float healthWeight = 0.4f;
        [SerializeField] private float stressWeight = 0.4f;
        [SerializeField] private float speedWeight = 0.2f;
        [SerializeField] private float suppressionWeight = 0.3f;
        [SerializeField] private float explosionSpike = 0.5f;
        [SerializeField] private float killSpike = 0.4f;
        [SerializeField] private float hitSpike = 0.3f;

        private float suppressionLevel;
        private float intensity;
        private BaseUnitVRController unit;

        private void Awake()
        {
            if (_session == null)
                _session = VRSessionManager.Instance;

            if (_focus == null)
                _focus = FindAnyObjectByType<VRUnitFocusController>();

            calmStyle = CombatStylesLibrary.Documentary;
            intenseStyle = CombatStylesLibrary.Heroic;
        }

        private void Update()
        {
            if (_session == null || !_session.SessionActive)
                return;

            unit = _session.ActiveUnit;
            if (unit == null)
                return;

            UpdateIntensity();
            blendT = Mathf.Clamp01(intensity);

            CombatStyle blended = CombatStyle.Lerp(calmStyle, intenseStyle, blendT);
            ApplyDynamicStates(blended);
        }

        // ------------------------------------------------------------
        // INTENSITY MODEL
        // ------------------------------------------------------------
        private void UpdateIntensity()
        {
            float healthFactor = 1f - Mathf.Clamp01(unit.GetHealth() / 100f);
            float stressFactor = Mathf.Clamp01(unit.StressLevel);
            float speedFactor = Mathf.Clamp01(unit.GetCurrentSpeed() / 6f);
            float suppressionFactor = Mathf.Clamp01(suppressionLevel);

            float baseIntensity =
                healthFactor * healthWeight +
                stressFactor * stressWeight +
                speedFactor * speedWeight +
                suppressionFactor * suppressionWeight;

            intensity = Mathf.Lerp(intensity, baseIntensity, Time.deltaTime * 3f);

            suppressionLevel = Mathf.Lerp(suppressionLevel, 0f, Time.deltaTime * 1.5f);
        }

        // ------------------------------------------------------------
        // GAMEPLAY EVENT API
        // ------------------------------------------------------------
        public void OnGunfire()
        {
            _focus.TriggerGunfire();
            intensity += 0.1f;
        }

        public void OnHit(Vector3 direction)
        {
            _focus.TriggerImpact(direction, hitSpike);
            intensity += hitSpike;
        }

        public void OnKill(Vector3 direction)
        {
            _focus.TriggerImpact(direction, killSpike);
            intensity += killSpike;
        }

        public void OnExplosion()
        {
            _focus.TriggerExplosion(explosionSpike);
            intensity += explosionSpike;
        }

        public void OnSuppression(Vector3 direction)
        {
            _focus.TriggerImpact(direction, suppressionWeight);
            suppressionLevel += 0.2f;
        }

        // ------------------------------------------------------------
        // APPLY BLENDED STYLE
        // ------------------------------------------------------------
        private void ApplyDynamicStates(CombatStyle style)
        {
            float health = unit.GetHealth();
            if (health <= 25f)
                _focus.TriggerImpact(Vector3.zero, style.lowHealthForce);

            float speed = unit.GetCurrentSpeed();
            if (speed > 4f)
                _focus.TriggerImpact(Vector3.forward, style.sprintForce);

            float stress = unit.StressLevel;
            if (stress > 0.7f)
                _focus.TriggerImpact(Vector3.zero, style.stressForce);
        }
    }
}
