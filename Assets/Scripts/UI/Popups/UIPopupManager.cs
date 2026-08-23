using UnityEngine;

namespace ObsidianProtocol.UI
{
    public class UIPopupManager : MonoBehaviour
    {
        public static UIPopupManager Instance { get; private set; }

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
