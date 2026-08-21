using UnityEngine;

namespace Obsidian.VR
{
    public class VRUnitDamageRelay : MonoBehaviour
    {
        [SerializeField] private VRUnitContextProvider _contextProvider;
        [SerializeField] private VRUnitCameraEffects _cameraEffects;
        [SerializeField] private VRUnitOcclusionHandler _occlusion;
        [SerializeField] private VRUnitHaptics _haptics;

        private VRUnitContext _context;

        private void Awake()
        {
            if (_contextProvider == null)
                _contextProvider = GetComponent<VRUnitContextProvider>();

            if (_contextProvider == null)
                _contextProvider = Object.FindAnyObjectByType<VRUnitContextProvider>();

            if (_cameraEffects == null)
                _cameraEffects = GetComponent<VRUnitCameraEffects>();

            if (_cameraEffects == null)
                _cameraEffects = Object.FindAnyObjectByType<VRUnitCameraEffects>();

            if (_occlusion == null)
                _occlusion = GetComponent<VRUnitOcclusionHandler>();

            if (_occlusion == null)
                _occlusion = Object.FindAnyObjectByType<VRUnitOcclusionHandler>();

            if (_haptics == null)
                _haptics = GetComponent<VRUnitHaptics>();

            if (_haptics == null)
                _haptics = Object.FindAnyObjectByType<VRUnitHaptics>();
        }

        private void Update()
        {
            if (_contextProvider == null)
                return;

            _context = _contextProvider.Context;
            if (_context == null || !_context.Valid)
                return;

            TickDamage();
        }

        private void TickDamage()
        {
            var unit = _context.Unit;
            if (unit == null)
                return;

            float recentDamage = unit.GetRecentDamage();
            if (recentDamage <= 0f)
                return;

            float strength = Mathf.Clamp01(recentDamage);

            if (_cameraEffects != null)
                _cameraEffects.AddShake(strength * 0.5f, 0.15f);

            if (_occlusion != null)
                _occlusion.AddOcclusion(strength * 0.3f);

            if (_haptics != null)
                _haptics.ImpactPulse(strength);
        }
    }
}
