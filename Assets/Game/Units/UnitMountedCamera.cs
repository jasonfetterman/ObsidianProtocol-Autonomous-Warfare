using UnityEngine;

namespace ObsidianProtocol.Game.Units
{
    [DisallowMultipleComponent]
    public sealed class UnitMountedCamera : MonoBehaviour
    {
        [SerializeField] private Camera unitCamera;

        public Camera Camera => unitCamera;

        public void SetCamera(Camera value)
        {
            unitCamera = value;
        }

        private void Reset()
        {
            unitCamera = GetComponent<Camera>();
        }

        private void Awake()
        {
            if (unitCamera == null)
                unitCamera = GetComponent<Camera>();
        }
    }
}
