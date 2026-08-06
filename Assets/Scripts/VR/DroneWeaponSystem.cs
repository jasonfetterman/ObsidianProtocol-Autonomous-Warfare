using UnityEngine;

namespace Obsidian.VR
{
    public class DroneWeaponSystem : MonoBehaviour
    {
        [Header("Weapon Settings")]
        public float fireRate = 0.1f;
        public float heatPerShot = 0.05f;
        public float maxHeat = 1f;

        private float _heat = 0f;
        private float _coolRate = 0.25f;
        private float _nextFireTime = 0f;

        public bool IsFiring { get; private set; }
        public bool IsJammed { get; private set; }
        public int AmmoCount { get; private set; } = 999;

        private void Update()
        {
            CoolWeapon();
        }

        private void CoolWeapon()
        {
            if (_heat > 0f)
                _heat = Mathf.Max(0f, _heat - _coolRate * Time.deltaTime);

            if (_heat >= maxHeat)
                IsJammed = true;

            if (_heat <= 0.1f)
                IsJammed = false;
        }

        public void FirePrimary()
        {
            if (IsJammed)
                return;

            if (AmmoCount <= 0)
                return;

            if (Time.time < _nextFireTime)
                return;

            _nextFireTime = Time.time + fireRate;
            _heat = Mathf.Clamp01(_heat + heatPerShot);
            AmmoCount--;

            IsFiring = true;
        }

        public void StopFiring()
        {
            IsFiring = false;
        }

        public float GetWeaponHeat()
        {
            return _heat;
        }

        public int GetAmmo()
        {
            return AmmoCount;
        }

        // ---------------------------------------------------------
        // REQUIRED BY YOUR ERROR LOG
        // ---------------------------------------------------------

        public void FireAt(Vector3 targetPosition)
        {
            if (IsJammed)
                return;

            if (AmmoCount <= 0)
                return;

            if (Time.time < _nextFireTime)
                return;

            _nextFireTime = Time.time + fireRate;
            _heat = Mathf.Clamp01(_heat + heatPerShot);
            AmmoCount--;

            IsFiring = true;
        }
    }
}
