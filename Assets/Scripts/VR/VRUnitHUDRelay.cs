using UnityEngine;

namespace Obsidian.VR
{
    public class VRUnitHUDRelay : MonoBehaviour
    {
        [Header("HUD Values")]
        private float _health;
        private float _battery;
        private float _speed;
        private float _heading;
        private int _ammo;

        [Header("HUD Targets")]
        [SerializeField] private VRHUDController _hud;

        private void Awake()
        {
            if (_hud == null)
                _hud = FindAnyObjectByType<VRHUDController>();
        }

        public void SetHealth(float value)
        {
            _health = value;
            UpdateHUD();
        }

        public void SetBattery(float value)
        {
            _battery = value;
            UpdateHUD();
        }

        public void SetSpeed(float value)
        {
            _speed = value;
            UpdateHUD();
        }

        public void SetHeading(float value)
        {
            _heading = value;
            UpdateHUD();
        }

        public void SetAmmo(int value)
        {
            _ammo = value;
            UpdateHUD();
        }

        private void UpdateHUD()
        {
            if (_hud == null)
                return;

            _hud.UpdateHealth(_health);
            _hud.UpdateBattery(_battery);
            _hud.UpdateSpeed(_speed);
            _hud.UpdateHeading(_heading);
            _hud.UpdateAmmo(_ammo);
        }
    }
}
