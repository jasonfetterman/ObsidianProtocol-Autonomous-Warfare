using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Navigation.Queries
{
    public sealed class NavigationQueryService : MonoBehaviour
    {
        [SerializeField] private NavigationQueryDefinition definition;

        public NavigationQueryDefinition Definition => definition;

        public bool IsWithinSearchDistance(
            Vector3 origin,
            Vector3 target)
        {
            if (definition == null)
            {
                return true;
            }

            return Vector3.Distance(origin, target) <=
                   definition.MaximumSearchDistance;
        }

        public int LimitResults(int resultCount)
        {
            if (definition == null)
            {
                return Mathf.Max(0, resultCount);
            }

            return Mathf.Min(
                Mathf.Max(0, resultCount),
                definition.MaximumResults);
        }
    }
}
