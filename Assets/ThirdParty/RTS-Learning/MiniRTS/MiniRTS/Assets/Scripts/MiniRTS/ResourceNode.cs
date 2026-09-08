using System.Collections.Generic;
using UnityEngine;

namespace MiniRTS
{
    /// <summary>
    /// Finite mineral crystal or gas geyser harvested in worker-sized batches.
    /// Raw geysers remain locked until a later Refinery marks them available.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class ResourceNode : MonoBehaviour
    {
        private static readonly List<ResourceNode> Nodes = new List<ResourceNode>();

        [SerializeField] private ResourceType resourceType;
        [SerializeField] private int amount;
        [SerializeField] private bool requiresRefinery;
        [SerializeField] private bool refineryPresent;
        [SerializeField] private float interactionRadius;

        public static IReadOnlyList<ResourceNode> ActiveNodes => Nodes;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetForPlaySession()
        {
            ResetStaticState();
        }

        internal static void ResetStaticState()
        {
            Nodes.Clear();
        }
        public ResourceType Type => resourceType;
        public int Amount => amount;
        public bool RequiresRefinery => requiresRefinery;
        public bool RefineryPresent => refineryPresent;
        public bool IsHarvestable =>
            amount > 0 && (!requiresRefinery || refineryPresent);
        public float InteractionRadius => interactionRadius;
        public float HarvestSeconds => BalanceConfig.HarvestSeconds;
        public int CarryCapacity => BalanceConfig.HarvestCarryCapacity;

        private void OnEnable()
        {
            if (!Nodes.Contains(this))
            {
                Nodes.Add(this);
            }
        }

        private void OnDisable()
        {
            Nodes.Remove(this);
        }

        public void Initialize(
            ResourceType type,
            int startingAmount,
            bool needsRefinery,
            float approachRadius)
        {
            if (startingAmount < 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(startingAmount));
            }

            if (approachRadius <= 0f)
            {
                throw new System.ArgumentOutOfRangeException(nameof(approachRadius));
            }

            resourceType = type;
            amount = startingAmount;
            requiresRefinery = needsRefinery;
            refineryPresent = !needsRefinery;
            interactionRadius = approachRadius;
        }

        public void SetRefineryPresent(bool present)
        {
            refineryPresent = present;
        }

        public bool TryHarvest(int requestedAmount, out int harvestedAmount)
        {
            if (requestedAmount <= 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(requestedAmount));
            }

            if (!IsHarvestable)
            {
                harvestedAmount = 0;
                return false;
            }

            harvestedAmount = Mathf.Min(requestedAmount, amount);
            amount -= harvestedAmount;
            return harvestedAmount > 0;
        }

        public bool IsInInteractionRange(Vector3 worldPosition)
        {
            Vector3 difference = worldPosition - transform.position;
            difference.y = 0f;
            return difference.sqrMagnitude <= interactionRadius * interactionRadius;
        }
    }
}
