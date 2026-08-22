using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Graph
{
    public sealed class NavigationGraph : MonoBehaviour
    {
        [SerializeField] private NavigationGraphDefinition definition;
        [SerializeField] private List<Transform> nodes = new();

        public NavigationGraphDefinition Definition => definition;
        public IReadOnlyList<Transform> Nodes => nodes;

        public void RegisterNode(Transform node)
        {
            if (node == null || nodes.Contains(node))
            {
                return;
            }

            if (definition != null &&
                nodes.Count >= definition.MaximumNodes)
            {
                return;
            }

            nodes.Add(node);
        }

        public void UnregisterNode(Transform node)
        {
            if (node == null)
            {
                return;
            }

            nodes.Remove(node);
        }
    }
}
