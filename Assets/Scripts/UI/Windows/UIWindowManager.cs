using UnityEngine;

namespace ObsidianProtocol.UI
{
    public class UIWindowManager : MonoBehaviour
    {
        public static UIWindowManager Instance { get; private set; }

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
