using UnityEngine;

namespace ObsidianProtocol.UI
{
    public class UIModalManager : MonoBehaviour
    {
        public static UIModalManager Instance { get; private set; }

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
