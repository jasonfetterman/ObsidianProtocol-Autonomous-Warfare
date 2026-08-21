using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Handles temporary visual occlusion (dust, water, smoke, blackout)
    /// for the active FPV POV camera. Uses VRUnitContext for gating.
    /// </summary>
    public class VRUnitOcclusionHandler : MonoBehaviour
    {
        [SerializeField] private VRUnitContextProvider _contextProvider;

        private VRUnitContext _context;

        [Header("Occlusion")]
        public CanvasGroup overlay;
        public float fadeSpeed = 4f;
        private float targetAlpha = 0f;

        private void Awake()
        {
            if (_contextProvider == null)
                _contextProvider = GetComponent<VRUnitContextProvider>();

            if (_contextProvider == null)
                _contextProvider = Object.FindAnyObjectByType<VRUnitContextProvider>();

            if (overlay != null)
                overlay.alpha = 0f;
        }

        private void Update()
        {
            if (_contextProvider == null)
                return;

            _context = _contextProvider.Context;
            if (_context == null || !_context.Valid)
                return;

            TickOcclusion();
        }

        public void AddOcclusion(float strength)
        {
            targetAlpha = Mathf.Clamp01(targetAlpha + strength);
        }

        public void ClearOcclusion()
        {
            targetAlpha = 0f;
        }

        private void TickOcclusion()
        {
            if (overlay == null)
                return;

            overlay.alpha = Mathf.Lerp(overlay.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
        }
    }
}
