using UnityEngine;

namespace Obsidian.VR
{
    public class UnitMover : MonoBehaviour
    {
        private Vector3 _moveInput;

        // ⭐ REQUIRED — fixes your error
        public void SetMoveInput(Vector3 move)
        {
            _moveInput = move;
        }

        public void SetThrottle(float value)
        {
            // Optional throttle logic
        }

        private void Update()
        {
            // Placeholder movement logic
            // transform.position += _moveInput * Time.deltaTime;
        }
    }
}
