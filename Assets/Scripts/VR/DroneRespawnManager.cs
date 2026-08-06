using UnityEngine;
using System.Collections.Generic;
using Obsidian.VR;   // REQUIRED FIX — this makes DroneHealth visible

namespace Assets.Scripts.VR
{
    public class DroneRespawnManager : MonoBehaviour
    {
        public List<Transform> spawnPoints;
        public float respawnDelay = 5f;

        private Dictionary<DroneHealth, float> respawnTimers = new Dictionary<DroneHealth, float>();

        void Update()
        {
            HandleRespawns();
        }

        public void RegisterDrone(DroneHealth drone)
        {
            if (!respawnTimers.ContainsKey(drone))
                respawnTimers.Add(drone, -1f);
        }

        public void OnDroneDestroyed(DroneHealth drone)
        {
            if (respawnTimers.ContainsKey(drone))
                respawnTimers[drone] = respawnDelay;
        }

        private void HandleRespawns()
        {
            List<DroneHealth> ready = new List<DroneHealth>();

            foreach (var kvp in respawnTimers)
            {
                DroneHealth drone = kvp.Key;
                float timer = kvp.Value;

                if (timer < 0f)
                    continue;

                timer -= Time.deltaTime;
                respawnTimers[drone] = timer;

                if (timer <= 0f)
                    ready.Add(drone);
            }

            foreach (var drone in ready)
                RespawnDrone(drone);
        }

        private void RespawnDrone(DroneHealth drone)
        {
            if (spawnPoints.Count == 0)
                return;

            Transform point = spawnPoints[Random.Range(0, spawnPoints.Count)];

            // Reset health
            drone.ResetHealth();

            // Reset physics
            Rigidbody rb = drone.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.useGravity = false;
            }

            // Reset position
            drone.transform.position = point.position;
            drone.transform.rotation = point.rotation;

            // Re-enable visuals and colliders
            foreach (var renderer in drone.GetComponentsInChildren<Renderer>())
                renderer.enabled = true;

            foreach (var collider in drone.GetComponentsInChildren<Collider>())
                collider.enabled = true;

            // Clear respawn timer
            respawnTimers[drone] = -1f;
        }
    }
}
