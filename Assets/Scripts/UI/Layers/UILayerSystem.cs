using UnityEngine;
using System.Collections.Generic;

namespace ObsidianProtocol.UI
{
    [System.Serializable]
    public class UILayer
    {
        public string id;
        public GameObject root;
    }

    public class UILayerSystem : MonoBehaviour
    {
        public static UILayerSystem Instance { get; private set; }

        [Header("Registered Layers")]
        [SerializeField] private List<UILayer> layers = new List<UILayer>();

        private readonly Dictionary<string, UILayer> lookup = new Dictionary<string, UILayer>();
        public UILayer ActiveLayer { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            // Build lookup table
            foreach (var layer in layers)
            {
                if (layer != null && !string.IsNullOrEmpty(layer.id))
                {
                    lookup[layer.id] = layer;

                    if (layer.root != null)
                        layer.root.SetActive(false);
                }
            }
        }

        /// <summary>
        /// Sets the active layer by ID.
        /// </summary>
        public void SetLayer(string layerId)
        {
            if (!lookup.TryGetValue(layerId, out var layer))
            {
                Debug.LogWarning($"[UILayerSystem] Layer not found: {layerId}");
                return;
            }

            // Disable all layers
            foreach (var l in layers)
            {
                if (l != null && l.root != null)
                    l.root.SetActive(false);
            }

            // Enable target layer
            if (layer.root != null)
                layer.root.SetActive(true);

            ActiveLayer = layer;
        }

        /// <summary>
        /// Clears the active layer.
        /// </summary>
        public void ClearLayer()
        {
            if (ActiveLayer != null && ActiveLayer.root != null)
                ActiveLayer.root.SetActive(false);

            ActiveLayer = null;
        }

        /// <summary>
        /// Returns the currently active layer ID.
        /// </summary>
        public string GetActiveLayerId()
        {
            return ActiveLayer != null ? ActiveLayer.id : null;
        }
    }
}
