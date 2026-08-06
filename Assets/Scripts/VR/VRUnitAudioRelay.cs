using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Routes VR input events to the VRUnitAudioFeedback component.
    /// Handles interaction, firing, and movement audio triggers.
    /// </summary>
    public class VRUnitAudioRelay : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;
        private VRUnitAudioFeedback _audio;

        private void Awake()
        {
            if (_session == null)
                _session = Object.FindAnyObjectByType<VRSessionManager>();

            if (_runtime == null)
                _runtime = Object.FindAnyObjectByType<VRRuntimeAdapter>();
        }

        private void Start()
        {
            BindToActiveUnit();
        }

        private void Update()
        {
            if (_session == null || _runtime == null)
                return;

            if (_unit == null)
                BindToActiveUnit();

            if (_unit == null || _audio == null)
                return;

            RouteAudioEvents();
        }

        private void BindToActiveUnit()
        {
            _unit = _session?.ActiveUnit;

            if (_unit == null)
            {
                _audio = null;
                return;
            }

            _audio = _unit.GetComponent<VRUnitAudioFeedback>();
            if (_audio == null)
                _audio = _unit.gameObject.AddComponent<VRUnitAudioFeedback>();
        }

        private void RouteAudioEvents()
        {
            // Interaction audio
            if (_runtime.IsInteractPressed())
                _audio.PlayInteraction();

            // Firing audio (weapon relay handles actual firing)
            if (_runtime.IsTriggerPressed())
                _audio.PlayInteraction(); // optional: replace with weapon-specific audio

            // Movement audio is handled internally by VRUnitAudioFeedback
        }
    }
}
