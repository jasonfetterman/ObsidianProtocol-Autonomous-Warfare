using UnityEngine;

namespace ObsidianProtocol.UI
{
    public class UIInputManager : MonoBehaviour
    {
        public static UIInputManager Instance { get; private set; }

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
