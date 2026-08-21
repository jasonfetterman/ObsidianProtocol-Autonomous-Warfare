using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class ServiceBayManager : MonoBehaviour
    {
        [Header("Service Bays")]
        [SerializeField]
        private List<ServiceBayState> bays =
            new List<ServiceBayState>();

        public IReadOnlyList<ServiceBayState> Bays =>
            bays;

        public int Count =>
            bays.Count;

        public ServiceBayState AddBay(
            string bayId,
            string bayName)
        {
            if (string.IsNullOrWhiteSpace(bayId))
                return null;

            ServiceBayState existing =
                GetBay(bayId);

            if (existing != null)
                return existing;

            ServiceBayState bay =
                new ServiceBayState
                {
                    bayId = bayId,
                    bayName = bayName
                };

            bays.Add(bay);

            return bay;
        }

        public ServiceBayState GetBay(
            string bayId)
        {
            if (string.IsNullOrWhiteSpace(bayId))
                return null;

            foreach (ServiceBayState bay in bays)
            {
                if (bay == null)
                    continue;

                if (bay.bayId == bayId)
                    return bay;
            }

            return null;
        }

        public ServiceBayState FindAvailableBay()
        {
            foreach (ServiceBayState bay in bays)
            {
                if (bay == null)
                    continue;

                if (bay.available &&
                    !bay.occupied)
                    return bay;
            }

            return null;
        }

        public bool AssignUnit(
            string bayId,
            string unitInstanceId)
        {
            ServiceBayState bay =
                GetBay(bayId);

            if (bay == null ||
                !bay.available ||
                string.IsNullOrWhiteSpace(unitInstanceId))
                return false;

            bay.AssignUnit(unitInstanceId);

            return true;
        }

        public bool AssignUnitToAvailableBay(
            string unitInstanceId)
        {
            if (string.IsNullOrWhiteSpace(unitInstanceId))
                return false;

            ServiceBayState bay =
                FindAvailableBay();

            if (bay == null)
                return false;

            bay.AssignUnit(unitInstanceId);

            return true;
        }

        public void ReleaseBay(
            string bayId)
        {
            ServiceBayState bay =
                GetBay(bayId);

            if (bay == null)
                return;

            bay.ClearUnit();
        }

        public bool IsUnitInService(
            string unitInstanceId)
        {
            if (string.IsNullOrWhiteSpace(unitInstanceId))
                return false;

            foreach (ServiceBayState bay in bays)
            {
                if (bay == null)
                    continue;

                if (bay.activeUnitInstanceId ==
                    unitInstanceId)
                    return bay.servicing;
            }

            return false;
        }

        public void Clear()
        {
            bays.Clear();
        }
    }
}
