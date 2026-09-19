using UnityEngine;

namespace ObsidianProtocol.UI
{
    public class UIFocusSystem : MonoBehaviour
    {
        public static UIFocusSystem Instance { get; private set; }

        private GameObject currentFocus;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        /// <summary>
        /// Sets focus on a UI element.
        /// </summary>
        public void SetFocus(GameObject target)
        {
            if (target == null)
                return;

            currentFocus = target;
        }

        /// <summary>
        /// Clears the current focus.
        /// </summary>
        public void ClearFocus()
        {
            currentFocus = null;
        }

        /// <summary>
        /// Returns the currently focused UI element.
        /// </summary>
        public GameObject GetFocus()
        {
            return currentFocus;
        }
    }
}
