using UnityEngine;

namespace Obsidian.VR
{
    public class DroneHUDVR : MonoBehaviour
    {
        [Header("HUD Elements")]
        public Canvas hudCanvas;
        public RectTransform root;

        [Header("Runtime")]
        public bool IsEnabled = false;

        private void Awake()
        {
            if (hudCanvas == null)
                hudCanvas = GetComponentInChildren<Canvas>();
        }

        // ---------------------------------------------------------
        // REQUIRED BY YOUR ERROR LOG
        // ---------------------------------------------------------

        public void EnableHUD()
        {
            IsEnabled = true;

            if (hudCanvas != null)
                hudCanvas.enabled = true;

            if (root != null)
                root.gameObject.SetActive(true);
        }

        public void DisableHUD()
        {
            IsEnabled = false;

            if (hudCanvas != null)
                hudCanvas.enabled = false;

            if (root != null)
                root.gameObject.SetActive(false);
        }

        public void TickHUD(float dt)
        {
            if (!IsEnabled)
                return;

            // placeholder update loop
        }
    }
}
