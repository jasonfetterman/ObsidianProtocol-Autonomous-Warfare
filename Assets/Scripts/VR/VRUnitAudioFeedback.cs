using UnityEngine;

namespace Obsidian.VR
{
    /// <summary>
    /// Handles audio feedback for VR-controlled units.
    /// Plays movement, damage, battery, and interaction sounds.
    /// </summary>
    public class VRUnitAudioFeedback : MonoBehaviour
    {
        [SerializeField] private VRSessionManager _session;
        [SerializeField] private VRRuntimeAdapter _runtime;

        private BaseUnitVRController _unit;
        private VRUnitContext _context;

        [Header("Audio Sources")]
        public AudioSource movementAudio;
        public AudioSource damageAudio;
        public AudioSource batteryLowAudio;
        public AudioSource interactionAudio;

        private float _lastHealth;
        private float _lastBattery;

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

            if (_unit == null || _context == null)
                return;

            HandleMovementAudio();
            HandleDamageAudio();
            HandleBatteryAudio();
        }

        private void BindToActiveUnit()
        {
            _unit = _session?.ActiveUnit;

            if (_unit == null)
            {
                _context = null;
                return;
            }

            _context = _unit.GetComponent<VRUnitContext>();
            if (_context == null)
                _context = _unit.gameObject.AddComponent<VRUnitContext>();

            _lastHealth = _context.Health;
            _lastBattery = _context.Battery;
        }

        private void HandleMovementAudio()
        {
            if (movementAudio == null)
                return;

            if (_context.IsMoving)
            {
                if (!movementAudio.isPlaying)
                    movementAudio.Play();
            }
            else
            {
                if (movementAudio.isPlaying)
                    movementAudio.Stop();
            }
        }

        private void HandleDamageAudio()
        {
            if (damageAudio == null)
                return;

            if (_context.Health < _lastHealth)
            {
                damageAudio.Play();
            }

            _lastHealth = _context.Health;
        }

        private void HandleBatteryAudio()
        {
            if (batteryLowAudio == null)
                return;

            if (_context.Battery < 0.2f && _lastBattery >= 0.2f)
            {
                batteryLowAudio.Play();
            }

            _lastBattery = _context.Battery;
        }

        public void PlayInteraction()
        {
            interactionAudio?.Play();
        }
    }
}
