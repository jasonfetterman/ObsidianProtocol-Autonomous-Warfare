using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Game.Deployment
{
    [DisallowMultipleComponent]
    public sealed class BlackspireChargingStation : MonoBehaviour
    {
        [SerializeField] private bool supportsAirUnits;
        [SerializeField] private float rechargeRate = 0.5f;
        [SerializeField] private Transform[] dockingSlots;

        private readonly Dictionary<Transform, Transform> reservations =
            new Dictionary<Transform, Transform>();

        private static readonly List<BlackspireChargingStation> Stations =
            new List<BlackspireChargingStation>();

        public float RechargeRate => rechargeRate;
        public bool SupportsAirUnits => supportsAirUnits;

        private void OnEnable()
        {
            if (!Stations.Contains(this))
                Stations.Add(this);
        }

        private void OnDisable()
        {
            Stations.Remove(this);
            reservations.Clear();
        }

        public void Configure(bool airStation, float rate, Transform[] slots)
        {
            supportsAirUnits = airStation;
            rechargeRate = Mathf.Max(0.01f, rate);
            dockingSlots = slots;
        }

        public static bool TryReserveNearest(
            Transform unit,
            bool airborne,
            out BlackspireChargingStation station,
            out Transform slot)
        {
            station = null;
            slot = null;

            float nearestDistance = float.MaxValue;

            foreach (BlackspireChargingStation candidate in Stations)
            {
                if (candidate == null ||
                    candidate.supportsAirUnits != airborne)
                    continue;

                if (!candidate.TryReserve(unit, out Transform availableSlot))
                    continue;

                float distance = Vector3.Distance(
                    unit.position,
                    availableSlot.position);

                if (distance < nearestDistance)
                {
                    if (station != null)
                        station.Release(unit);

                    nearestDistance = distance;
                    station = candidate;
                    slot = availableSlot;
                }
                else
                {
                    candidate.Release(unit);
                }
            }

            return station != null;
        }

        private bool TryReserve(Transform unit, out Transform slot)
        {
            if (reservations.TryGetValue(unit, out slot))
                return slot != null;

            if (dockingSlots == null)
                dockingSlots = new Transform[0];

            foreach (Transform candidate in dockingSlots)
            {
                if (candidate == null || reservations.ContainsValue(candidate))
                    continue;

                reservations.Add(unit, candidate);
                slot = candidate;
                return true;
            }

            slot = null;
            return false;
        }

        public void Release(Transform unit)
        {
            if (unit != null)
                reservations.Remove(unit);
        }

        public bool IsReservedBy(Transform unit)
        {
            return unit != null &&
                reservations.ContainsKey(unit);
        }
    }
}
