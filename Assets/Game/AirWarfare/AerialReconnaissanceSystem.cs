using System;
using System.Collections.Generic;

namespace ObsidianProtocol.Game.AirWarfare
{
    public enum ReconnaissanceState
    {
        Idle,
        Searching,
        Investigating,
        Reporting,
        Returning
    }

    public sealed class AerialReconnaissanceState
    {
        public string UnitId { get; }

        public ReconnaissanceState State { get; private set; }

        public float SearchRadius { get; private set; }
        public float SearchAltitude { get; private set; }

        public string AssignedArea { get; private set; }
        public string LastContact { get; private set; }

        public AerialReconnaissanceState(string unitId)
        {
            UnitId = unitId ?? string.Empty;

            State = ReconnaissanceState.Idle;
            AssignedArea = string.Empty;
            LastContact = string.Empty;
        }

        public void Configure(
            float searchRadius,
            float searchAltitude)
        {
            SearchRadius =
                Math.Max(0f, searchRadius);

            SearchAltitude =
                Math.Max(0f, searchAltitude);
        }

        public void AssignArea(string areaId)
        {
            AssignedArea =
                areaId ?? string.Empty;

            State =
                string.IsNullOrWhiteSpace(AssignedArea)
                    ? ReconnaissanceState.Idle
                    : ReconnaissanceState.Searching;
        }

        public void RecordContact(string contactId)
        {
            if (string.IsNullOrWhiteSpace(contactId))
            {
                return;
            }

            LastContact = contactId;
            State = ReconnaissanceState.Investigating;
        }

        public void BeginReporting()
        {
            State = ReconnaissanceState.Reporting;
        }

        public void ReturnToBase()
        {
            State = ReconnaissanceState.Returning;
        }

        public void Reset()
        {
            State = ReconnaissanceState.Idle;
            AssignedArea = string.Empty;
            LastContact = string.Empty;
        }
    }

    public sealed class AerialReconnaissanceSystem
    {
        private readonly Dictionary<string, AerialReconnaissanceState> states =
            new Dictionary<string, AerialReconnaissanceState>(
                StringComparer.OrdinalIgnoreCase);

        public void RegisterDrone(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            if (!states.ContainsKey(unitId))
            {
                states.Add(
                    unitId,
                    new AerialReconnaissanceState(unitId));
            }
        }

        public void ConfigureDrone(
            string unitId,
            float searchRadius,
            float searchAltitude)
        {
            RegisterDrone(unitId);

            states[unitId].Configure(
                searchRadius,
                searchAltitude);
        }

        public void AssignArea(
            string unitId,
            string areaId)
        {
            RegisterDrone(unitId);

            states[unitId].AssignArea(areaId);
        }

        public void RecordContact(
            string unitId,
            string contactId)
        {
            RegisterDrone(unitId);

            states[unitId].RecordContact(contactId);
        }

        public void BeginReporting(string unitId)
        {
            if (states.TryGetValue(
                    unitId,
                    out AerialReconnaissanceState state))
            {
                state.BeginReporting();
            }
        }

        public void ReturnToBase(string unitId)
        {
            if (states.TryGetValue(
                    unitId,
                    out AerialReconnaissanceState state))
            {
                state.ReturnToBase();
            }
        }

        public bool TryGetState(
            string unitId,
            out AerialReconnaissanceState state)
        {
            return states.TryGetValue(
                unitId,
                out state);
        }

        public void RemoveDrone(string unitId)
        {
            states.Remove(unitId);
        }

        public void Clear()
        {
            states.Clear();
        }
    }
}
