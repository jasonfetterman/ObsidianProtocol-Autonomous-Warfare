using UnityEngine;

namespace ObsidianProtocol.UI
{
    public class UIOverlayManager : MonoBehaviour
    {
        public static UIOverlayManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
    }
}
