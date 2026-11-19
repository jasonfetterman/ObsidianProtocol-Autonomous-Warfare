using UnityEngine;

namespace ObsidianProtocol.Game.Deployment
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BlackspireAutonomousPatrol))]
    public sealed class BlackspireUnitBattery : MonoBehaviour
    {
        [SerializeField] private float batteryCapacity = 100f;
        [SerializeField] private float returnThreshold = 40f;
        [SerializeField] private float drainPerSecond = 0.05f;
        [SerializeField] private float dockingDistance = 2.5f;

        private float battery;
        private BlackspireAutonomousPatrol patrol;
        private BlackspireChargingStation station;
        private Transform chargingSlot;
        private bool returning;
        private bool charging;
        private float retryTimer;

        public float BatteryPercent =>
            batteryCapacity > 0f ? battery / batteryCapacity * 100f : 0f;

        private void Awake()
        {
            patrol = GetComponent<BlackspireAutonomousPatrol>();
            batteryCapacity = Mathf.Max(1f, batteryCapacity);
            battery = batteryCapacity;
        }

        private void Update()
        {
            if (patrol == null)
                return;

            if (station != null && !station.isActiveAndEnabled)
            {
                ReleaseStation();
                patrol.enabled = true;
                retryTimer = 5f;
            }

            if (charging)
            {
                if (station == null)
                {
                    ReleaseStation();
                    patrol.enabled = true;
                    retryTimer = 5f;
                    return;
                }

                battery = Mathf.MoveTowards(
                    battery,
                    batteryCapacity,
                    station.RechargeRate * Time.deltaTime);

                if (battery >= batteryCapacity)
                {
                    ReleaseStation();
                    patrol.enabled = true;
                    Debug.Log("[BLACKSPIRE BATTERY] Fully recharged; patrol resumed: " + name, this);
                }

                return;
            }

            if (returning)
            {
                if (station == null || chargingSlot == null)
                {
                    ReleaseStation();
                    patrol.enabled = true;
                    retryTimer = 5f;
                    return;
                }

                float distance = Vector2.Distance(
                    new Vector2(transform.position.x, transform.position.z),
                    new Vector2(chargingSlot.position.x, chargingSlot.position.z));

                if (distance <= dockingDistance)
                {
                    patrol.enabled = false;
                    transform.position = new Vector3(
                        chargingSlot.position.x,
                        transform.position.y,
                        chargingSlot.position.z);

                    charging = true;
                    Debug.Log("[BLACKSPIRE BATTERY] Charging: " + name, this);
                }

                return;
            }

            battery = Mathf.Max(
                0f,
                battery - drainPerSecond * Time.deltaTime);

            if (BatteryPercent <= returnThreshold)
                RequestChargingStation();
        }

        private void RequestChargingStation()
        {
            if (retryTimer > 0f)
            {
                retryTimer -= Time.deltaTime;
                return;
            }

            if (!BlackspireChargingStation.TryReserveNearest(
                transform,
                patrol.IsAirborne,
                out station,
                out chargingSlot))
            {
                retryTimer = 5f;
                return;
            }

            if (!patrol.TrySetReturnDestination(chargingSlot.position))
            {
                ReleaseStation();
                retryTimer = 5f;
                return;
            }

            returning = true;
            patrol.enabled = true;

            Debug.Log("[BLACKSPIRE BATTERY] Returning to charger: " + name, this);
        }

        private void ReleaseStation()
        {
            if (station != null)
                station.Release(transform);

            station = null;
            chargingSlot = null;
            returning = false;
            charging = false;
        }

        private void OnDestroy()
        {
            ReleaseStation();
        }
    }
}
