using UnityEngine;
using System.Collections.Generic;

namespace ObsidianProtocol.UI
{
    public class UINavigationSystem : MonoBehaviour
    {
        public static UINavigationSystem Instance { get; private set; }

        [Header("Navigation Settings")]
        [SerializeField] private GameObject defaultSelection;

        private readonly Stack<GameObject> selectionHistory = new Stack<GameObject>();
        private GameObject currentSelection;

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
        /// Sets the default selection used when UI initializes.
        /// </summary>
        public void SetDefaultSelection(GameObject target)
        {
            defaultSelection = target;
        }

        /// <summary>
        /// Selects a UI element and pushes previous selection to history.
        /// </summary>
        public void Select(GameObject target)
        {
            if (target == null)
                return;

            if (currentSelection != null)
                selectionHistory.Push(currentSelection);

            currentSelection = target;
        }

        /// <summary>
        /// Clears current selection and history.
        /// </summary>
        public void ClearSelection()
        {
            currentSelection = null;
            selectionHistory.Clear();
        }

        /// <summary>
        /// Returns to the previous selection.
        /// </summary>
        public bool GoBack()
        {
            if (selectionHistory.Count == 0)
                return false;

            currentSelection = selectionHistory.Pop();
            return true;
        }

        /// <summary>
        /// Returns the currently selected UI element.
        /// </summary>
        public GameObject GetCurrentSelection()
        {
            return currentSelection;
        }

        /// <summary>
        /// Returns the default selection.
        /// </summary>
        public GameObject GetDefaultSelection()
        {
            return defaultSelection;
        }
    }
}
