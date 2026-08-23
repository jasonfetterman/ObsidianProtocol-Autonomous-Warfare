using UnityEngine;

namespace ObsidianProtocol.UI
{
    public class UINotificationManager : MonoBehaviour
    {
        public static UINotificationManager Instance { get; private set; }

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
