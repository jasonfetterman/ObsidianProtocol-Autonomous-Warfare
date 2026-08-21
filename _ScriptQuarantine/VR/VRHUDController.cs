using UnityEngine;

namespace Obsidian.VR
{
    public class VRHUDController : MonoBehaviour
    {
        [Header("HUD Readouts")]
        [SerializeField] private TMPro.TextMeshProUGUI _healthText;
        [SerializeField] private TMPro.TextMeshProUGUI _batteryText;
        [SerializeField] private TMPro.TextMeshProUGUI _speedText;
        [SerializeField] private TMPro.TextMeshProUGUI _headingText;
        [SerializeField] private TMPro.TextMeshProUGUI _ammoText;

        public void UpdateHealth(float value)
        {
            if (_healthText != null)
                _healthText.text = $"HEALTH: {value:0}";
        }

        public void UpdateBattery(float value)
        {
            if (_batteryText != null)
                _batteryText.text = $"BATTERY: {value:0}%";
        }

        public void UpdateSpeed(float value)
        {
            if (_speedText != null)
                _speedText.text = $"SPEED: {value:0.0}";
        }

        public void UpdateHeading(float value)
        {
            if (_headingText != null)
                _headingText.text = $"HEADING: {value:0}";
        }

        public void UpdateAmmo(int value)
        {
            if (_ammoText != null)
                _ammoText.text = $"AMMO: {value}";
        }
    }
}
