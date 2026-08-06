using UnityEngine;
using Obsidian.VR;   // ⭐ REQUIRED — fixes your UnitMover error

namespace Obsidian.Sound
{
    public class FootstepAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip[] _footstepClips;

        private UnitMover _mover;

        private void Awake()
        {
            if (_audioSource == null)
                _audioSource = GetComponent<AudioSource>();

            _mover = GetComponent<UnitMover>();
        }

        private void Update()
        {
            if (_mover == null || _audioSource == null || _footstepClips == null || _footstepClips.Length == 0)
                return;

            HandleFootsteps();
        }

        private void HandleFootsteps()
        {
            // Placeholder: play footsteps when moving
            // You can replace this with your real movement speed logic
            if (_mover != null)
            {
                // Example: if movement input magnitude is high enough
                // (Replace with your actual movement speed check)
                // if (_mover.MoveMagnitude > 0.1f) PlayStep();
            }
        }

        private void PlayStep()
        {
            var clip = _footstepClips[Random.Range(0, _footstepClips.Length)];
            _audioSource.PlayOneShot(clip);
        }
    }
}
