using System.Collections.Generic;
using UnityEngine;

namespace ObsidianProtocol.Garage
{
    public class MaintenanceManager : MonoBehaviour
    {
        [Header("Tracked Units")]
        private readonly Dictionary<string, MaintenanceState> states =
            new Dictionary<string, MaintenanceState>();

        public MaintenanceState GetOrCreate(string instanceId)
        {
            if (string.IsNullOrWhiteSpace(instanceId))
                return null;

            if (!states.TryGetValue(instanceId, out MaintenanceState state))
            {
                state = new MaintenanceState();
                states.Add(instanceId, state);
            }

            return state;
        }

        public bool HasUnit(string instanceId)
        {
            return !string.IsNullOrWhiteSpace(instanceId) &&
                   states.ContainsKey(instanceId);
        }

        public void ApplyDamage(
            string instanceId,
            float amount)
        {
            MaintenanceState state =
                GetOrCreate(instanceId);

            if (state == null)
                return;

            state.ApplyDamage(amount);
        }

        public void ApplyWear(
            string instanceId,
            float amount)
        {
            MaintenanceState state =
                GetOrCreate(instanceId);

            if (state == null)
                return;

            state.ApplyWear(amount);
        }

        public void Repair(
            string instanceId,
            float amount)
        {
            MaintenanceState state =
                GetOrCreate(instanceId);

            if (state == null)
                return;

            state.Repair(amount);
        }

        public bool RequiresMaintenance(
            string instanceId)
        {
            MaintenanceState state =
                GetOrCreate(instanceId);

            return state != null &&
                   state.requiresMaintenance;
        }

        public bool IsGrounded(
            string instanceId)
        {
            MaintenanceState state =
                GetOrCreate(instanceId);

            return state != null &&
                   state.grounded;
        }

        public void RemoveUnit(string instanceId)
        {
            if (string.IsNullOrWhiteSpace(instanceId))
                return;

            states.Remove(instanceId);
        }

        public void Clear()
        {
            states.Clear();
        }
    }
}
